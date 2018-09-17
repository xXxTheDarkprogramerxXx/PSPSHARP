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
namespace pspsharp.filesystems.umdiso.iso9660
{

	/// 
	/// <summary>
	/// @author gigaherz
	/// </summary>
	public class Iso9660File
	{

		private int fileLBA;
		private long fileSize;
		private int fileProperties;
		// padding: byte[3]
		private string fileName; //[128+1];
		//Iso9660Date date; // byte[7]
		private DateTime timestamp;
		private static readonly Charset jolietCharset = Charset.forName("UTF-16BE");
		private static readonly Charset normalCharset = Charset.forName("US-ASCII");

		private static int Ubyte(sbyte b)
		{
			return b & 0xFF;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public Iso9660File(byte[] data, int Length, bool jolietExtension) throws java.io.IOException
		public Iso9660File(sbyte[] data, int Length, bool jolietExtension)
		{
			/*
			   1           Length of Directory Record (LEN-DR) -- read by the Iso9660Directory
			   2           Extended Attribute Record Length
			   3 to 10     Location of Extent
			   11 to 18    Data Length
			   19 to 25    Recording Date and Time
			   26          File Flags
			   27          File Unit Size
			   28          Interleave Gap Size
			   29 to 32    Volume Sequence Number
			   33          Length of File Identifier (LEN_FI)
			   34 to (33+LEN_FI)   File Identifier
			   (34 + LEN_FI)   Padding FieldInfo
			 */

			fileLBA = Ubyte(data[1]) | (Ubyte(data[2]) << 8) | (Ubyte(data[3]) << 16) | (data[4] << 24);
			fileSize = Ubyte(data[9]) | (Ubyte(data[10]) << 8) | (Ubyte(data[11]) << 16) | (((long) Ubyte(data[12])) << 24);
			int year = Ubyte(data[17]);
			int month = Ubyte(data[18]);
			int day = Ubyte(data[19]);
			int hour = Ubyte(data[20]);
			int minute = Ubyte(data[21]);
			int second = Ubyte(data[22]);
			int gmtOffset = data[23]; // Offset from Greenwich Mean Time in number of 15 min intervals from -48 (West) to + 52 (East)

			int gmtOffsetHours = gmtOffset / 4;
			int gmtOffsetMinutes = (gmtOffset % 4) * 15;
			// Build TimeZone name as e.g.
			//   "GMT+1015", meaning GMT + 10 hours and 15 minutes
			string timeZoneName = "GMT";
			if (gmtOffset >= 0)
			{
				timeZoneName += "+";
			}
			timeZoneName += gmtOffsetHours;
			if (gmtOffsetMinutes > 0)
			{
				if (gmtOffsetMinutes < 10)
				{
					timeZoneName += "0";
				}
				timeZoneName += gmtOffsetMinutes;
			}
			TimeZone timeZone = TimeZone.getTimeZone(timeZoneName);

			DateTime timestampCalendar = DateTime.getInstance(timeZone);
			timestampCalendar = new DateTime(1900 + year, month - 1, day, hour, minute, second);
			timestamp = timestampCalendar;

			fileProperties = data[24];

			if (fileLBA < 0 || fileSize < 0)
			{
				throw new IOException("Invalid ISO, size or lba < 0");
			}

			int fileNameLength = data[31];

			if (jolietExtension)
			{
				if (fileNameLength == 1)
				{
					char c = (char) data[32];
					if (c == (char)0)
					{
						c = '.';
					}
					fileName = Convert.ToString(c);
				}
				else
				{
					fileName = StringHelper.NewString(data, 32, fileNameLength, jolietCharset);
					int end = fileName.IndexOf(';');
					if (end >= 0)
					{
						fileName = fileName.Substring(0, end);
					}
				}
			}
			else
			{
				// Replace all '\0' bytes by '.' in the filename
				for (int i = 0; i < fileNameLength; i++)
				{
					if (data[32 + i] == (sbyte) 0)
					{
						data[32 + i] = (sbyte) '.';
					}
				}
				fileName = StringHelper.NewString(data, 32, fileNameLength, normalCharset);
			}
		}

		public virtual int LBA
		{
			get
			{
				return fileLBA;
			}
		}

		public virtual long Size
		{
			get
			{
				return fileSize;
			}
		}

		public virtual int Properties
		{
			get
			{
				return fileProperties;
			}
		}

		public virtual string FileName
		{
			get
			{
				return fileName;
			}
		}

		public virtual DateTime Timestamp
		{
			get
			{
				return timestamp;
			}
		}

		public override string ToString()
		{
			return fileName;
		}
	}
}