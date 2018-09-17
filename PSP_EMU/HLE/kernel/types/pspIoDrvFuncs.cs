﻿using System.Text;

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
namespace pspsharp.HLE.kernel.types
{
	public class pspIoDrvFuncs : pspAbstractMemoryMappedStructure
	{
		public int ioInit;
		public int ioExit;
		public int ioOpen;
		public int ioClose;
		public int ioRead;
		public int ioWrite;
		public int ioLseek;
		public int ioIoctl;
		public int ioRemove;
		public int ioMkdir;
		public int ioRmdir;
		public int ioDopen;
		public int ioDclose;
		public int ioDread;
		public int ioGetstat;
		public int ioChstat;
		public int ioRename;
		public int ioChdir;
		public int ioMount;
		public int ioUmount;
		public int ioDevctl;
		public int ioUnk21;

		protected internal override void read()
		{
			ioInit = read32();
			ioExit = read32();
			ioOpen = read32();
			ioClose = read32();
			ioRead = read32();
			ioWrite = read32();
			ioLseek = read32();
			ioIoctl = read32();
			ioRemove = read32();
			ioMkdir = read32();
			ioRmdir = read32();
			ioDopen = read32();
			ioDclose = read32();
			ioDread = read32();
			ioGetstat = read32();
			ioChstat = read32();
			ioRename = read32();
			ioChdir = read32();
			ioMount = read32();
			ioUmount = read32();
			ioDevctl = read32();
			ioUnk21 = read32();
		}

		protected internal override void write()
		{
			write32(ioInit);
			write32(ioExit);
			write32(ioOpen);
			write32(ioClose);
			write32(ioRead);
			write32(ioWrite);
			write32(ioLseek);
			write32(ioIoctl);
			write32(ioRemove);
			write32(ioMkdir);
			write32(ioRmdir);
			write32(ioDopen);
			write32(ioDclose);
			write32(ioDread);
			write32(ioGetstat);
			write32(ioChstat);
			write32(ioRename);
			write32(ioChdir);
			write32(ioMount);
			write32(ioUmount);
			write32(ioDevctl);
			write32(ioUnk21);
		}

		public override int @sizeof()
		{
			return 88;
		}

		private static void ToString(StringBuilder s, string name, int addr)
		{
			if (addr != 0)
			{
				if (s.Length > 0)
				{
					s.Append(", ");
				}
				s.Append(string.Format("{0}=0x{1:X8}", name, addr));
			}
		}

		public override string ToString()
		{
			StringBuilder s = new StringBuilder();

			ToString(s, "ioInit", ioInit);
			ToString(s, "ioExit", ioExit);
			ToString(s, "ioOpen", ioOpen);
			ToString(s, "ioClose", ioClose);
			ToString(s, "ioRead", ioRead);
			ToString(s, "ioWrite", ioWrite);
			ToString(s, "ioLseek", ioLseek);
			ToString(s, "ioIoctl", ioIoctl);
			ToString(s, "ioRemove", ioRemove);
			ToString(s, "ioMkdir", ioMkdir);
			ToString(s, "ioRmdir", ioRmdir);
			ToString(s, "ioDopen", ioDopen);
			ToString(s, "ioDclose", ioDclose);
			ToString(s, "ioDread", ioDread);
			ToString(s, "ioGetstat", ioGetstat);
			ToString(s, "ioChstat", ioChstat);
			ToString(s, "ioRename", ioRename);
			ToString(s, "ioChdir", ioChdir);
			ToString(s, "ioMount", ioMount);
			ToString(s, "ioUmount", ioUmount);
			ToString(s, "ioDevctl", ioDevctl);
			ToString(s, "ioUnk21", ioUnk21);

			return s.ToString();
		}
	}

}