﻿/*
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
namespace pspsharp.media
{

	using sceAtrac3plus = pspsharp.HLE.modules.sceAtrac3plus;

	public class OMAFormat
	{
		private const int OMA_EA3_MAGIC = 0x45413301;
		private const sbyte OMA_CODECID_ATRAC3P = 1;

		private static ByteBuffer getOmaHeader(sbyte codecId, sbyte headerCode1, sbyte headerCode2)
		{
			ByteBuffer header = ByteBuffer.allocate(96).order(ByteOrder.BIG_ENDIAN);
			header.putInt(OMA_EA3_MAGIC);
			header.putShort((short) header.capacity());
			header.putShort((short) -1);

			// Unknown 24 bytes...
			header.putInt(0x00000000);
			header.putInt(0x010f5000);
			header.putInt(0x00040000);
			header.putInt(0x0000f5ce);
			header.putInt(0xd2929132);
			header.putInt(0x2480451c);

			header.put(codecId);
			header.put((sbyte) 0);
			header.put(headerCode1);
			header.put(headerCode2);
			while (header.position() < header.limit())
			{
				header.put((sbyte) 0);
			}
			header.rewind();

			return header;
		}

		private static bool isHeader(ByteBuffer audioStream, int offset)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override byte header1 = (byte) 0x0F;
			sbyte header1 = (sbyte) 0x0F;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override byte header2 = (byte) 0xD0;
			sbyte header2 = unchecked((sbyte) 0xD0);

			return audioStream.get(offset) == header1 && audioStream.get(offset + 1) == header2;
		}

		private static int getNextHeaderPosition(ByteBuffer audioStream, int frameSize)
		{
			int endScan = audioStream.limit() - 1;

			// Most common case: the header can be found at each frameSize
			int offset = audioStream.position() + frameSize - 8;
			if (offset < endScan && isHeader(audioStream, offset))
			{
				return offset;
			}

			for (int scan = audioStream.position(); scan < endScan; scan++)
			{
				if (isHeader(audioStream, scan))
				{
					return scan;
				}
			}

			return -1;
		}

		public static ByteBuffer convertStreamToOMA(ByteBuffer audioStream)
		{
			if (!isHeader(audioStream, 0))
			{
				return null;
			}

			sbyte headerCode1 = audioStream.get(2);
			sbyte headerCode2 = audioStream.get(3);
			ByteBuffer header = getOmaHeader(OMA_CODECID_ATRAC3P, headerCode1, headerCode2);

			int frameSize = ((headerCode1 & 0x03) << 8) | (headerCode2 & 0xFF) * 8 + 0x10;
			int numCompleteFrames = audioStream.remaining() / (frameSize + 8);
			int lastFrameSize = audioStream.remaining() - (numCompleteFrames * (frameSize + 8));

			int omaStreamSize = header.remaining() + numCompleteFrames * frameSize + lastFrameSize;
			// Allocate an OMA stream size large enough (better too large than too short)
			omaStreamSize = System.Math.Max(omaStreamSize, audioStream.remaining());
			ByteBuffer oma = ByteBuffer.allocate(omaStreamSize).order(ByteOrder.LITTLE_ENDIAN);

			oma.put(header);
			while (audioStream.remaining() > 8)
			{
				// Skip 8 bytes frame header
				audioStream.position(audioStream.position() + 8);
				int nextHeader = getNextHeaderPosition(audioStream, frameSize);
				ByteBuffer frame = audioStream.slice();
				if (nextHeader >= 0)
				{
					frame.limit(nextHeader - audioStream.position());
					audioStream.position(nextHeader);
				}
				else
				{
					audioStream.position(audioStream.limit());
				}
				oma.put(frame);
			}
			oma.limit(oma.position());
			oma.rewind();

			return oma;
		}

		private static int getChunkOffset(ByteBuffer riff, int chunkMagic, int offset)
		{
			for (int i = offset; i <= riff.limit() - 4;)
			{
				if (riff.getInt(i) == chunkMagic)
				{
					return i;
				}
				// Move to next chunk
				int chunkSize = riff.getInt(i + 4);
				i += chunkSize + 8;
			}

			return -1;
		}

		public static ByteBuffer convertRIFFtoOMA(ByteBuffer riff)
		{
			const int firstChunkOffset = 12;
			riff.order(ByteOrder.LITTLE_ENDIAN);

			if (riff.getInt(0) != sceAtrac3plus.RIFF_MAGIC)
			{
				// Not a RIFF data
				return null;
			}

			int fmtChunkOffset = getChunkOffset(riff, sceAtrac3plus.FMT_CHUNK_MAGIC, firstChunkOffset);
			if (fmtChunkOffset < 0)
			{
				return null;
			}
			sbyte codecId = riff.get(fmtChunkOffset + 0x30);
			sbyte headerCode1 = riff.get(fmtChunkOffset + 0x32);
			sbyte headerCode2 = riff.get(fmtChunkOffset + 0x33);
			ByteBuffer header = getOmaHeader(codecId, headerCode1, headerCode2);

			int dataChunkOffset = getChunkOffset(riff, sceAtrac3plus.DATA_CHUNK_MAGIC, firstChunkOffset);
			if (dataChunkOffset < 0)
			{
				return null;
			}
			int dataSize = riff.getInt(dataChunkOffset + 4);
			ByteBuffer dataBuffer = riff.slice();
			dataBuffer.position(dataChunkOffset + 8);
			dataBuffer.limit(dataBuffer.position() + dataSize);

			ByteBuffer oma = ByteBuffer.allocate(header.remaining() + dataBuffer.remaining()).order(ByteOrder.LITTLE_ENDIAN);
			oma.put(header);
			oma.put(dataBuffer);

			oma.rewind();

			return oma;
		}

		public static int getOMANumberAudioChannels(ByteBuffer oma)
		{
			int headerParameters = oma.getInt(0x20);
			int channels = (headerParameters >> 18) & 0x7;

			return channels;
		}
	}

}