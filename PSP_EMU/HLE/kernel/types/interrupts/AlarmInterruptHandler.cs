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
namespace pspsharp.HLE.kernel.types.interrupts
{
	public class AlarmInterruptHandler : AbstractAllegrexInterruptHandler
	{
		public AlarmInterruptHandler(int address, int argument) : base(address, argument)
		{
		}

		public virtual int Argument
		{
			get
			{
				return getArgument(0);
			}
			set
			{
				setArgument(0, value);
			}
		}

	}

}