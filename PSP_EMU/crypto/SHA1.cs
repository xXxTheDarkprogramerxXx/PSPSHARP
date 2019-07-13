﻿using System;

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

namespace pspsharp.crypto
{

	public class SHA1
	{

		public SHA1()
		{
		}

		public virtual sbyte[] doSHA1(sbyte[] bytes, int Length)
		{
			try
			{
				MessageDigest md = MessageDigest.getInstance("SHA-1");
				md.update(bytes, 0, Length);
				sbyte[] sha1Hash = md.digest();
				return sha1Hash;
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.ToString());
				System.Console.Write(e.StackTrace);
				return null;
			}
		}
	}
}