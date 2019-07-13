using System.Runtime.InteropServices;

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
namespace pspsharp.util
{
	//using Logger = org.apache.log4j.Logger;

	/// 
	/// <summary>
	/// @author shadow
	/// </summary>
	public class NativeCpuInfo
	{
		private static Logger log = Logger.getLogger("cpuinfo");
		private static bool isAvailable = false;

		static NativeCpuInfo()
		{
			try
			{
//JAVA TO C# CONVERTER TODO TASK: The library is specified in the 'DllImport' attribute for .NET:
//				System.loadLibrary("cpuinfo");
				isAvailable = true;
			}
			catch (UnsatisfiedLinkError ule)
			{
				System.Console.WriteLine("Loading cpuinfo native library", ule);
			}
		}

		public static bool Available
		{
			get
			{
				return isAvailable;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern void init();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasSSE();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasSSE2();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasSSE3();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasSSSE3();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasSSE41();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasSSE42();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasAVX();

//JAVA TO C# CONVERTER TODO TASK: Replace 'unknown' with the appropriate dll name:
		[DllImport("unknown")]
		public static extern bool hasAVX2();

		public static void printInfo()
		{
			System.Console.WriteLine("Supports SSE    " + hasSSE());
			System.Console.WriteLine("Supports SSE2   " + hasSSE2());
			System.Console.WriteLine("Supports SSE3   " + hasSSE3());
			System.Console.WriteLine("Supports SSSE3  " + hasSSSE3());
			System.Console.WriteLine("Supports SSE4.1 " + hasSSE41());
			System.Console.WriteLine("Supports SSE4.2 " + hasSSE42());
			System.Console.WriteLine("Supports AVX    " + hasAVX());
			System.Console.WriteLine("Supports AVX2   " + hasAVX2());
		}
	}

}