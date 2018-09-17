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
namespace pspsharp.memory.mmio.memorystick
{
	using pspAbstractMemoryMappedStructure = pspsharp.HLE.kernel.types.pspAbstractMemoryMappedStructure;

	/// <summary>
	/// The Memory Stick Pro sysinfo attribute entry structure.
	/// Based on information from
	/// https://github.com/torvalds/linux/blob/master/drivers/memstick/core/mspro_block.c
	/// see "struct mspro_sys_info".
	/// </summary>
	public class MemoryStickSysInfo : pspAbstractMemoryMappedStructure
	{
		public const int MEMORY_STICK_CLASS_PRO = 2;
		public int memoryStickClass;
		public int reserved0;
		public int blockSize;
		public int blockCount;
		public int userBlockCount;
		public int pageSize;
		public readonly sbyte[] reserved1 = new sbyte[2];
		public readonly sbyte[] assemblyDate = new sbyte[8];
		public int serialNumber;
		public int assemblyMakerCode;
		public readonly sbyte[] assemblyModelCode = new sbyte[3];
		public int memoryMakerCode;
		public int memoryModelCode;
		public readonly sbyte[] reserved2 = new sbyte[4];
		public int vcc;
		public int vpp;
		public int controllerNumber;
		public int controllerFunction;
		public int startSector;
		public int unitSize;
		public int memoryStickSubClass;
		public readonly sbyte[] reserved3 = new sbyte[4];
		public int interfaceType;
		public int controllerCode;
		public int formatType;
		public int reserved4;
		public int deviceType;
		public readonly sbyte[] reserved5 = new sbyte[7];
		public readonly sbyte[] memoryStickProId = new sbyte[16];
		public readonly sbyte[] reserved6 = new sbyte[16];

		protected internal override bool BigEndian
		{
			get
			{
				return true;
			}
		}

		protected internal override void read()
		{
			memoryStickClass = read8(); // Offset 0
			reserved0 = read8(); // Offset 1
			blockSize = read16(); // Offset 2
			blockCount = read16(); // Offset 4
			userBlockCount = read16(); // Offset 6
			pageSize = read16(); // Offset 8
			read8Array(reserved1); // Offset 10
			read8Array(assemblyDate); // Offset 12
			serialNumber = read32(); // Offset 20
			assemblyMakerCode = read8(); // Offset 24
			read8Array(assemblyModelCode); // Offset 25
			memoryMakerCode = read16(); // Offset 28
			memoryModelCode = read16(); // Offset 30
			read8Array(reserved2); // Offset 32
			vcc = read8(); // Offset 36
			vpp = read8(); // Offset 37
			controllerNumber = read16(); // Offset 38
			controllerFunction = read16(); // Offset 40
			startSector = read16(); // Offset 42
			unitSize = read16(); // Offset 44
			memoryStickSubClass = read8(); // Offset 46
			read8Array(reserved3); // Offset 47
			interfaceType = read8(); // Offset 51
			controllerCode = read16(); // Offset 52
			formatType = read8(); // Offset 54
			reserved4 = read8(); // Offset 55
			deviceType = read8(); // Offset 56
			read8Array(reserved5); // Offset 57
			read8Array(memoryStickProId); // Offset 64
			read8Array(reserved6); // Offset 80
		}

		protected internal override void write()
		{
			write8((sbyte) memoryStickClass);
			write8((sbyte) reserved0);
			write16((short) blockSize);
			write16((short) blockCount);
			write16((short) userBlockCount);
			write16((short) pageSize);
			write8Array(reserved1);
			write8Array(assemblyDate);
			write32(serialNumber);
			write8((sbyte) assemblyMakerCode);
			write8Array(assemblyModelCode);
			write16((short) memoryMakerCode);
			write16((short) memoryModelCode);
			write8Array(reserved2);
			write8((sbyte) vcc);
			write8((sbyte) vpp);
			write16((short) controllerNumber);
			write16((short) controllerFunction);
			write16((short) startSector);
			write16((short) unitSize);
			write8((sbyte) memoryStickSubClass);
			write8Array(reserved3);
			write8((sbyte) interfaceType);
			write16((short) controllerCode);
			write8((sbyte) formatType);
			write8((sbyte) reserved4);
			write8((sbyte) deviceType);
			write8Array(reserved5);
			write8Array(memoryStickProId);
			write8Array(reserved6);
		}

		public override int @sizeof()
		{
			return 96;
		}
	}

}