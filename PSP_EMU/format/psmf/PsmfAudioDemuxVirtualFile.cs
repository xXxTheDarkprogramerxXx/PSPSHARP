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
namespace pspsharp.format.psmf
{
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static pspsharp.HLE.VFS.AbstractVirtualFileSystem.IO_ERROR;

    //using Logger = org.apache.log4j.Logger;

    using TPointer = pspsharp.HLE.TPointer;
    using AbstractProxyVirtualFile = pspsharp.HLE.VFS.AbstractProxyVirtualFile;
    using IVirtualFile = pspsharp.HLE.VFS.IVirtualFile;
    using System;

    /// <summary>
    /// Provides a IVirtualFile interface to read only the audio from a PSMF file.
    /// 
    /// @author gid15
    /// 
    /// </summary>
    public class PsmfAudioDemuxVirtualFile : AbstractProxyVirtualFile
	{
		//private static new Logger log = Emulator.log;
		public const int PACKET_START_CODE_MASK = unchecked((int)0xffffff00);
		public const int PACKET_START_CODE_PREFIX = 0x00000100;

		public const int SEQUENCE_START_CODE = 0x000001b3;
		public const int EXT_START_CODE = 0x000001b5;
		public const int SEQUENCE_END_CODE = 0x000001b7;
		public const int GOP_START_CODE = 0x000001b8;
		public const int ISO_11172_END_CODE = 0x000001b9;
		public const int PACK_START_CODE = 0x000001ba;
		public const int SYSTEM_HEADER_START_CODE = 0x000001bb;
		public const int PROGRAM_STREAM_MAP = 0x000001bc;
		public const int PRIVATE_STREAM_1 = 0x000001bd;
		public const int PADDING_STREAM = 0x000001be;
		public const int PRIVATE_STREAM_2 = 0x000001bf;

		private sbyte[] buffer = new sbyte[1];
		private int audioChannel;
		private long position;
		private int mpegOffset;
		private long startPosition;
		private int remainingPacketLength;

		public PsmfAudioDemuxVirtualFile(IVirtualFile vFile, int mpegOffset, int audioChannel) : base(vFile)
		{
			this.mpegOffset = mpegOffset;
			this.audioChannel = audioChannel;
			startPosition = vFile.Position;

			if (mpegOffset > 0)
			{
				vFile.ioLseek(startPosition + mpegOffset);
			}
			else
			{
				this.mpegOffset = 0;
			}
		}

		private int read8()
		{
			if (vFile.ioRead(buffer, 0, 1) != 1)
			{
				return -1;
			}

			return buffer[0] & 0xFF;
		}

		private int read16()
		{
			return (read8() << 8) | read8();
		}

		private long readPts()
		{
			return readPts(read8());
		}

		private long readPts(int c)
		{
			return (((long)(c & 0x0E)) << 29) | ((read16() >> 1) << 15) | (read16() >> 1);
		}

		private void skip(int n)
		{
			if (n > 0)
			{
				vFile.ioLseek(vFile.Position + n);
			}
		}

		private int readPesHeader(PesHeader pesHeader, int Length, int startCode)
		{
			int c = 0;
			while (Length > 0)
			{
				c = read8();
				Length--;
				if (c != 0xFF)
				{
					break;
				}
			}

			if ((c & 0xC0) == 0x40)
			{
				read8();
				c = read8();
				Length -= 2;
			}
			pesHeader.DtsPts = 0;
			if ((c & 0xE0) == 0x20)
			{
				pesHeader.DtsPts = readPts(c);
				Length -= 4;
				if ((c & 0x10) != 0)
				{
					pesHeader.Dts = readPts();
					Length -= 5;
				}
			}
			else if ((c & 0xC0) == 0x80)
			{
				int flags = read8();
				int headerLength = read8();
				Length -= 2;
				Length -= headerLength;
				if ((flags & 0x80) != 0)
				{
					pesHeader.DtsPts = readPts();
					headerLength -= 5;
					if ((flags & 0x40) != 0)
					{
						pesHeader.Dts = readPts();
						headerLength -= 5;
					}
				}
				if ((flags & 0x3F) != 0 && headerLength == 0)
				{
					flags &= 0xC0;
				}
				if ((flags & 0x01) != 0)
				{
					int pesExt = read8();
					headerLength--;
					int skip = (pesExt >> 4) & 0x0B;
					skip += skip & 0x09;
					if ((pesExt & 0x40) != 0 || skip > headerLength)
					{
						pesExt = skip = 0;
					}
					this.skip(skip);
					headerLength -= skip;
					if ((pesExt & 0x01) != 0)
					{
						int ext2Length = read8();
						headerLength--;
						 if ((ext2Length & 0x7F) != 0)
						 {
							 int idExt = read8();
							 headerLength--;
							 if ((idExt & 0x80) == 0)
							 {
								 startCode = ((startCode & 0xFF) << 8) | idExt;
							 }
						 }
					}
				}
				skip(headerLength);
			}
			if (startCode == PRIVATE_STREAM_1)
			{
				int channel = read8();
				pesHeader.Channel = channel;
				Length--;
				if (channel >= 0x80 && channel <= 0xCF)
				{
					// Skip audio header
					skip(3);
					Length -= 3;
					if (channel >= 0xB0 && channel <= 0xBF)
					{
						skip(1);
						Length--;
					}
				}
				else
				{
					// PSP audio has additional 3 bytes in header
					skip(3);
					Length -= 3;
				}
			}

			return Length;
		}

