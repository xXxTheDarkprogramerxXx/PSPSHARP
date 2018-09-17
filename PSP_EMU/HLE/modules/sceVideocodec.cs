﻿using System;
using System.Threading;

/*
This file is part of pspsharp.

pspsharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

pspsharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with pspsharp.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace pspsharp.HLE.modules
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceMpeg.getIntBuffer;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceMpeg.releaseIntBuffer;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.alignUp;


	//using Logger = org.apache.log4j.Logger;

	using RuntimeContext = pspsharp.Allegrex.compiler.RuntimeContext;
	using LengthInfo = pspsharp.HLE.BufferInfo.LengthInfo;
	using Usage = pspsharp.HLE.BufferInfo.Usage;
	using IAction = pspsharp.HLE.kernel.types.IAction;
	using SceKernelThreadInfo = pspsharp.HLE.kernel.types.SceKernelThreadInfo;
	using SysMemInfo = pspsharp.HLE.modules.SysMemUserForUser.SysMemInfo;
	using CodecFactory = pspsharp.media.codec.CodecFactory;
	using IVideoCodec = pspsharp.media.codec.IVideoCodec;
	using IMemoryReader = pspsharp.memory.IMemoryReader;
	using IMemoryWriter = pspsharp.memory.IMemoryWriter;
	using MemoryReader = pspsharp.memory.MemoryReader;
	using MemoryWriter = pspsharp.memory.MemoryWriter;
	using DelayThreadAction = pspsharp.scheduler.DelayThreadAction;
	using UnblockThreadAction = pspsharp.scheduler.UnblockThreadAction;
	using Utilities = pspsharp.util.Utilities;

	public class sceVideocodec : HLEModule
	{
		//public static Logger log = Modules.getLogger("sceVideocodec");
		private const int videocodecDecodeDelay = 4000;
		// Based on JpcspTrace tests, sceVideocodecDelete delays for 40ms
		public const int videocodecDeleteDelay = 40000;
		public const int EDRAM_MEMORY_MASK = 0x03FFFFFF;
		public const int VIDEOCODEC_OPEN_TYPE0_UNKNOWN24 = 0x3C2C;
		public const int VIDEOCODEC_OPEN_TYPE0_UNKNOWN0 = 0x1F6400;
		public const int VIDEOCODEC_OPEN_TYPE0_UNKNOWN4 = 0x15C00;
		public const int VIDEOCODEC_OPEN_TYPE1_UNKNOWN24 = 0x264C;
		public const int VIDEOCODEC_OPEN_TYPE1_UNKNOWN32 = 0xB69E3;
		protected internal SysMemInfo memoryInfo;
		protected internal SysMemInfo edramInfo;
		protected internal int frameCount;
		protected internal int bufferY1;
		protected internal int bufferY2;
		protected internal int bufferCr1;
		protected internal int bufferCr2;
		protected internal int bufferCb1;
		protected internal int bufferCb2;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: protected internal readonly int[][] buffers = new int[4][8];
		protected internal readonly int[][] buffers = RectangularArrays.ReturnRectangularIntArray(4, 8);
		protected internal int bufferUnknown1;
		protected internal int bufferUnknown2;
		protected internal IVideoCodec videoCodec;
		private VideocodecDecoderThread videocodecDecoderThread;

		private class VideocodecDecoderThread : Thread
		{
			private readonly sceVideocodec outerInstance;

			public VideocodecDecoderThread(sceVideocodec outerInstance)
			{
				this.outerInstance = outerInstance;
			}

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
			internal volatile bool exit_Renamed = false;
			internal volatile bool done = false;
			internal Semaphore sema = new Semaphore(1);
			internal TPointer buffer;
			internal int type;
			internal int threadUid;
			internal long threadWakeupMicroTime;

			public override void run()
			{
				while (!exit_Renamed)
				{
					if (waitForTrigger(100) && !exit_Renamed)
					{
						outerInstance.hleVideocodecDecoderStep(buffer, type, threadUid, threadWakeupMicroTime);
					}
				}

				//if (log.DebugEnabled)
				{
					Console.WriteLine("Exiting the VideocodecDecoderThread");
				}
				done = true;
			}

			public virtual void exit()
			{
				exit_Renamed = true;

				while (!done)
				{
					Utilities.sleep(1);
				}
			}

			public virtual void trigger(TPointer buffer, int type, int threadUid, long threadWakeupMicroTime)
			{
				this.buffer = buffer;
				this.type = type;
				this.threadUid = threadUid;
				this.threadWakeupMicroTime = threadWakeupMicroTime;

				trigger();
			}

			internal virtual void trigger()
			{
				if (sema != null)
				{
					sema.release();
				}
			}

			internal virtual bool waitForTrigger(int millis)
			{
				while (true)
				{
					try
					{
						int availablePermits = sema.drainPermits();
						if (availablePermits > 0)
						{
							break;
						}

						if (sema.tryAcquire(millis, TimeUnit.MILLISECONDS))
						{
							break;
						}

						return false;
					}
					catch (InterruptedException)
					{
						// Ignore exception and retry
					}
				}

				return true;
			}
		}

		private void hleVideocodecDecoderStep(TPointer buffer, int type, int threadUid, long threadWakeupMicroTime)
		{
			if (buffer == null)
			{
				return;
			}

			int mp4Data = buffer.getValue32(36) | MemoryMap.START_RAM;
			int mp4Size = buffer.getValue32(40);

			if (log.TraceEnabled)
			{
				log.trace(string.Format("sceVideocodecDecode mp4Data:{0}", Utilities.getMemoryDump(mp4Data, mp4Size)));
			}

			if (videoCodec == null)
			{
				videoCodec = CodecFactory.VideoCodec;
				videoCodec.init(null);
			}

			int[] mp4Buffer = getIntBuffer(mp4Size);
			IMemoryReader memoryReader = MemoryReader.getMemoryReader(mp4Data, mp4Size, 1);
			for (int i = 0; i < mp4Size; i++)
			{
				mp4Buffer[i] = memoryReader.readNext();
			}

			int result = videoCodec.decode(mp4Buffer, 0, mp4Size);
			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceVideocodecDecode videoCodec returned 0x{0:X} from 0x{1:X} data bytes", result, mp4Size));
			}

			releaseIntBuffer(mp4Buffer);

			buffer.setValue32(8, 0);

			int frameWidth = videoCodec.ImageWidth;
			int frameHeight = videoCodec.ImageHeight;

			if (log.TraceEnabled)
			{
				log.trace(string.Format("sceVideocodecDecode codec image size {0:D}x{1:D}, frame size {2:D}x{3:D}", videoCodec.ImageWidth, videoCodec.ImageHeight, frameWidth, frameHeight));
			}
			int frameBufferWidthY = videoCodec.ImageWidth;
			int frameBufferWidthCr = frameBufferWidthY / 2;
			int frameBufferWidthCb = frameBufferWidthY / 2;

			Memory mem = buffer.Memory;
			TPointer buffer2 = buffer.getPointer(16);
			switch (type)
			{
				case 0:
					buffer2.setValue32(8, frameWidth);
					buffer2.setValue32(12, frameHeight);
					buffer2.setValue32(28, 1);
					buffer2.setValue32(32, videoCodec.hasImage());
					buffer2.setValue32(36, !videoCodec.hasImage());

					if (videoCodec.hasImage())
					{
						if (memoryInfo == null)
						{
							int sizeY1 = alignUp(((frameWidth + 16) >> 5) * (frameHeight >> 1) * 16, 0x1FF);
							int sizeY2 = alignUp((frameWidth >> 5) * (frameHeight >> 1) * 16, 0x1FF);
							int sizeCr1 = alignUp(((frameWidth + 16) >> 5) * (frameHeight >> 1) * 8, 0x1FF);
							int sizeCr2 = alignUp((frameWidth >> 5) * (frameHeight >> 1) * 8, 0x1FF);
							int size = 256 + (sizeY1 + sizeY2 + sizeCr1 + sizeCr2) * 2 * buffers.Length;

							memoryInfo = Modules.SysMemUserForUserModule.malloc(SysMemUserForUser.KERNEL_PARTITION_ID, "sceVideocodecDecode", SysMemUserForUser.PSP_SMEM_Low, size, 0);

							int @base = memoryInfo.addr;

							bufferUnknown1 = @base;
							mem.memset(bufferUnknown1, (sbyte) 0, 36);

							bufferUnknown2 = @base + 36;
							mem.memset(bufferUnknown2, (sbyte) 0, 32);

							int yuvBuffersBase = @base + 256; // Add 256 to keep aligned
							int base1 = yuvBuffersBase & EDRAM_MEMORY_MASK;
							int base2 = base1 + (sizeY1 + sizeY2) * buffers.Length;
							int step = (sizeY1 + sizeY2 + sizeCr1 + sizeCr2) * buffers.Length;
							for (int i = 0; i < buffers.Length; i++)
							{
								buffers[i][0] = base1;
								buffers[i][1] = buffers[i][0] + step;
								buffers[i][2] = base1 + sizeY1;
								buffers[i][3] = buffers[i][2] + step;
								buffers[i][4] = base2;
								buffers[i][5] = buffers[i][4] + step;
								buffers[i][6] = base2 + sizeCr1;
								buffers[i][7] = buffers[i][6] + step;

								base1 += sizeY1 + sizeY2;
								base2 += sizeCr1 + sizeCr2;
							}
						}

						int buffersIndex = frameCount % 3;
						int width = videoCodec.ImageWidth;
						int height = videoCodec.ImageHeight;

						int[] luma = getIntBuffer(width * height);
						int[] cb = getIntBuffer(width * height / 4);
						int[] cr = getIntBuffer(width * height / 4);
						if (videoCodec.getImage(luma, cb, cr) == 0)
						{
							// The PSP is storing the YCbCr information in a non-linear format.
							// By analyzing the output of sceMpegBaseYCrCbCopy on a real PSP,
							// the following format for the YCbCr was found:
							// the image is divided vertically into bands of 32 pixels.
							// Each band is stored vertically into different buffers.
							// The Y information is stored as 1 byte per pixel.
							// The Cb information is stored as 1 byte for a square of four pixels (2x2).
							// The Cr information is stored as 1 byte for a square of four pixels (2x2).
							// For a square of four pixels, the one Cb byte is stored first,
							// followed by the one Cr byte.
							//
							// - buffer0:
							//     storing the Y information of the first block
							//     of 16 pixels of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=0,y=0),
							//     16 horizontal pixels are stored sequentially in the buffer,
							//     followed by 16 pixels of the next next image row (i.e. every 2nd row).
							//     The rows are stored from the image top to the image bottom.
							//     [x=0-15,y=0], [x=0-15,y=2], [x=0-15,y=4]...
							//     [x=32-47,y=0], [x=32-47,y=2], [x=32-47,y=4]...
							//     [x=64-79,y=0], [x=64-79,y=2], [x=64-79,y=4]...
							// - buffer1:
							//     storing the Y information of the second block
							//     of 16 pixels of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=16,y=0),
							//     16 horizontal pixels are stored sequentially in the buffer,
							//     followed by 16 pixels of the next next image row (i.e. every 2nd row).
							//     The rows are stored from the image top to the image bottom.
							//     [x=16-31,y=0], [x=16-31,y=2], [x=16-31,y=4]...
							//     [x=48-63,y=0], [x=48-63,y=2], [x=48-63,y=4]...
							//     [x=80-95,y=0], [x=80-95,y=2], [x=80-95,y=4]...
							// - buffer2:
							//     storing the Y information of the first block of 16 pixels
							//     of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=0,y=1),
							//     16 horizontal pixels are stored sequentially in the buffer,
							//     followed by 16 pixels of the next next image row (i.e. every 2nd row).
							//     The rows are stored from the image top to the image bottom.
							//     [x=0-15,y=1], [x=0-15,y=3], [x=0-15,y=5]...
							//     [x=32-47,y=1], [x=32-47,y=3], [x=32-47,y=5]...
							//     [x=64-79,y=1], [x=64-79,y=3], [x=64-79,y=5]...
							// - buffer3:
							//     storing the Y information of the second block of 16 pixels
							//     of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=16,y=1),
							//     16 horizontal pixels are stored sequentially in the buffer,
							//     followed by 16 pixels of the next next image row (i.e. every 2nd row).
							//     The rows are stored from the image top to the image bottom.
							//     [x=16-31,y=1], [x=16-31,y=3], [x=16-31,y=5]...
							//     [x=48-63,y=1], [x=48-63,y=3], [x=48-63,y=5]...
							//     [x=80-95,y=1], [x=80-95,y=3], [x=80-95,y=5]...
							// - buffer4:
							//     storing the Cb and Cr information of the first block
							//     of 16 pixels of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=0,y=0),
							//     8 byte pairs of (Cb,Cr) are stored sequentially in the buffer
							//     (representing 16 horizontal pixels),
							//     then the next 3 rows are being skipped,
							//     and then followed by 8 byte pairs of the next image row (i.e. every 4th row).
							//     The rows are stored from the image top to the image bottom.
							//     CbCr[x=0,y=0], CbCr[x=2,y=0], CbCr[x=4,y=0], CbCr[x=6,y=0], CbCr[x=8,y=0], CbCr[x=10,y=0], CbCr[x=12,y=0], CbCr[x=14,y=0]
							//     CbCr[x=32,y=0], CbCr[x=34,y=0], CbCr[x=36,y=0], CbCr[x=38,y=0], CbCr[x=40,y=0], CbCr[x=42,y=0], CbCr[x=44,y=0], CbCr[x=46,y=0]
							//     ...
							//     CbCr[x=0,y=4], CbCr[x=2,y=4], CbCr[x=4,y=4], CbCr[x=6,y=4], CbCr[x=8,y=4], CbCr[x=10,y=4], CbCr[x=12,y=4], CbCr[x=14,y=4]
							//     CbCr[x=32,y=4], CbCr[x=34,y=4], CbCr[x=36,y=4], CbCr[x=38,y=4], CbCr[x=40,y=4], CbCr[x=42,y=4], CbCr[x=44,y=4], CbCr[x=46,y=4]
							//     ...
							// - buffer5:
							//     storing the Cb and Cr information of the first block
							//     of 16 pixels of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=0,y=2),
							//     8 byte pairs of (Cb,Cr) are stored sequentially in the buffer
							//     (representing 16 horizontal pixels),
							//     then the next 3 rows are being skipped,
							//     and then followed by 8 byte pairs of the next image row (i.e. every 4th row).
							//     The rows are stored from the image top to the image bottom.
							//     CbCr[x=0,y=2], CbCr[x=2,y=2], CbCr[x=4,y=2], CbCr[x=6,y=2], CbCr[x=8,y=2], CbCr[x=10,y=2], CbCr[x=12,y=2], CbCr[x=14,y=2]
							//     CbCr[x=32,y=2], CbCr[x=34,y=2], CbCr[x=36,y=2], CbCr[x=38,y=2], CbCr[x=40,y=2], CbCr[x=42,y=2], CbCr[x=44,y=2], CbCr[x=46,y=2]
							//     ...
							//     CbCr[x=0,y=6], CbCr[x=2,y=6], CbCr[x=4,y=6], CbCr[x=6,y=6], CbCr[x=8,y=6], CbCr[x=10,y=6], CbCr[x=12,y=6], CbCr[x=14,y=6]
							//     CbCr[x=32,y=6], CbCr[x=34,y=6], CbCr[x=36,y=6], CbCr[x=38,y=6], CbCr[x=40,y=6], CbCr[x=42,y=6], CbCr[x=44,y=6], CbCr[x=46,y=6]
							//     ...
							// - buffer6:
							//     storing the Cb and Cr information of the second block
							//     of 16 pixels of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=16,y=0),
							//     8 byte pairs of (Cb,Cr) are stored sequentially in the buffer
							//     (representing 16 horizontal pixels),
							//     then the next 3 rows are being skipped,
							//     and then followed by 8 byte pairs of the next image row (i.e. every 4th row).
							//     The rows are stored from the image top to the image bottom.
							//     CbCr[x=16,y=0], CbCr[x=18,y=0], CbCr[x=20,y=0], CbCr[x=22,y=0], CbCr[x=24,y=0], CbCr[x=26,y=0], CbCr[x=28,y=0], CbCr[x=30,y=0]
							//     CbCr[x=48,y=0], CbCr[x=50,y=0], CbCr[x=52,y=0], CbCr[x=54,y=0], CbCr[x=56,y=0], CbCr[x=58,y=0], CbCr[x=60,y=0], CbCr[x=62,y=0]
							//     ...
							//     CbCr[x=16,y=4], CbCr[x=18,y=4], CbCr[x=20,y=4], CbCr[x=22,y=4], CbCr[x=24,y=4], CbCr[x=26,y=4], CbCr[x=28,y=4], CbCr[x=30,y=4]
							//     CbCr[x=48,y=4], CbCr[x=50,y=4], CbCr[x=52,y=4], CbCr[x=54,y=4], CbCr[x=56,y=4], CbCr[x=58,y=4], CbCr[x=60,y=4], CbCr[x=62,y=4]
							//     ...
							// - buffer7:
							//     storing the Cb and Cr information of the second block
							//     of 16 pixels of a 32 pixels wide vertical band.
							//     Starting at the image pixel (x=16,y=2),
							//     8 byte pairs of (Cb,Cr) are stored sequentially in the buffer
							//     (representing 16 horizontal pixels),
							//     then the next 3 rows are being skipped,
							//     and then followed by 8 byte pairs of the next image row (i.e. every 4th row).
							//     The rows are stored from the image top to the image bottom.
							//     CbCr[x=16,y=2], CbCr[x=18,y=2], CbCr[x=20,y=2], CbCr[x=22,y=2], CbCr[x=24,y=2], CbCr[x=26,y=2], CbCr[x=28,y=2], CbCr[x=30,y=2]
							//     CbCr[x=48,y=2], CbCr[x=50,y=2], CbCr[x=52,y=2], CbCr[x=54,y=2], CbCr[x=56,y=2], CbCr[x=58,y=2], CbCr[x=60,y=2], CbCr[x=62,y=2]
							//     ...
							//     CbCr[x=16,y=6], CbCr[x=18,y=6], CbCr[x=20,y=6], CbCr[x=22,y=6], CbCr[x=24,y=6], CbCr[x=26,y=6], CbCr[x=28,y=6], CbCr[x=30,y=6]
							//     CbCr[x=48,y=6], CbCr[x=50,y=6], CbCr[x=52,y=6], CbCr[x=54,y=6], CbCr[x=56,y=6], CbCr[x=58,y=6], CbCr[x=60,y=6], CbCr[x=62,y=6]
							//     ...
							int width2 = width / 2;
							int height2 = height / 2;
							int sizeY1 = ((width + 16) >> 5) * (height >> 1) * 16;
							int sizeY2 = (width >> 5) * (height >> 1) * 16;
							int sizeCrCb1 = sizeY1 >> 1;
							int sizeCrCb2 = sizeY1 >> 1;

							int[] bufferY1 = getIntBuffer(sizeY1);
							for (int x = 0, j = 0; x < width; x += 32)
							{
								for (int y = 0, i = x; y < height; y += 2, j += 16, i += 2 * width)
								{
									Array.Copy(luma, i, bufferY1, j, 16);
								}
							}
							write(buffers[buffersIndex][0] | MemoryMap.START_RAM, sizeY1, bufferY1, 0);

							int[] bufferY2 = getIntBuffer(sizeY2);
							for (int x = 16, j = 0; x < width; x += 32)
							{
								for (int y = 0, i = x; y < height; y += 2, j += 16, i += 2 * width)
								{
									Array.Copy(luma, i, bufferY2, j, 16);
								}
							}
							write(buffers[buffersIndex][1] | MemoryMap.START_RAM, sizeY2, bufferY2, 0);

							int[] bufferCrCb1 = getIntBuffer(sizeCrCb1);
							for (int x = 0, j = 0; x < width2; x += 16)
							{
								for (int y = 0; y < height2; y += 2)
								{
									for (int xx = 0, i = y * width2 + x; xx < 8; xx++, i++)
									{
										bufferCrCb1[j++] = cb[i];
										bufferCrCb1[j++] = cr[i];
									}
								}
							}
							write(buffers[buffersIndex][4] | MemoryMap.START_RAM, sizeCrCb1, bufferCrCb1, 0);

							int[] bufferCrCb2 = getIntBuffer(sizeCrCb2);
							for (int x = 0, j = 0; x < width2; x += 16)
							{
								for (int y = 1; y < height2; y += 2)
								{
									for (int xx = 0, i = y * width2 + x; xx < 8; xx++, i++)
									{
										bufferCrCb2[j++] = cb[i];
										bufferCrCb2[j++] = cr[i];
									}
								}
							}
							write(buffers[buffersIndex][5] | MemoryMap.START_RAM, sizeCrCb2, bufferCrCb2, 0);

							for (int x = 0, j = 0; x < width; x += 32)
							{
								for (int y = 1, i = x + width; y < height; y += 2, j += 16, i += 2 * width)
								{
									Array.Copy(luma, i, bufferY1, j, 16);
								}
							}
							write(buffers[buffersIndex][2] | MemoryMap.START_RAM, sizeY1, bufferY1, 0);
							releaseIntBuffer(bufferY1);

							for (int x = 16, j = 0; x < width; x += 32)
							{
								for (int y = 1, i = x + width; y < height; y += 2, j += 16, i += 2 * width)
								{
									Array.Copy(luma, i, bufferY2, j, 16);
								}
							}
							write(buffers[buffersIndex][3] | MemoryMap.START_RAM, sizeY2, bufferY2, 0);
							releaseIntBuffer(bufferY2);

							for (int x = 8, j = 0; x < width2; x += 16)
							{
								for (int y = 0; y < height2; y += 2)
								{
									for (int xx = 0, i = y * width2 + x; xx < 8; xx++, i++)
									{
										bufferCrCb1[j++] = cb[i];
										bufferCrCb1[j++] = cr[i];
									}
								}
							}
							write(buffers[buffersIndex][6] | MemoryMap.START_RAM, sizeCrCb1, bufferCrCb1, 0);
							releaseIntBuffer(bufferCrCb1);

							for (int x = 8, j = 0; x < width2; x += 16)
							{
								for (int y = 1; y < height2; y += 2)
								{
									for (int xx = 0, i = y * width2 + x; xx < 8; xx++, i++)
									{
										bufferCrCb2[j++] = cb[i];
										bufferCrCb2[j++] = cr[i];
									}
								}
							}
							write(buffers[buffersIndex][7] | MemoryMap.START_RAM, sizeCrCb2, bufferCrCb2, 0);
							releaseIntBuffer(bufferCrCb2);
						}
						releaseIntBuffer(luma);
						releaseIntBuffer(cb);
						releaseIntBuffer(cr);

						TPointer mpegAvcYuvStruct = buffer.getPointer(44);
						for (int i = 0; i < 8; i++)
						{
							mpegAvcYuvStruct.setValue32(i * 4, buffers[buffersIndex][i]);
							if (log.TraceEnabled)
							{
								log.trace(string.Format("sceVideocodecDecode YUV buffer[{0:D}]=0x{1:X8}", i, buffers[buffersIndex][i]));
							}
						}

						mpegAvcYuvStruct.setValue32(32, videoCodec.hasImage()); // 0 or 1

						mpegAvcYuvStruct.setValue32(36, bufferUnknown1);
						mem.write8(bufferUnknown1 + 0, (sbyte) 0x02); // 0x00 or 0x04
						mem.write32(bufferUnknown1 + 8, sceMpeg.mpegTimestampPerSecond);
						mem.write32(bufferUnknown1 + 16, sceMpeg.mpegTimestampPerSecond);
						mem.write32(bufferUnknown1 + 24, frameCount * 2);
						mem.write32(bufferUnknown1 + 28, 2);
						mem.write8(bufferUnknown1 + 32, (sbyte) 0x00); // 0x00 or 0x01 or 0x02
						mem.write8(bufferUnknown1 + 33, (sbyte) 0x01);

						mpegAvcYuvStruct.setValue32(40, bufferUnknown2);
						mem.write8(bufferUnknown2 + 0, (sbyte) 0x00); // 0x00 or 0x04
						mem.write32(bufferUnknown2 + 24, 0);
						mem.write32(bufferUnknown2 + 28, 0);

						TPointer buffer3 = buffer.getPointer(48);
						buffer3.setValue8(0, (sbyte) 0x01);
						buffer3.setValue8(1, unchecked((sbyte) 0xFF));
						buffer3.setValue32(4, 3);
						buffer3.setValue32(8, 4);
						buffer3.setValue32(12, 1);
						buffer3.setValue8(16, (sbyte) 0);
						buffer3.setValue32(20, 0x10000);
						buffer3.setValue32(32, 4004); // 4004 or 5005
						buffer3.setValue32(36, 240000);

						TPointer decodeSEI = buffer.getPointer(80);
						decodeSEI.setValue8(0, (sbyte) 0x02);
						decodeSEI.setValue32(8, sceMpeg.mpegTimestampPerSecond);
						decodeSEI.setValue32(16, sceMpeg.mpegTimestampPerSecond);
						decodeSEI.setValue32(24, frameCount * 2);
						decodeSEI.setValue32(28, 2);
						decodeSEI.setValue8(32, (sbyte) 0x00);
						decodeSEI.setValue8(33, (sbyte) 0x01);
					}
					break;
				case 1:
					if (videoCodec.hasImage())
					{
						if (memoryInfo == null)
						{
							int sizeY = frameBufferWidthY * frameHeight;
							int sizeCr = frameBufferWidthCr * (frameHeight / 2);
							int sizeCb = frameBufferWidthCr * (frameHeight / 2);
							int size = (sizeY + sizeCr + sizeCb) * 2;

							memoryInfo = Modules.SysMemUserForUserModule.malloc(SysMemUserForUser.KERNEL_PARTITION_ID, "sceVideocodecDecode", SysMemUserForUser.PSP_SMEM_Low, size, 0);

							bufferY1 = memoryInfo.addr & EDRAM_MEMORY_MASK;
							bufferY2 = bufferY1 + sizeY;
							bufferCr1 = bufferY1 + sizeY;
							bufferCb1 = bufferCr1 + sizeCr;
							bufferCr2 = bufferY2 + sizeY;
							bufferCb2 = bufferCr2 + sizeCr;
						}
					}

					bool buffer1 = (frameCount & 1) == 0;
					int bufferY = buffer1 ? bufferY1 : bufferY2;
					int bufferCr = buffer1 ? bufferCr1 : bufferCr2;
					int bufferCb = buffer1 ? bufferCb1 : bufferCb2;

					if (videoCodec.hasImage())
					{
						mem.memset(bufferY | MemoryMap.START_RAM, unchecked((sbyte) 0x80), frameBufferWidthY * frameHeight);
						mem.memset(bufferCr | MemoryMap.START_RAM, (sbyte)(buffer1 ? 0x50 : 0x80), frameBufferWidthCr * (frameHeight / 2));
						mem.memset(bufferCb | MemoryMap.START_RAM, unchecked((sbyte) 0x80), frameBufferWidthCb * (frameHeight / 2));
					}

					buffer2.setValue32(0, mp4Data);
					buffer2.setValue32(4, mp4Size);
					buffer2.setValue32(8, buffer.getValue32(56));
					buffer2.setValue32(12, 0x40);
					buffer2.setValue32(16, 0);
					buffer2.setValue32(44, mp4Size);
					buffer2.setValue32(48, frameWidth);
					buffer2.setValue32(52, frameHeight);
					buffer2.setValue32(60, videoCodec.hasImage() ? 2 : 1);
					buffer2.setValue32(64, 1);
					buffer2.setValue32(72, -1);
					buffer2.setValue32(76, frameCount * 0x64);
					buffer2.setValue32(80, 2997);
					buffer2.setValue32(84, bufferY);
					buffer2.setValue32(88, bufferCr);
					buffer2.setValue32(92, bufferCb);
					buffer2.setValue32(96, frameBufferWidthY);
					buffer2.setValue32(100, frameBufferWidthCr);
					buffer2.setValue32(104, frameBufferWidthCb);
					break;
				default:
					Console.WriteLine(string.Format("sceVideocodecDecode unknown type=0x{0:X}", type));
					break;
			}

			if (videoCodec.hasImage())
			{
				frameCount++;
			}

			IAction action;
			long delayMicros = threadWakeupMicroTime - Emulator.Clock.microTime();
			if (delayMicros > 0L)
			{
				//if (log.DebugEnabled)
				{
					Console.WriteLine(string.Format("Further delaying thread=0x{0:X} by {1:D} microseconds", threadUid, delayMicros));
				}
				action = new DelayThreadAction(threadUid, (int) delayMicros, false, true);
			}
			else
			{
				//if (log.DebugEnabled)
				{
					Console.WriteLine(string.Format("Unblocking thread=0x{0:X}", threadUid));
				}
				action = new UnblockThreadAction(threadUid);
			}
			// The action cannot be executed immediately as we are running
			// in a non-PSP thread. The action has to be executed by the scheduler
			// as soon as possible.
			Emulator.Scheduler.addAction(action);
		}

		public static void write(int addr, int Length, int[] buffer, int offset)
		{
			Length = System.Math.Min(Length, buffer.Length - offset);
			if (log.TraceEnabled)
			{
				log.trace(string.Format("write addr=0x{0:X8}, Length=0x{1:X}", addr, Length));
			}

			// Optimize the most common case
			if (RuntimeContext.hasMemoryInt())
			{
				int length4 = Length >> 2;
				int addrOffset = addr >> 2;
				int[] memoryInt = RuntimeContext.MemoryInt;
				for (int i = 0, j = offset; i < length4; i++)
				{
					int value = buffer[j++] & 0xFF;
					value += (buffer[j++] & 0xFF) << 8;
					value += (buffer[j++] & 0xFF) << 16;
					value += buffer[j++] << 24;
					memoryInt[addrOffset++] = value;
				}
			}
			else
			{
				IMemoryWriter memoryWriter = MemoryWriter.getMemoryWriter(addr, Length, 1);
				for (int i = 0, j = offset; i < Length; i++)
				{
					memoryWriter.writeNext(buffer[j++] & 0xFF);
				}
				memoryWriter.flush();
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0xC01EC829, version = 150) public int sceVideocodecOpen(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0xC01EC829, version : 150)]
		public virtual int sceVideocodecOpen(TPointer buffer, int type)
		{
			TPointer buffer2 = buffer.getPointer(16);

			buffer.setValue32(0, 0x05100601);

			switch (type)
			{
				case 0:
					buffer.setValue32(8, 1);
					buffer.setValue32(24, VIDEOCODEC_OPEN_TYPE0_UNKNOWN24);
					buffer.setValue32(32, VIDEOCODEC_OPEN_TYPE0_UNKNOWN4);

					buffer2.setValue32(0, VIDEOCODEC_OPEN_TYPE0_UNKNOWN0);
					buffer2.setValue32(4, VIDEOCODEC_OPEN_TYPE0_UNKNOWN4);
					break;
				case 1:
					buffer.setValue32(8, 0);
					buffer.setValue32(24, VIDEOCODEC_OPEN_TYPE1_UNKNOWN24);
					buffer.setValue32(32, VIDEOCODEC_OPEN_TYPE1_UNKNOWN32);
					break;
				default:
					Console.WriteLine(string.Format("sceVideocodecOpen unknown type {0:D}", type));
					return -1;
			}

			if (videocodecDecoderThread == null)
			{
				videocodecDecoderThread = new VideocodecDecoderThread(this);
				videocodecDecoderThread.Daemon = true;
				videocodecDecoderThread.Name = "Videocodec Decoder Thread";
				videocodecDecoderThread.Start();
			}

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA2F0564E, version = 150) public int sceVideocodecStop(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0xA2F0564E, version : 150)]
		public virtual int sceVideocodecStop(TPointer buffer, int type)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0x17099F0A, version = 150) public int sceVideocodecInit(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0x17099F0A, version : 150)]
		public virtual int sceVideocodecInit(TPointer buffer, int type)
		{
			buffer.setValue32(12, buffer.getValue32(20) + 8);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0x2D31F5B1, version = 150) public int sceVideocodecGetEDRAM(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0x2D31F5B1, version : 150)]
		public virtual int sceVideocodecGetEDRAM(TPointer buffer, int type)
		{
			int size = (buffer.getValue32(24) + 63) | 0x3F;
			edramInfo = Modules.SysMemUserForUserModule.malloc(SysMemUserForUser.KERNEL_PARTITION_ID, "sceVideocodecEDRAM", SysMemUserForUser.PSP_SMEM_Low, size, 0);
			if (edramInfo == null)
			{
				return -1;
			}

			int addrEDRAM = edramInfo.addr & EDRAM_MEMORY_MASK;
			buffer.setValue32(20, alignUp(addrEDRAM, 63));
			buffer.setValue32(92, addrEDRAM);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0x4F160BF4, version = 150) public int sceVideocodecReleaseEDRAM(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer)
		[HLEFunction(nid : 0x4F160BF4, version : 150)]
		public virtual int sceVideocodecReleaseEDRAM(TPointer buffer)
		{
			buffer.setValue32(20, 0);
			buffer.setValue32(92, 0);

			if (edramInfo != null)
			{
				Modules.SysMemUserForUserModule.free(edramInfo);
				edramInfo = null;
			}

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0xDBA273FA, version = 150) public int sceVideocodecDecode(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0xDBA273FA, version : 150)]
		public virtual int sceVideocodecDecode(TPointer buffer, int type)
		{
			if (type != 0 && type != 1)
			{
				Console.WriteLine(string.Format("sceVideocodecDecode unknown type=0x{0:X}", type));
				return -1;
			}

			int threadUid = Modules.ThreadManForUserModule.CurrentThreadID;
			Modules.ThreadManForUserModule.hleBlockCurrentThread(SceKernelThreadInfo.JPCSP_WAIT_VIDEO_DECODER);
			videocodecDecoderThread.trigger(buffer, type, threadUid, Emulator.Clock.microTime() + videocodecDecodeDelay);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x17CF7D2C, version = 150) public int sceVideocodecGetFrameCrop()
		[HLEFunction(nid : 0x17CF7D2C, version : 150)]
		public virtual int sceVideocodecGetFrameCrop()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x26927D19, version = 150) public int sceVideocodecGetVersion(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0x26927D19, version : 150)]
		public virtual int sceVideocodecGetVersion(TPointer buffer, int type)
		{
			// This is the value returned on my PSP according to JpcspTrace.
			buffer.setValue32(4, 0x78);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2F385E7F, version = 150) public int sceVideocodecScanHeader()
		[HLEFunction(nid : 0x2F385E7F, version : 150)]
		public virtual int sceVideocodecScanHeader()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x307E6E1C, version = 150) public int sceVideocodecDelete()
		[HLEFunction(nid : 0x307E6E1C, version : 150)]
		public virtual int sceVideocodecDelete()
		{
			if (videocodecDecoderThread != null)
			{
				videocodecDecoderThread.exit();
				videocodecDecoderThread = null;
			}

			if (videoCodec != null)
			{
				videoCodec = null;
			}

			if (memoryInfo != null)
			{
				Modules.SysMemUserForUserModule.free(memoryInfo);
				memoryInfo = null;
			}

			if (edramInfo != null)
			{
				Modules.SysMemUserForUserModule.free(edramInfo);
				edramInfo = null;
			}

			Modules.ThreadManForUserModule.hleKernelDelayThread(videocodecDeleteDelay, false);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x627B7D42, version = 150) public int sceVideocodecGetSEI(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0x627B7D42, version : 150)]
		public virtual int sceVideocodecGetSEI(TPointer buffer, int type)
		{
			TPointer decodeSEI = buffer.getPointer(80);
			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceVideocodecGetSEI storing decodeSEI to {0}", decodeSEI));
			}
			decodeSEI.setValue32(28, 0);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x745A7B7A, version = 150) public int sceVideocodecSetMemory(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=96, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer buffer, int type)
		[HLEFunction(nid : 0x745A7B7A, version : 150)]
		public virtual int sceVideocodecSetMemory(TPointer buffer, int type)
		{
			int unknown1 = buffer.getValue32(64);
			int unknown2 = buffer.getValue32(68);
			int unknown3 = buffer.getValue32(72);
			int unknown4 = buffer.getValue32(76);

			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceVideocodecSetMemory unknown1=0x{0:X8}, unknown2=0x{1:X8}, unknown3=0x{2:X8}, unknown4=0x{3:X8}", unknown1, unknown2, unknown3, unknown4));
			}

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x893B32B1, version = 150) public int sceVideocodec_893B32B1()
		[HLEFunction(nid : 0x893B32B1, version : 150)]
		public virtual int sceVideocodec_893B32B1()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD95C24D5, version = 150) public int sceVideocodec_D95C24D5()
		[HLEFunction(nid : 0xD95C24D5, version : 150)]
		public virtual int sceVideocodec_D95C24D5()
		{
			return 0;
		}
	}

}