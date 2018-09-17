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
namespace pspsharp.HLE.modules
{

	//using Logger = org.apache.log4j.Logger;

	using CpuState = pspsharp.Allegrex.CpuState;
	using SysMemInfo = pspsharp.HLE.modules.SysMemUserForUser.SysMemInfo;

	public class scePaf : HLEModule
	{
		//public static Logger log = Modules.getLogger("scePaf");
		private Dictionary<int, SysMemInfo> allocated = new Dictionary<int, SysMemInfo>();

		[HLEFunction(nid : 0xA138A376, version : 660)]
		public virtual int scePaf_sprintf_660(CpuState cpu, TPointer buffer, string format)
		{
			return Modules.SysclibForKernelModule.sprintf(cpu, buffer, format);
		}

		[HLEFunction(nid : 0x0FCDFA1E, version : 150)]
		public virtual int scePaf_0FCDFA1E(int size)
		{
			SysMemInfo sysMemInfo = Modules.SysMemUserForUserModule.malloc(SysMemUserForUser.USER_PARTITION_ID, "scePaf_0FCDFA1E", SysMemUserForUser.PSP_SMEM_Low, size, 0);
			if (sysMemInfo == null)
			{
				return 0;
			}

			int addr = sysMemInfo.addr;
			allocated[addr] = sysMemInfo;

			return addr;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0xB4652CFE, version = 150) public int scePaf_B4652CFE(@CanBeNull pspsharp.HLE.TPointer destAddr, pspsharp.HLE.TPointer srcAddr, int size)
		[HLEFunction(nid : 0xB4652CFE, version : 150)]
		public virtual int scePaf_B4652CFE(TPointer destAddr, TPointer srcAddr, int size)
		{
			return Modules.SysclibForKernelModule.memcpy(destAddr, srcAddr, size);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x003B3F87, version = 150) public int scePaf_003B3F87()
		[HLEFunction(nid : 0x003B3F87, version : 150)]
		public virtual int scePaf_003B3F87()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x0C2CD696, version = 150) public int scePaf_0C2CD696()
		[HLEFunction(nid : 0x0C2CD696, version : 150)]
		public virtual int scePaf_0C2CD696()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x1F02DD65, version = 150) public int scePaf_1F02DD65()
		[HLEFunction(nid : 0x1F02DD65, version : 150)]
		public virtual int scePaf_1F02DD65()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x20BEF384, version = 150) public int scePaf_20BEF384()
		[HLEFunction(nid : 0x20BEF384, version : 150)]
		public virtual int scePaf_20BEF384()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x22420CC7, version = 150) public int scePaf_22420CC7()
		[HLEFunction(nid : 0x22420CC7, version : 150)]
		public virtual int scePaf_22420CC7()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3C4BC2CD, version = 150) public int scePaf_3C4BC2CD()
		[HLEFunction(nid : 0x3C4BC2CD, version : 150)]
		public virtual int scePaf_3C4BC2CD()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3E564415, version = 150) public int scePaf_3E564415()
		[HLEFunction(nid : 0x3E564415, version : 150)]
		public virtual int scePaf_3E564415()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x43759F51, version = 150) public int scePaf_43759F51()
		[HLEFunction(nid : 0x43759F51, version : 150)]
		public virtual int scePaf_43759F51()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x4E43A742, version = 150) public int scePaf_4E43A742()
		[HLEFunction(nid : 0x4E43A742, version : 150)]
		public virtual int scePaf_4E43A742()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x5A12583F, version = 150) public int scePaf_5A12583F()
		[HLEFunction(nid : 0x5A12583F, version : 150)]
		public virtual int scePaf_5A12583F()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x65169E51, version = 150) public int scePaf_65169E51()
		[HLEFunction(nid : 0x65169E51, version : 150)]
		public virtual int scePaf_65169E51()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x66CCA794, version = 150) public int scePaf_66CCA794()
		[HLEFunction(nid : 0x66CCA794, version : 150)]
		public virtual int scePaf_66CCA794()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6D55F3F5, version = 150) public int scePaf_6D55F3F5()
		[HLEFunction(nid : 0x6D55F3F5, version : 150)]
		public virtual int scePaf_6D55F3F5()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x706ABBFF, version = 150) public int scePaf_706ABBFF()
		[HLEFunction(nid : 0x706ABBFF, version : 150)]
		public virtual int scePaf_706ABBFF()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x71B92320, version = 150) public int scePaf_71B92320()
		[HLEFunction(nid : 0x71B92320, version : 150)]
		public virtual int scePaf_71B92320()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7B7133D5, version = 150) public int scePaf_7B7133D5()
		[HLEFunction(nid : 0x7B7133D5, version : 150)]
		public virtual int scePaf_7B7133D5()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8166CA82, version = 150) public int scePaf_8166CA82()
		[HLEFunction(nid : 0x8166CA82, version : 150)]
		public virtual int scePaf_8166CA82()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8C5CC663, version = 150) public int scePaf_8C5CC663()
		[HLEFunction(nid : 0x8C5CC663, version : 150)]
		public virtual int scePaf_8C5CC663()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8E805192, version = 150) public int scePaf_8E805192()
		[HLEFunction(nid : 0x8E805192, version : 150)]
		public virtual int scePaf_8E805192()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9A418CCC, version = 150) public int scePaf_9A418CCC()
		[HLEFunction(nid : 0x9A418CCC, version : 150)]
		public virtual int scePaf_9A418CCC()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9CD6C5F4, version = 150) public int scePaf_9CD6C5F4()
		[HLEFunction(nid : 0x9CD6C5F4, version : 150)]
		public virtual int scePaf_9CD6C5F4()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9D854500, version = 150) public int scePaf_9D854500()
		[HLEFunction(nid : 0x9D854500, version : 150)]
		public virtual int scePaf_9D854500()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA631AC8B, version = 150) public int scePaf_A631AC8B()
		[HLEFunction(nid : 0xA631AC8B, version : 150)]
		public virtual int scePaf_A631AC8B()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB05D9677, version = 150) public int scePaf_B05D9677()
		[HLEFunction(nid : 0xB05D9677, version : 150)]
		public virtual int scePaf_B05D9677()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB110AF46, version = 150) public int scePaf_B110AF46()
		[HLEFunction(nid : 0xB110AF46, version : 150)]
		public virtual int scePaf_B110AF46()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBB89C9EA, version = 150) public int scePaf_BB89C9EA()
		[HLEFunction(nid : 0xBB89C9EA, version : 150)]
		public virtual int scePaf_BB89C9EA()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD229572C, version = 150) public int scePaf_D229572C()
		[HLEFunction(nid : 0xD229572C, version : 150)]
		public virtual int scePaf_D229572C()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD7DCB972, version = 150) public int scePaf_D7DCB972()
		[HLEFunction(nid : 0xD7DCB972, version : 150)]
		public virtual int scePaf_D7DCB972()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD9E2D6E1, version = 150) public int scePaf_D9E2D6E1()
		[HLEFunction(nid : 0xD9E2D6E1, version : 150)]
		public virtual int scePaf_D9E2D6E1()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE0B32AE8, version = 150) public int scePaf_E0B32AE8()
		[HLEFunction(nid : 0xE0B32AE8, version : 150)]
		public virtual int scePaf_E0B32AE8()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE49835DC, version = 150) public int scePaf_E49835DC()
		[HLEFunction(nid : 0xE49835DC, version : 150)]
		public virtual int scePaf_E49835DC()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFF9C876B, version = 150) public int scePaf_FF9C876B()
		[HLEFunction(nid : 0xFF9C876B, version : 150)]
		public virtual int scePaf_FF9C876B()
		{
			return 0;
		}
	}

}