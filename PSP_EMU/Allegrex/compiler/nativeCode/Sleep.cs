﻿using System.Threading;

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
namespace pspsharp.Allegrex.compiler.nativeCode
{

	/// <summary>
	/// @author gid15
	/// 
	/// </summary>
	public class Sleep : AbstractNativeCodeSequence
	{
		public static void call(int regDoubleLow, int regDoubleHigh)
		{
			int doubleLow = getRegisterValue(regDoubleLow);
			int doubleHigh = getRegisterValue(regDoubleHigh);
			double? sleepSeconds = Double.longBitsToDouble(getLong(doubleLow, doubleHigh));

			Compiler.Console.WriteLine("Sleeping " + sleepSeconds + " s");

			try
			{
				Thread.Sleep((long)(sleepSeconds * 1000));
			}
			catch (InterruptedException)
			{
				// Ignore exception
			}
		}
	}

}