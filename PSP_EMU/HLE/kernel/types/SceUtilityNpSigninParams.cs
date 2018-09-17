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
namespace pspsharp.HLE.kernel.types
{
	public class SceUtilityNpSigninParams : pspUtilityBaseDialog
	{
		public const int NP_SIGNING_STATUS_OK = 1;
		public const int NP_SIGNING_STATUS_CANCEL = 2;
		public int signinStatus;
		public int unknown2;
		public int unknown3;
		public int unknown4;

		protected internal override void read()
		{
			@base = new pspUtilityDialogCommon();
			read(@base);
			MaxSize = @base.totalSizeof();

			signinStatus = read32();
			unknown2 = read32();
			unknown3 = read32();
			unknown4 = read32();
		}

		protected internal override void write()
		{
			write(@base);
			MaxSize = @base.totalSizeof();

			write32(signinStatus);
			write32(unknown2);
			write32(unknown3);
			write32(unknown4);
		}

		public override int @sizeof()
		{
			return @base.totalSizeof();
		}

		public override string ToString()
		{
			return string.Format("signinStatus={0:D}, unknown2=0x{1:X}, unknown3=0x{2:X}, unknown4=0x{3:X}", signinStatus, unknown2, unknown3, unknown4);
		}
	}

}