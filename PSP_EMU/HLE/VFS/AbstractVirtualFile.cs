﻿using System.Collections.Generic;

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
namespace pspsharp.HLE.VFS
{

	using Level = org.apache.log4j.Level;
	//using Logger = org.apache.log4j.Logger;

	using SceKernelErrors = pspsharp.HLE.kernel.types.SceKernelErrors;
	using IoFileMgrForUser = pspsharp.HLE.modules.IoFileMgrForUser;
	using IoOperation = pspsharp.HLE.modules.IoFileMgrForUser.IoOperation;
	using IoOperationTiming = pspsharp.HLE.modules.IoFileMgrForUser.IoOperationTiming;
	using SeekableDataInput = pspsharp.filesystems.SeekableDataInput;
	using Utilities = pspsharp.util.Utilities;

	public abstract class AbstractVirtualFile : IVirtualFile
	{
		protected internal static Logger log = AbstractVirtualFileSystem.log;
		protected internal readonly SeekableDataInput file;
		protected internal const int IO_ERROR = AbstractVirtualFileSystem.IO_ERROR;
		private readonly IVirtualFile ioctlFile;

		public AbstractVirtualFile(SeekableDataInput file)
		{
			this.file = file;
			ioctlFile = null;
		}

		public AbstractVirtualFile(SeekableDataInput file, IVirtualFile ioctlFile)
		{
			this.file = file;
			this.ioctlFile = ioctlFile;
		}

		public virtual long Position
		{
			get
			{
				try
				{
					return file.FilePointer;
				}
				catch (IOException e)
				{
					System.Console.WriteLine("getPosition", e);
				}
				return Modules.IoFileMgrForUserModule.getPosition(this);
			}
			set
			{
				Modules.IoFileMgrForUserModule.setPosition(this, value);
				ioLseek(value);
			}
		}


		public virtual int ioClose()
		{
			try
			{
				file.Dispose();
			}
			catch (IOException e)
			{
				System.Console.WriteLine("ioClose", e);
				return IO_ERROR;
			}

			return 0;
		}

		private int getReadLength(int outputLength)
		{
			int readLength = outputLength;
			long restLength = Length() - Position;
			if (restLength < readLength)
			{
				readLength = (int) restLength;
			}

			return readLength;
		}

		public virtual int ioRead(TPointer outputPointer, int outputLength)
		{
			int readLength = getReadLength(outputLength);
			try
			{
				Utilities.readFully(file, outputPointer.Address, readLength);
			}
			catch (IOException e)
			{
				System.Console.WriteLine("ioRead", e);
				return SceKernelErrors.ERROR_KERNEL_FILE_READ_ERROR;
			}

			return readLength;
		}

		public virtual int ioRead(sbyte[] outputBuffer, int outputOffset, int outputLength)
		{
			int readLength = getReadLength(outputLength);
			if (readLength > 0)
			{
				try
				{
					file.readFully(outputBuffer, outputOffset, readLength);
				}
				catch (IOException e)
				{
					System.Console.WriteLine("ioRead", e);
					return SceKernelErrors.ERROR_KERNEL_FILE_READ_ERROR;
				}
			}
			else if (outputLength > 0)
			{
				// End of file
				return SceKernelErrors.ERROR_KERNEL_FILE_READ_ERROR;
			}

			return readLength;
		}

		public virtual int ioWrite(TPointer inputPointer, int inputLength)
		{
			return IO_ERROR;
		}

		public virtual int ioWrite(sbyte[] inputBuffer, int inputOffset, int inputLength)
		{
			return IO_ERROR;
		}

		public virtual long ioLseek(long offset)
		{
			try
			{
				file.seek(offset);
			}
			catch (IOException e)
			{
				System.Console.WriteLine("ioLseek", e);
				return IO_ERROR;
			}
			return offset;
		}

		public virtual int ioIoctl(int command, TPointer inputPointer, int inputLength, TPointer outputPointer, int outputLength)
		{
			if (ioctlFile != null)
			{
				return ioctlFile.ioIoctl(command, inputPointer, inputLength, outputPointer, outputLength);
			}

			if (log.isEnabledFor(Level.WARN))
			{
				System.Console.WriteLine(string.Format("ioIoctl 0x{0:X8} unsupported command, inlen={1:D}, outlen={2:D}", command, inputLength, outputLength));
				if (inputPointer.AddressGood)
				{
					System.Console.WriteLine(string.Format("ioIoctl indata: {0}", Utilities.getMemoryDump(inputPointer.Address, inputLength)));
				}
				if (outputPointer.AddressGood)
				{
					System.Console.WriteLine(string.Format("ioIoctl outdata: {0}", Utilities.getMemoryDump(outputPointer.Address, outputLength)));
				}
			}

			return IO_ERROR;
		}

		public virtual long Length()
		{
			try
			{
				return file.Length();
			}
			catch (IOException e)
			{
				//if (log.DebugEnabled)
				{
					System.Console.WriteLine("Length", e);
				}
			}

			return 0;
		}

		public virtual bool SectorBlockMode
		{
			get
			{
				return false;
			}
		}

		public virtual IVirtualFile duplicate()
		{
			return null;
		}

		public virtual IDictionary<IoFileMgrForUser.IoOperation, IoFileMgrForUser.IoOperationTiming> Timings
		{
			get
			{
				return IoFileMgrForUser.defaultTimings;
			}
		}

		public override string ToString()
		{
			return file == null ? null : file.ToString();
		}
	}

}