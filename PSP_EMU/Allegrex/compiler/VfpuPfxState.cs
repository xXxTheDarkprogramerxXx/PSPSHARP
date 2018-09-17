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
namespace pspsharp.Allegrex.compiler
{
	/// <summary>
	/// @author gid15
	/// 
	/// </summary>
	public class VfpuPfxState
	{
		private bool known;

		public VfpuPfxState()
		{
			reset();
		}

		public virtual void reset()
		{
			known = false;
		}

		public virtual bool Known
		{
			get
			{
				return known;
			}
			set
			{
				this.known = value;
			}
		}

		public virtual bool Unknown
		{
			get
			{
				return !known;
			}
		}

	}

}