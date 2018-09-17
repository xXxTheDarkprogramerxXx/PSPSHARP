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
	/*
	 * GPS Satellite Info Structure for sceUsbGpsGetData().
	 * Based on MapThis! homebrew v5.2
	 */
	public class pspUsbGpsSatInfo : pspAbstractMemoryMappedStructure
	{
		public const int SIZEOF = 8;
		public int id;
		public int elevation;
		public short azimuth;
		public int snr; // Signal-to-Noise Ratio
		public int good;
		public short garbage;

		protected internal override void read()
		{
			id = read8();
			elevation = read8();
			azimuth = (short) read16();
			snr = read8();
			good = read8();
			garbage = (short) read16();
		}

		protected internal override void write()
		{
			write8((sbyte) id);
			write8((sbyte) elevation);
			write16(azimuth);
			write8((sbyte) snr);
			write8((sbyte) good);
			write16(garbage);
		}

		public override int @sizeof()
		{
			return SIZEOF;
		}
	}

}