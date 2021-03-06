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
namespace pspsharp.media.codec.util
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static Math.max;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static Math.min;
	using IMemoryWriter = pspsharp.memory.IMemoryWriter;
	using MemoryWriter = pspsharp.memory.MemoryWriter;

	public class CodecUtils
	{
		// FLT_EPSILON the minimum positive number such that 1.0 + FLT_EPSILON != 1.0
		public const float FLT_EPSILON = 1.19209290E-07F;
		public const float M_SQRT1_2 = 0.707106781186547524401f; // 1/Sqrt(2)
		public static readonly float M_PI = (float) Math.PI;
		public const float M_SQRT2 = 1.41421356237309504880f; // Sqrt(2)

		public static readonly int[] ff_log2_tab = new int[] {0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7};

		private static int convertSampleFloatToInt16(float sample)
		{
			return min(max((int)(sample * 32768f + 0.5f), -32768), 32767) & 0xFFFF;
		}

		public static void writeOutput(float[][] samples, int outputAddr, int numberOfSamples, int decodedChannels, int outputChannels)
		{
			IMemoryWriter writer = MemoryWriter.getMemoryWriter(outputAddr, numberOfSamples * 2 * outputChannels, 2);
			switch (outputChannels)
			{
				case 1:
					for (int i = 0; i < numberOfSamples; i++)
					{
						int sample = convertSampleFloatToInt16(samples[0][i]);
						writer.writeNext(sample);
					}
					break;
				case 2:
					if (decodedChannels == 1)
					{
						// Convert decoded mono into output stereo
						for (int i = 0; i < numberOfSamples; i++)
						{
							int sample = convertSampleFloatToInt16(samples[0][i]);
							writer.writeNext(sample);
							writer.writeNext(sample);
						}
					}
					else
					{
						for (int i = 0; i < numberOfSamples; i++)
						{
							int lsample = convertSampleFloatToInt16(samples[0][i]);
							int rsample = convertSampleFloatToInt16(samples[1][i]);
							writer.writeNext(lsample);
							writer.writeNext(rsample);
						}
					}
					break;
			}
			writer.flush();
		}

		public static int avLog2(int n)
		{
			if (n == 0)
			{
				return 0;
			}
			return 31 - Integer.numberOfLeadingZeros(n);
		}

		private static readonly float log2 = (float) System.Math.Log(2.0);

		public static float log2f(float n)
		{
			return (float) System.Math.Log(n) / log2;
		}

		public static int lrintf(float n)
		{
			return (int) Math.Round(n);
		}

		public static float exp2f(float n)
		{
			return (float) System.Math.Pow(2.0, n);
		}

		public static float sqrtf(float n)
		{
			return (float) System.Math.Sqrt(n);
		}

		public static float cosf(float n)
		{
			return (float) System.Math.Cos(n);
		}

		public static float sinf(float n)
		{
			return (float) System.Math.Sin(n);
		}

		public static float atanf(float n)
		{
			return (float) System.Math.Atan(n);
		}

		public static float atan2f(float y, float x)
		{
			return (float) System.Math.Atan2(y, x);
		}
	}

}