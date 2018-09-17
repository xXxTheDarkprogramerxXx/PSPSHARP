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
namespace pspsharp.HLE.modules
{
	//using Logger = org.apache.log4j.Logger;

	using LengthInfo = pspsharp.HLE.BufferInfo.LengthInfo;
	using Usage = pspsharp.HLE.BufferInfo.Usage;

	public class sceAta : HLEModule
	{
		//public static Logger log = Modules.getLogger("sceAta");

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBE6261DA, version = 150) public void sceAta_driver_BE6261DA(int unknown)
		[HLEFunction(nid : 0xBE6261DA, version : 150)]
		public virtual void sceAta_driver_BE6261DA(int unknown)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x4222D6F3, version = 150) public void sceAta_driver_4222D6F3()
		[HLEFunction(nid : 0x4222D6F3, version : 150)]
		public virtual void sceAta_driver_4222D6F3()
		{
			// Has no parameters
			// Stores 0 at hardware address 0xBD600010
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x4D225674, version = 150) public void sceAta_driver_4D225674()
		[HLEFunction(nid : 0x4D225674, version : 150)]
		public virtual void sceAta_driver_4D225674()
		{
			// Has no parameters
			// Stores 1 at hardware address 0xBD600010
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xAE100256, version = 150) public int sceAtaAhbSetAtaBlock()
		[HLEFunction(nid : 0xAE100256, version : 150)]
		public virtual int sceAtaAhbSetAtaBlock()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7DE9E14A, version = 150) public int sceAtaAhbEnableIntr()
		[HLEFunction(nid : 0x7DE9E14A, version : 150)]
		public virtual int sceAtaAhbEnableIntr()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x12805193, version = 150) public int sceAtaAhbSetupBus()
		[HLEFunction(nid : 0x12805193, version : 150)]
		public virtual int sceAtaAhbSetupBus()
		{
			// Has no parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xF61EAFC0, version = 150) public int sceAta_driver_F61EAFC0()
		[HLEFunction(nid : 0xF61EAFC0, version : 150)]
		public virtual int sceAta_driver_F61EAFC0()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x76C0923D, version = 150) public int sceAta_driver_76C0923D()
		[HLEFunction(nid : 0x76C0923D, version : 150)]
		public virtual int sceAta_driver_76C0923D()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE3E1EED7, version = 150) public int sceAta_driver_E3E1EED7()
		[HLEFunction(nid : 0xE3E1EED7, version : 150)]
		public virtual int sceAta_driver_E3E1EED7()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x0B53CAD8, version = 150) public int sceAta_driver_0B53CAD8()
		[HLEFunction(nid : 0x0B53CAD8, version : 150)]
		public virtual int sceAta_driver_0B53CAD8()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC74F04B7, version = 150) public int sceAtaExecPacketCmd(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=36, usage=pspsharp.HLE.BufferInfo.Usage.in) pspsharp.HLE.TPointer driveStructure, int unknown1, int unknown2, int unknown3, int unknown4, int operationCode, @BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=64, usage=pspsharp.HLE.BufferInfo.Usage.inout) pspsharp.HLE.TPointer8 unknown5)
		[HLEFunction(nid : 0xC74F04B7, version : 150)]
		public virtual int sceAtaExecPacketCmd(TPointer driveStructure, int unknown1, int unknown2, int unknown3, int unknown4, int operationCode, TPointer8 unknown5)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBDD30DEE, version = 150) public int sceAtaExecPacketCmdIE()
		[HLEFunction(nid : 0xBDD30DEE, version : 150)]
		public virtual int sceAtaExecPacketCmdIE()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x010F750B, version = 150) public int sceAtaExecSetFeaturesCmd()
		[HLEFunction(nid : 0x010F750B, version : 150)]
		public virtual int sceAtaExecSetFeaturesCmd()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x1C29566B, version = 150) public int sceAtaGetIntrStateFlag()
		[HLEFunction(nid : 0x1C29566B, version : 150)]
		public virtual int sceAtaGetIntrStateFlag()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7F551D66, version = 150) public int sceAtaSetIntrStateFlag()
		[HLEFunction(nid : 0x7F551D66, version : 150)]
		public virtual int sceAtaSetIntrStateFlag()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x68DEA2FF, version = 150) public int sceAtaClearIntrStateFlag()
		[HLEFunction(nid : 0x68DEA2FF, version : 150)]
		public virtual int sceAtaClearIntrStateFlag()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD1E6E175, version = 150) public int sceAtaEnableClkIo(int unknown)
		[HLEFunction(nid : 0xD1E6E175, version : 150)]
		public virtual int sceAtaEnableClkIo(int unknown)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xAC800B1D, version = 150) public int sceAtaDisableClkIo()
		[HLEFunction(nid : 0xAC800B1D, version : 150)]
		public virtual int sceAtaDisableClkIo()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC8BC8B83, version = 150) public int sceAtaStart()
		[HLEFunction(nid : 0xC8BC8B83, version : 150)]
		public virtual int sceAtaStart()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xCDC50BF0, version = 150) public int sceAtaStop()
		[HLEFunction(nid : 0xCDC50BF0, version : 150)]
		public virtual int sceAtaStop()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x52D9A4CA, version = 150) public int sceAtaGetAtaDrive()
		[HLEFunction(nid : 0x52D9A4CA, version : 150)]
		public virtual int sceAtaGetAtaDrive()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xEBB91566, version = 150) public int sceAtaSetupAtaBlock()
		[HLEFunction(nid : 0xEBB91566, version : 150)]
		public virtual int sceAtaSetupAtaBlock()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6C58F096, version = 150) public int sceAtaWaitBusBusy1()
		[HLEFunction(nid : 0x6C58F096, version : 150)]
		public virtual int sceAtaWaitBusBusy1()
		{
			// Has no parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC21318E6, version = 150) public int sceAtaWaitBusBusy2()
		[HLEFunction(nid : 0xC21318E6, version : 150)]
		public virtual int sceAtaWaitBusBusy2()
		{
			// Has no parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8DE58134, version = 660) public int sceAtaWaitBusBusy2_660()
		[HLEFunction(nid : 0x8DE58134, version : 660)]
		public virtual int sceAtaWaitBusBusy2_660()
		{
			// Has no parameters
			return sceAtaWaitBusBusy2();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA77C230B, version = 150) public int sceAtaWaitBusBusyIE()
		[HLEFunction(nid : 0xA77C230B, version : 150)]
		public virtual int sceAtaWaitBusBusyIE()
		{
			// Has no parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2F7BE141, version = 660) public int sceAtaWaitBusBusyIE_660()
		[HLEFunction(nid : 0x2F7BE141, version : 660)]
		public virtual int sceAtaWaitBusBusyIE_660()
		{
			// Has no parameters
			return sceAtaWaitBusBusyIE();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3AA3FA39, version = 150) public int sceAtaSelectDevice(int unknown)
		[HLEFunction(nid : 0x3AA3FA39, version : 150)]
		public virtual int sceAtaSelectDevice(int unknown)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xACCEE63F, version = 150) public int sceAta_driver_ACCEE63F()
		[HLEFunction(nid : 0xACCEE63F, version : 150)]
		public virtual int sceAta_driver_ACCEE63F()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6E639701, version = 150) public int sceAtaScanDevice(pspsharp.HLE.TPointer driveStructure)
		[HLEFunction(nid : 0x6E639701, version : 150)]
		public virtual int sceAtaScanDevice(TPointer driveStructure)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB863FD83, version = 150) public int sceAtaCheckDeviceReady()
		[HLEFunction(nid : 0xB863FD83, version : 150)]
		public virtual int sceAtaCheckDeviceReady()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB6BED47E, version = 150) public int sceAtaAccessDataPort()
		[HLEFunction(nid : 0xB6BED47E, version : 150)]
		public virtual int sceAtaAccessDataPort()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6DE1C65F, version = 150) public int sceAtaAccessDataPortIE()
		[HLEFunction(nid : 0x6DE1C65F, version : 150)]
		public virtual int sceAtaAccessDataPortIE()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x84E14690, version = 150) public int sceAta_driver_84E14690()
		[HLEFunction(nid : 0x84E14690, version : 150)]
		public virtual int sceAta_driver_84E14690()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC7B02795, version = 150) public int sceAta_driver_C7B02795()
		[HLEFunction(nid : 0xC7B02795, version : 150)]
		public virtual int sceAta_driver_C7B02795()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x1F05F48C, version = 150) public int sceAta_driver_1F05F48C()
		[HLEFunction(nid : 0x1F05F48C, version : 150)]
		public virtual int sceAta_driver_1F05F48C()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBA09142A, version = 150) public int sceAta_driver_BA09142A()
		[HLEFunction(nid : 0xBA09142A, version : 150)]
		public virtual int sceAta_driver_BA09142A()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x1C8DA2FD, version = 150) public int sceAtaSetDeviceIntr()
		[HLEFunction(nid : 0x1C8DA2FD, version : 150)]
		public virtual int sceAtaSetDeviceIntr()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2AE26E08, version = 150) public int sceAta_driver_2AE26E08()
		[HLEFunction(nid : 0x2AE26E08, version : 150)]
		public virtual int sceAta_driver_2AE26E08()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9D98086E, version = 150) public int sceAtaSetBusErrorIntr()
		[HLEFunction(nid : 0x9D98086E, version : 150)]
		public virtual int sceAtaSetBusErrorIntr()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x689FCB7D, version = 150) public int sceAta_driver_689FCB7D()
		[HLEFunction(nid : 0x689FCB7D, version : 150)]
		public virtual int sceAta_driver_689FCB7D()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC144826E, version = 150) public int sceAta_driver_C144826E(int unknown)
		[HLEFunction(nid : 0xC144826E, version : 150)]
		public virtual int sceAta_driver_C144826E(int unknown)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFCF939D9, version = 150) public int sceAtaGetAtaCallBack()
		[HLEFunction(nid : 0xFCF939D9, version : 150)]
		public virtual int sceAtaGetAtaCallBack()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9AE67E14, version = 150) public int sceAtaGetDriveStat()
		[HLEFunction(nid : 0x9AE67E14, version : 150)]
		public virtual int sceAtaGetDriveStat()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x110D3739, version = 150) public void sceAtaSetDriveStat(int driveNumber, int driveStat)
		[HLEFunction(nid : 0x110D3739, version : 150)]
		public virtual void sceAtaSetDriveStat(int driveNumber, int driveStat)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7BBA095C, version = 150) public void sceAtaClearDriveStat(int driveNumber, int maskDriveStat)
		[HLEFunction(nid : 0x7BBA095C, version : 150)]
		public virtual void sceAtaClearDriveStat(int driveNumber, int maskDriveStat)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3A61BF97, version = 150) public int sceAtaIsNormalDriveStat()
		[HLEFunction(nid : 0x3A61BF97, version : 150)]
		public virtual int sceAtaIsNormalDriveStat()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xACFF7CB5, version = 150) public int sceAta_driver_ACFF7CB5()
		[HLEFunction(nid : 0xACFF7CB5, version : 150)]
		public virtual int sceAta_driver_ACFF7CB5()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD8E525CB, version = 150) public int sceAtaGetIntrFlag()
		[HLEFunction(nid : 0xD8E525CB, version : 150)]
		public virtual int sceAtaGetIntrFlag()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB7F5B2CA, version = 150) public int sceAtaSetIntrFlag()
		[HLEFunction(nid : 0xB7F5B2CA, version : 150)]
		public virtual int sceAtaSetIntrFlag()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x638EEA14, version = 150) public int sceAta_driver_638EEA14()
		[HLEFunction(nid : 0x638EEA14, version : 150)]
		public virtual int sceAta_driver_638EEA14()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB5982381, version = 150) public int sceAta_driver_B5982381()
		[HLEFunction(nid : 0xB5982381, version : 150)]
		public virtual int sceAta_driver_B5982381()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7C6B31D8, version = 150) public int sceAta_driver_7C6B31D8()
		[HLEFunction(nid : 0x7C6B31D8, version : 150)]
		public virtual int sceAta_driver_7C6B31D8()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6FD8E2AB, version = 150) public int sceAta_driver_6FD8E2AB()
		[HLEFunction(nid : 0x6FD8E2AB, version : 150)]
		public virtual int sceAta_driver_6FD8E2AB()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBE07B3A7, version = 150) public int sceAta_driver_BE07B3A7()
		[HLEFunction(nid : 0xBE07B3A7, version : 150)]
		public virtual int sceAta_driver_BE07B3A7()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xDCC8A89E, version = 150) public int sceAtaIsUmdDrive()
		[HLEFunction(nid : 0xDCC8A89E, version : 150)]
		public virtual int sceAtaIsUmdDrive()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6FC42083, version = 150) public int sceAtaIsDvdDrive()
		[HLEFunction(nid : 0x6FC42083, version : 150)]
		public virtual int sceAtaIsDvdDrive()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD225B43E, version = 660) public int sceAta_driver_D225B43E(int driveNumber)
		[HLEFunction(nid : 0xD225B43E, version : 660)]
		public virtual int sceAta_driver_D225B43E(int driveNumber)
		{
			if (driveNumber != 0)
			{
				return 0;
			}
			return 0; // Return the address of a 8-bytes long structure
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8B32C14C, version = 660) public int sceAta_driver_8B32C14C()
		[HLEFunction(nid : 0x8B32C14C, version : 660)]
		public virtual int sceAta_driver_8B32C14C()
		{
			// Has no parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC344D497, version = 660) public int sceAta_driver_C344D497(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=36, usage=pspsharp.HLE.BufferInfo.Usage.in) pspsharp.HLE.TPointer driveStructure, int unknown, int mode, int Length)
		[HLEFunction(nid : 0xC344D497, version : 660)]
		public virtual int sceAta_driver_C344D497(TPointer driveStructure, int unknown, int mode, int Length)
		{
			// unknown: possible values: 0x2, 0x4, 0x80
			// mode=1: write bytes to Ata drive
			// mode=2: read bytes from Ata drive
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3265D064, version = 660) public int sceAta_driver_3265D064(int unknown1, int unknown2)
		[HLEFunction(nid : 0x3265D064, version : 660)]
		public virtual int sceAta_driver_3265D064(int unknown1, int unknown2)
		{
			return 0;
		}

		/*
		 * Performs an ATA soft reset
		 */
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xEE230A74, version = 660) public int sceAta_driver_EE230A74()
		[HLEFunction(nid : 0xEE230A74, version : 660)]
		public virtual int sceAta_driver_EE230A74()
		{
			// Has no parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x0092958D, version = 150) public int sceAta_driver_0092958D()
		[HLEFunction(nid : 0x0092958D, version : 150)]
		public virtual int sceAta_driver_0092958D()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x0AC8F250, version = 150) public int sceAta_driver_0AC8F250()
		[HLEFunction(nid : 0x0AC8F250, version : 150)]
		public virtual int sceAta_driver_0AC8F250()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x1829BF5D, version = 150) public int sceAta_driver_1829BF5D()
		[HLEFunction(nid : 0x1829BF5D, version : 150)]
		public virtual int sceAta_driver_1829BF5D()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2065EC41, version = 150) public int sceAta_driver_2065EC41()
		[HLEFunction(nid : 0x2065EC41, version : 150)]
		public virtual int sceAta_driver_2065EC41()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2248D797, version = 150) public int sceAta_driver_2248D797()
		[HLEFunction(nid : 0x2248D797, version : 150)]
		public virtual int sceAta_driver_2248D797()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2F403F95, version = 150) public int sceAta_driver_2F403F95()
		[HLEFunction(nid : 0x2F403F95, version : 150)]
		public virtual int sceAta_driver_2F403F95()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x33ADDDB5, version = 150) public int sceAta_driver_33ADDDB5()
		[HLEFunction(nid : 0x33ADDDB5, version : 150)]
		public virtual int sceAta_driver_33ADDDB5()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x372F6826, version = 150) public int sceAta_driver_372F6826()
		[HLEFunction(nid : 0x372F6826, version : 150)]
		public virtual int sceAta_driver_372F6826()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3AE54125, version = 150) public int sceAta_driver_3AE54125()
		[HLEFunction(nid : 0x3AE54125, version : 150)]
		public virtual int sceAta_driver_3AE54125()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x40EB3E33, version = 150) public int sceAta_driver_40EB3E33()
		[HLEFunction(nid : 0x40EB3E33, version : 150)]
		public virtual int sceAta_driver_40EB3E33()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x55268789, version = 150) public int sceAta_driver_55268789()
		[HLEFunction(nid : 0x55268789, version : 150)]
		public virtual int sceAta_driver_55268789()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x56EFF1AD, version = 150) public int sceAta_driver_56EFF1AD()
		[HLEFunction(nid : 0x56EFF1AD, version : 150)]
		public virtual int sceAta_driver_56EFF1AD()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x585E69C2, version = 150) public int sceAta_driver_585E69C2()
		[HLEFunction(nid : 0x585E69C2, version : 150)]
		public virtual int sceAta_driver_585E69C2()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x64DA79ED, version = 150) public int sceAta_driver_64DA79ED()
		[HLEFunction(nid : 0x64DA79ED, version : 150)]
		public virtual int sceAta_driver_64DA79ED()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6BC8735F, version = 150) public int sceAta_driver_6BC8735F()
		[HLEFunction(nid : 0x6BC8735F, version : 150)]
		public virtual int sceAta_driver_6BC8735F()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x703189EB, version = 150) public int sceAta_driver_703189EB()
		[HLEFunction(nid : 0x703189EB, version : 150)]
		public virtual int sceAta_driver_703189EB()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x749BF82F, version = 150) public int sceAta_driver_749BF82F()
		[HLEFunction(nid : 0x749BF82F, version : 150)]
		public virtual int sceAta_driver_749BF82F()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x78A45AD9, version = 150) public int sceAta_driver_78A45AD9()
		[HLEFunction(nid : 0x78A45AD9, version : 150)]
		public virtual int sceAta_driver_78A45AD9()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x790CA1AA, version = 150) public int sceAta_driver_790CA1AA()
		[HLEFunction(nid : 0x790CA1AA, version : 150)]
		public virtual int sceAta_driver_790CA1AA()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7A307EE2, version = 150) public int sceAta_driver_7A307EE2()
		[HLEFunction(nid : 0x7A307EE2, version : 150)]
		public virtual int sceAta_driver_7A307EE2()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7B9FECFA, version = 150) public int sceAta_driver_7B9FECFA()
		[HLEFunction(nid : 0x7B9FECFA, version : 150)]
		public virtual int sceAta_driver_7B9FECFA()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7EAAF3DE, version = 150) public int sceAta_driver_7EAAF3DE()
		[HLEFunction(nid : 0x7EAAF3DE, version : 150)]
		public virtual int sceAta_driver_7EAAF3DE()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x807240E3, version = 150) public int sceAta_driver_807240E3()
		[HLEFunction(nid : 0x807240E3, version : 150)]
		public virtual int sceAta_driver_807240E3()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8611DD55, version = 150) public int sceAta_driver_8611DD55()
		[HLEFunction(nid : 0x8611DD55, version : 150)]
		public virtual int sceAta_driver_8611DD55()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8862E565, version = 150) public int sceAta_driver_8862E565()
		[HLEFunction(nid : 0x8862E565, version : 150)]
		public virtual int sceAta_driver_8862E565()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8C975118, version = 150) public int sceAta_driver_8C975118()
		[HLEFunction(nid : 0x8C975118, version : 150)]
		public virtual int sceAta_driver_8C975118()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9C57F0F4, version = 150) public int sceAta_driver_9C57F0F4()
		[HLEFunction(nid : 0x9C57F0F4, version : 150)]
		public virtual int sceAta_driver_9C57F0F4()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA8704EAE, version = 150) public int sceAta_driver_A8704EAE()
		[HLEFunction(nid : 0xA8704EAE, version : 150)]
		public virtual int sceAta_driver_A8704EAE()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xAA2E6A18, version = 150) public int sceAta_driver_AA2E6A18()
		[HLEFunction(nid : 0xAA2E6A18, version : 150)]
		public virtual int sceAta_driver_AA2E6A18()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xAD4EE538, version = 150) public int sceAta_driver_AD4EE538()
		[HLEFunction(nid : 0xAD4EE538, version : 150)]
		public virtual int sceAta_driver_AD4EE538()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xB750550D, version = 150) public int sceAta_driver_B750550D()
		[HLEFunction(nid : 0xB750550D, version : 150)]
		public virtual int sceAta_driver_B750550D()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBB8C811E, version = 150) public int sceAta_driver_BB8C811E()
		[HLEFunction(nid : 0xBB8C811E, version : 150)]
		public virtual int sceAta_driver_BB8C811E()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xBC04ADE2, version = 150) public int sceAta_driver_BC04ADE2()
		[HLEFunction(nid : 0xBC04ADE2, version : 150)]
		public virtual int sceAta_driver_BC04ADE2()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC200327E, version = 150) public int sceAta_driver_C200327E()
		[HLEFunction(nid : 0xC200327E, version : 150)]
		public virtual int sceAta_driver_C200327E()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC68EDAA4, version = 150) public int sceAta_driver_C68EDAA4()
		[HLEFunction(nid : 0xC68EDAA4, version : 150)]
		public virtual int sceAta_driver_C68EDAA4()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC8B7A389, version = 150) public int sceAta_driver_C8B7A389()
		[HLEFunction(nid : 0xC8B7A389, version : 150)]
		public virtual int sceAta_driver_C8B7A389()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xCB7D083B, version = 150) public int sceAta_driver_CB7D083B()
		[HLEFunction(nid : 0xCB7D083B, version : 150)]
		public virtual int sceAta_driver_CB7D083B()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD1F7BE40, version = 150) public int sceAta_driver_D1F7BE40()
		[HLEFunction(nid : 0xD1F7BE40, version : 150)]
		public virtual int sceAta_driver_D1F7BE40()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD3AB80C2, version = 150) public int sceAta_driver_D3AB80C2()
		[HLEFunction(nid : 0xD3AB80C2, version : 150)]
		public virtual int sceAta_driver_D3AB80C2()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD9CB3246, version = 150) public int sceAta_driver_D9CB3246()
		[HLEFunction(nid : 0xD9CB3246, version : 150)]
		public virtual int sceAta_driver_D9CB3246()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xDE774356, version = 150) public int sceAta_driver_DE774356()
		[HLEFunction(nid : 0xDE774356, version : 150)]
		public virtual int sceAta_driver_DE774356()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE568A604, version = 150) public int sceAta_driver_E568A604()
		[HLEFunction(nid : 0xE568A604, version : 150)]
		public virtual int sceAta_driver_E568A604()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xEBAEAAE8, version = 150) public int sceAta_driver_EBAEAAE8()
		[HLEFunction(nid : 0xEBAEAAE8, version : 150)]
		public virtual int sceAta_driver_EBAEAAE8()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFB9EA525, version = 150) public int sceAta_driver_FB9EA525()
		[HLEFunction(nid : 0xFB9EA525, version : 150)]
		public virtual int sceAta_driver_FB9EA525()
		{
			return 0;
		}
	}

}