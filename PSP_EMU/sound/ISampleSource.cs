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
namespace pspsharp.sound
{
	/// <summary>
	/// @author gid15
	/// 
	/// </summary>
	public interface ISampleSource
	{
		/// <returns> sample in stereo (lower 16 bits = left, higher 16 bits = right) </returns>
		int NextSample {get;}
		void resetToStart();
		bool Ended {get;}
	}

}