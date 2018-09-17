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
namespace pspsharp.media.codec
{
	/// <summary>
	/// Codec interface.
	/// 
	/// @author gid15
	/// 
	/// </summary>
	public interface ICodec
	{
		/// <summary>
		/// Initialize the codec.
		/// This method has to be called once before calling the decode() method.
		/// </summary>
		/// <param name="bytesPerFrame">  the number of bytes per input frame (block_align in ffmpeg) </param>
		/// <param name="channels">       the number of input channels </param>
		/// <param name="outputChannels"> the number of output channels </param>
		/// <param name="codingMode">     Atrac3 1=JOINT_STEREO / 0=STEREO </param>
		/// <returns>   0 success
		///         < 0 error code </returns>
		int init(int bytesPerFrame, int channels, int outputChannels, int codingMode);

		/// <summary>
		/// Decode a frame.
		/// </summary>
		/// <param name="inputAddr">   the address of the input buffer </param>
		/// <param name="inputLength"> the maximum Length of the input buffer </param>
		/// <param name="outputAddr">  the address where to store the decode samples </param>
		/// <returns>            0  no frame decoded (end of input stream)
		///                  < 0  error code
		///                  > 0  number of bytes consumed from the input buffer </returns>
		int decode(int inputAddr, int inputLength, int outputAddr);

		/// <returns>  the number of samples generated by one decode() call. </returns>
		int NumberOfSamples {get;}
	}

}