		private bool EOF
		{
			get
			{
				return vFile.Position >= vFile.Length();
			}
		}

		private int doRead(TPointer outputPointer, sbyte[] outputBuffer, int outputOffset, int outputLength)
		{
			if (EOF)
			{
				return -1;
			}

			int readLength = 0;
			int readAddr = outputPointer != null ? outputPointer.Address : 0;

			while (remainingPacketLength > 0 && readLength < outputLength)
			{
				int maxReadLength = System.Math.Min(remainingPacketLength, outputLength - readLength);
				int l;
				if (outputBuffer != null)
				{
					l = vFile.ioRead(outputBuffer, outputOffset, maxReadLength);
				}
				else if (outputPointer != null)
				{
					l = vFile.ioRead(new TPointer(outputPointer.Memory, readAddr), maxReadLength);
				}
				else
				{
					l = maxReadLength;
				}

				if (l > 0)
				{
					remainingPacketLength -= l;
					readLength += l;
					readAddr += l;
					outputOffset += l;
					position += l;
				}
				else if (l < 0)
				{
					break;
				}
			}

			if (remainingPacketLength > 0)
			{
				return readLength;
			}

			while (!EOF && readLength < outputLength)
			{
				long startIndex = vFile.Position;
				int startCode = 0xFF;
				while ((startCode & PACKET_START_CODE_MASK) != PACKET_START_CODE_PREFIX && !EOF)
				{
					startCode = (startCode << 8) | read8();
				}
				//if (log.DebugEnabled)
				{
					Console.WriteLine(string.Format("StartCode 0x{0:X8}, offset {1:X8}, skipped {2:D}", startCode, vFile.Position, vFile.Position - startIndex - 4));
				}

				switch (startCode)
				{
					case PACK_START_CODE:
					{
						skip(10);
						break;
					}
					case SYSTEM_HEADER_START_CODE:
					{
						skip(14);
						break;
					}
					case PADDING_STREAM:
					case PRIVATE_STREAM_2:
					{
						int Length = read16();
						skip(Length);
						break;
					}
					case PRIVATE_STREAM_1:
					{
						// Audio stream
						int Length = read16();
						PesHeader pesHeader = new PesHeader(audioChannel);
						Length = readPesHeader(pesHeader, Length, startCode);
						if (pesHeader.Channel == audioChannel || audioChannel < 0)
						{
							int packetLength = 0;
							while (packetLength < Length && readLength < outputLength)
							{
								int maxReadLength = System.Math.Min(Length - packetLength, outputLength - readLength);
								int l;
								if (outputBuffer != null)
								{
									l = vFile.ioRead(outputBuffer, outputOffset, maxReadLength);
								}
								else if (outputPointer != null)
								{
									l = vFile.ioRead(new TPointer(outputPointer.Memory, readAddr), maxReadLength);
								}
								else
								{
									l = maxReadLength;
								}

								if (l > 0)
								{
									readLength += l;
									readAddr += l;
									outputOffset += l;
									packetLength += l;
									position += l;
								}
								else if (l < 0)
								{
									break;
								}
							}
							remainingPacketLength = Length - packetLength;
						}
						else
						{
							skip(Length);
						}
						break;
					}
					case 0x1E0:
				case 0x1E1:
			case 0x1E2:
		case 0x1E3:
					case 0x1E4:
				case 0x1E5:
			case 0x1E6:
		case 0x1E7:
					case 0x1E8:
				case 0x1E9:
			case 0x1EA:
		case 0x1EB:
					case 0x1EC:
				case 0x1ED:
			case 0x1EE:
		case 0x1EF:
		{
						// Video Stream, skipped
						int Length = read16();
						skip(Length);
						break;
		}
					default:
					{
						Console.WriteLine(string.Format("Unknown StartCode 0x{0:X8}, offset {1:X8}", startCode, vFile.Position));
					}
				break;
				}
			}

			return readLength;
		}

		public override int ioRead(TPointer outputPointer, int outputLength)
		{
			return doRead(outputPointer, null, 0, outputLength);
		}

		public override int ioRead(sbyte[] outputBuffer, int outputOffset, int outputLength)
		{
			return doRead(null, outputBuffer, outputOffset, outputLength);
		}

		public override long ioLseek(long offset)
		{
			long result = vFile.ioLseek(startPosition + mpegOffset);
			if (result < 0)
			{
				return result;
			}

			position = 0;
			while (Position < offset)
			{
				int Length = doRead(null, null, 0, offset < int.MaxValue ? (int) offset : int.MaxValue);
				if (Length < 0)
				{
					return -1;
				}
			}

			return Position;
		}

		public override long Length()
		{
			return base.Length() - startPosition - mpegOffset;
		}

		public override long Position
		{
			get
			{
				return position;
			}
		}

		public override IVirtualFile duplicate()
		{
			return base.duplicate();
		}
	}

}