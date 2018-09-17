﻿using System.Text;

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
namespace pspsharp.HLE.kernel.types
{
	public class SceUtilityOskParams : pspUtilityBaseDialog
	{
		public int oskDataCount; // Number of input fields (PSPSDK).
		public int oskDataAddr;
		public SceUtilityOskData oskData;
		public int oskState; // SceUtilityOskState (PSPSDK): internal status of this OSK.
			public const int PSP_UTILITY_OSK_STATE_NONE = 0;
			public const int PSP_UTILITY_OSK_STATE_INITIALIZING = 1;
			public const int PSP_UTILITY_OSK_STATE_INITIALIZED = 2;
			public const int PSP_UTILITY_OSK_STATE_OPEN = 3;
			public const int PSP_UTILITY_OSK_STATE_CLOSING = 4;
			public const int PSP_UTILITY_OSK_STATE_CLOSED = 5;
		public int errorCode;

		public class SceUtilityOskData : pspAbstractMemoryMappedStructure
		{
			public int inputMode; // How to parse the input chars.
				public const int PSP_UTILITY_OSK_DATA_INPUT_MODE_NONE = 0;
				public const int PSP_UTILITY_OSK_DATA_INPUT_MODE_JPN_CONVERT = 1;
				public const int PSP_UTILITY_OSK_DATA_INPUT_MODE_JPN_CHARS = 2;
				public const int PSP_UTILITY_OSK_DATA_INPUT_MODE_LATIN_CHARS = 3;
			public int inputAttr;
			public int language;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_SYSTEM = 0;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_JAPANESE = 1;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_ENGLISH = 2;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_FRENCH = 3;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_SPANISH = 4;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_GERMAN = 5;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_ITALIAN = 6;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_DUTCH = 7;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_PORTUGUESE = 8;
				public const int PSP_UTILITY_OSK_DATA_LANGUAGE_RUSSIAN = 9;
			public int hide; // Seems to be related to the visibility of the input FieldInfo.
				public const int PSP_UTILITY_OSK_DATA_SHOW = 0;
				public const int PSP_UTILITY_OSK_DATA_HIDE = 1;
			public int inputAllowCharType; // ORed flag that specifies which char types can be accepted.
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_ALL = 0x0;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_NUM = 0x1;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_ENG = 0x2;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_LOWERCASE = 0x4;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_UPPERCASE = 0x8;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_NUM = 0x100;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN = 0x200;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_LOWERCASE = 0x400;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_UPPERCASE = 0x800;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_HIRAGANA = 0x1000;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_HALF_KATAKANA = 0x2000;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_KATAKANA = 0x4000;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_KANJI = 0x8000;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_CYRILLIC_LOWERCASE = 0x10000;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_CYRILLIC_UPPERCASE = 0x20000;
				public const int PSP_UTILITY_OSK_DATA_CHAR_ALLOW_URL = 0x80000;
			public int lines;
			public int showInputText;
			public int descAddr;
			public string desc;
			public int inTextAddr;
			public string inText;
			public int outTextLength;
			public int outTextAddr;
			public string outText;
			public int result;
				public const int PSP_UTILITY_OSK_DATA_NOT_CHANGED = 0;
				public const int PSP_UTILITY_OSK_DATA_CANCELED = 1;
				public const int PSP_UTILITY_OSK_DATA_CHANGED = 2;
			public int outTextLimit;

			protected internal override void read()
			{
				inputMode = read32();
				inputAttr = read32();
				language = read32();
				hide = read32();
				inputAllowCharType = read32();
				lines = read32();
				showInputText = read32();
				descAddr = read32();
				desc = readStringUTF16Z(descAddr);
				inTextAddr = read32();
				inText = readStringUTF16Z(inTextAddr);
				outTextLength = read32();
				outTextAddr = read32();
				outText = readStringUTF16Z(outTextAddr);
				result = read32();
				outTextLimit = read32();
			}

