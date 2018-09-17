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
namespace pspsharp.graphics.RE.software
{
	/// <summary>
	/// @author gid15
	/// 
	/// An interface to access a texture in a random pixel order.
	/// I.e., it is not assumed that the pixels are read in a sequential order.
	/// </summary>
	public interface IRandomTextureAccess
	{
		/// <summary>
		/// Reads a texture pixel color at a given texture coordinate.
		/// 
		/// The pixel color is always returned in the format GU_COLOR_8888 (ABGR).
		/// 
		/// The (u,v) coordinate must be in the following valid range:
		/// - (0, 0): the texture upper left corner
		/// - (width - 1, height - 1): the texture lower right corner
		/// </summary>
		/// <param name="u">	  the texture u coordinate (X-Axis coordinate) </param>
		/// <param name="v">   the texture v coordinate (Y-Axis coordinate) </param>
		/// <returns>    the pixel color in the format GU_COLOR_8888 (ABGR) </returns>
		int readPixel(int u, int v);

		int Width {get;}
		int Height {get;}
	}

}