			internal static void replaceFullWidthChar(StringBuilder s, int index, int codePointOffset)
			{
				int codePoint = s.codePointAt(index);
				char[] chars = new char[2];
				if (Character.toChars(codePoint + codePointOffset, chars, 0) == 1)
				{
					s[index] = chars[0];
				}
			}

			protected internal override void write()
			{
				write32(inputMode); // Offset 0
				write32(inputAttr); // Offset 4
				write32(language); // Offset 8
				write32(hide); // Offset 12
				write32(inputAllowCharType); // Offset 16
				write32(lines); // Offset 20
				write32(showInputText); // Offset 24
				write32(descAddr); // Offset 28
				writeStringUTF16Z(descAddr, desc);
				write32(inTextAddr); // Offset 32
				writeStringUTF16Z(inTextAddr, inText);
				int maxTextOutLength = outTextLength - 1;
				if (outTextLimit > 0 && outTextLimit < maxTextOutLength)
				{
					maxTextOutLength = outTextLimit;
				}
				int Length = System.Math.Min(outText.Length, maxTextOutLength);

				// When the following values are set for the inputAllowCharType flag set,
				// the characters returned in the output text FieldInfo are full width letters/digits:
				// - PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN
				// - PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_LOWERCASE
				// - PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_UPPERCASE
				if ((inputAllowCharType & (PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN | PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_LOWERCASE | PSP_UTILITY_OSK_DATA_CHAR_ALLOW_JPN_UPPERCASE)) != 0)
				{
					StringBuilder s = new StringBuilder(outText);
					for (int i = 0; i < Length; i++)
					{
						int codePoint = s.codePointAt(i);
						if (codePoint >= 65 && codePoint <= 90)
						{ // A-Z
							replaceFullWidthChar(s, i, 0xFF21 - 65);
						}
						else if (codePoint >= 97 && codePoint <= 122)
						{ // a-z
							replaceFullWidthChar(s, i, 0xFF41 - 97);
						}
						else if (codePoint >= 48 && codePoint <= 57)
						{ // 0-9
							replaceFullWidthChar(s, i, 0xFF10 - 48);
						}
					}
					outText = s.ToString();
				}
				writeStringUTF16Z(outTextAddr, outText.Substring(0, Length));
				write32(outTextLength); // Offset 36
				write32(outTextAddr); // Offset 40
				write32(result); // Offset 44
				write32(outTextLimit); // Offset 48
			}

			public override int @sizeof()
			{
				return 13 * 4;
			}
		}

		protected internal override void read()
		{
			@base = new pspUtilityDialogCommon();
			read(@base);
			MaxSize = @base.totalSizeof();

			oskDataCount = read32();
			oskDataAddr = read32();
			if (oskDataAddr != 0)
			{
				oskData = new SceUtilityOskData();
				oskData.read(mem, oskDataAddr);
			}
			else
			{
				oskData = null;
			}
			oskState = read32();
			errorCode = read32();
		}

		protected internal override void write()
		{
			write(@base);
			MaxSize = @base.totalSizeof();

			write32(oskDataCount);
			write32(oskDataAddr);
			if (oskData != null && oskDataAddr != 0)
			{
				oskData.write(mem, oskDataAddr);
			}
			write32(oskState);
			write32(errorCode);
		}

		public override int @sizeof()
		{
			return @base.totalSizeof();
		}

		public override string ToString()
		{
			return string.Format("desc='{0}', inText='{1}', outText='{2}', inputMode={3:D}, inputAttr=0x{4:X}, language={5:D}, hide={6:D}, inputAllowCharType=0x{7:X}, lines={8:D}, showInputText={9:D}, outTextLength={10:D}, outTextLimit={11:D}", oskData.desc, oskData.inText, oskData.outText, oskData.inputMode, oskData.inputAttr, oskData.language, oskData.hide, oskData.inputAllowCharType, oskData.lines, oskData.showInputText, oskData.outTextLength, oskData.outTextLimit);
		}
	}
}