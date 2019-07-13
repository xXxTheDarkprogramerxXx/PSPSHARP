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
namespace pspsharp.HLE.modules
{

	using TimeNanos = pspsharp.Clock.TimeNanos;
	using LengthInfo = pspsharp.HLE.BufferInfo.LengthInfo;
	using Usage = pspsharp.HLE.BufferInfo.Usage;
	using ScePspDateTime = pspsharp.HLE.kernel.types.ScePspDateTime;

	//using Logger = org.apache.log4j.Logger;

	public class sceRtc : HLEModule
	{
		//public static Logger log = Modules.getLogger("sceRtc");

		internal const int PSP_TIME_INVALID_YEAR = -1;
		internal const int PSP_TIME_INVALID_MONTH = -2;
		internal const int PSP_TIME_INVALID_DAY = -3;
		internal const int PSP_TIME_INVALID_HOUR = -4;
		internal const int PSP_TIME_INVALID_MINUTES = -5;
		internal const int PSP_TIME_INVALID_SECONDS = -6;
		internal const int PSP_TIME_INVALID_MICROSECONDS = -7;

		// Statics verified on PSP.
		internal const int PSP_TIME_SECONDS_IN_MINUTE = 60;
		internal const int PSP_TIME_SECONDS_IN_HOUR = 3600;
		internal const int PSP_TIME_SECONDS_IN_DAY = 86400;
		internal const int PSP_TIME_SECONDS_IN_WEEK = 604800;
		internal const int PSP_TIME_SECONDS_IN_MONTH = 2629743;
		internal const int PSP_TIME_SECONDS_IN_YEAR = 31556926;

		// Number of milliseconds between 1900-01-01 (reference date on a PSP)
		// and 1970-01-01 (reference date on Java)
		private long rtcMagicOffset = 62135596800000000L;
		protected internal static SimpleDateFormat rfc3339 = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ssZ");

		public virtual long hleGetCurrentTick()
		{
			TimeNanos timeNanos = Emulator.Clock.currentTimeNanos();
			return (timeNanos.micros + timeNanos.millis * 1000) + timeNanos.seconds * 1000000L + rtcMagicOffset;
		}

		/// <summary>
		/// 64 bit addend </summary>
		protected internal virtual int hleRtcTickAdd64(TPointer64 dstPtr, TPointer64 srcPtr, long value, long multiplier)
		{
			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("hleRtcTickAdd64 dstPtr={0}, srcPtr={1}({2:D}), {3:D} * {4:D}", dstPtr, srcPtr, srcPtr.Value, value, multiplier));
			}

			long src = srcPtr.Value;
			dstPtr.Value = src + multiplier * value;

			return 0;
		}

		/// <summary>
		/// 32 bit addend </summary>
		protected internal virtual int hleRtcTickAdd32(TPointer64 dstPtr, TPointer64 srcPtr, int value, long multiplier)
		{
			System.Console.WriteLine("hleRtcTickAdd32 " + multiplier + " * " + value);

			long src = srcPtr.Value;
			dstPtr.Value = src + multiplier * value;

			return 0;
		}

		protected internal virtual DateTime getDateFromTick(long tick)
		{
			return new DateTime((tick - rtcMagicOffset) / 1000L);
		}

		protected internal virtual string formatRFC3339(DateTime date)
		{
			string result = rfc3339.format(date);
			// SimpleDateFormat outputs the timezone offset in the format "hhmm"
			// instead of "hh:mm" as required by RFC3339.
			result = result.replaceFirst("(\\d\\d)(\\d\\d)$", "$1:$2");

			return result;
		}

		protected internal virtual TimeZone LocalTimeZone
		{
			get
			{
				return TimeZone.Default;
			}
		}

		/// <summary>
		/// Obtains the Tick Resolution.
		/// </summary>
		/// <param name="processor">
		/// </param>
		/// <returns> The Tick Resolution in microseconds. </returns>
		[HLEFunction(nid : 0xC41C2853, version : 150)]
		public virtual int sceRtcGetTickResolution()
		{
			return 1000000;
		}

		[HLEFunction(nid : 0x3F7AD767, version : 150)]
		public virtual int sceRtcGetCurrentTick(TPointer64 currentTick)
		{
			currentTick.Value = hleGetCurrentTick();

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetCurrentTick returning {0:D}", currentTick.Value));
			}

			return 0;
		}

		[HLEFunction(nid : 0x011F03C1, version : 150)]
		public virtual long sceRtcGetAccumulativeTime()
		{
			// Returns the difference between the last reincarnated time and the current tick.
			// Just return our current tick, since there's no need to mimick such behaviour.
			return hleGetCurrentTick();
		}

		[HLEFunction(nid : 0x029CA3B3, version : 150)]
		public virtual long sceRtcGetAccumlativeTime()
		{
			// Typo. Same as sceRtcGetAccumulativeTime.
			return hleGetCurrentTick();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0x4CFA57B0, version = 150) public int sceRtcGetCurrentClock(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=16, usage=pspsharp.HLE.BufferInfo.Usage.out) pspsharp.HLE.TPointer addr, int tz)
		[HLEFunction(nid : 0x4CFA57B0, version : 150)]
		public virtual int sceRtcGetCurrentClock(TPointer addr, int tz)
		{
			ScePspDateTime pspTime = new ScePspDateTime(tz);
			pspTime.write(addr);

			return 0;
		}

		[HLEFunction(nid : 0xE7C27D1B, version : 150)]
		public virtual int sceRtcGetCurrentClockLocalTime(TPointer addr)
		{
			ScePspDateTime pspTime = new ScePspDateTime();
			pspTime.write(addr);

			return 0;
		}

		[HLEFunction(nid : 0x9012B140, version : 660)]
		public virtual int sceRtcGetCurrentClockLocalTime_660(TPointer addr)
		{
			return sceRtcGetCurrentClockLocalTime(addr);
		}

		[HLEFunction(nid : 0x34885E0D, version : 150)]
		public virtual int sceRtcConvertUtcToLocalTime(TPointer64 utcPtr, TPointer64 localPtr)
		{
			// Add the offset of the local time zone to UTC
			TimeZone localTimeZone = LocalTimeZone;
			hleRtcTickAdd64(localPtr, utcPtr, localTimeZone.RawOffset, 1000L);

			return 0;
		}

		[HLEFunction(nid : 0x779242A2, version : 150)]
		public virtual int sceRtcConvertLocalTimeToUTC(TPointer64 localPtr, TPointer64 utcPtr)
		{
			// Subtract the offset of the local time zone to UTC
			TimeZone localTimeZone = LocalTimeZone;
			hleRtcTickAdd64(utcPtr, localPtr, -localTimeZone.RawOffset, 1000L);

			return 0;
		}

		[HLEFunction(nid : 0x42307A17, version : 150)]
		public virtual bool sceRtcIsLeapYear(int year)
		{
			return (year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0);
		}

		[HLEFunction(nid : 0x05EF322C, version : 150)]
		public virtual int sceRtcGetDaysInMonth(int year, int month)
		{
			DateTime cal = new GregorianCalendar(year, month - 1, 1);
			int days = cal.getActualMaximum(DateTime.DAY_OF_MONTH);

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetDaysInMonth returning {0:D}", days));
			}

			return days;
		}

		/// <summary>
		/// Returns the day of the week.
		/// 0 = sunday, 1 = monday, 2 = tuesday, 3 = wednesday, 4 = thursday, 5 = friday, 6 = saturnday
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="month"> </param>
		/// <param name="day">
		/// </param>
		/// <returns> The day of the week. </returns>
		[HLEFunction(nid : 0x57726BC1, version : 150)]
		public virtual int sceRtcGetDayOfWeek(int year, int month, int day)
		{
			DateTime cal = new DateTime();
			cal = new DateTime(year, month - 1, day);

			int dayOfWeekNumber = cal.DayOfWeek;
			dayOfWeekNumber = (dayOfWeekNumber - 1 + 7) % 7;

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetDayOfWeek returning {0:D}", dayOfWeekNumber));
			}

			return dayOfWeekNumber;
		}

		/// <summary>
		/// Validate pspDate component ranges
		/// </summary>
		/// <param name="date"> - pointer to pspDate struct to be checked </param>
		/// <returns> 0 on success, one of PSP_TIME_INVALID_* on error </returns>
		[HLEFunction(nid : 0x4B1B5E82, version : 150)]
		public virtual int sceRtcCheckValid(ScePspDateTime time)
		{
			DateTime cal = new GregorianCalendar(time.year, time.month - 1, time.day, time.hour, time.minute, time.second);

			int result = 0;

			if (time.year < 1582 || time.year > 3000)
			{ // What are valid years?
				result = PSP_TIME_INVALID_YEAR;
			}
			else if (time.month < 1 || time.month > 12)
			{
				result = PSP_TIME_INVALID_MONTH;
			}
			else if (time.day < 1 || time.day > 31)
			{
				result = PSP_TIME_INVALID_DAY;
			}
			else if (time.hour < 0 || time.hour > 23)
			{
				result = PSP_TIME_INVALID_HOUR;
			}
			else if (time.minute < 0 || time.minute > 59)
			{
				result = PSP_TIME_INVALID_MINUTES;
			}
			else if (time.second < 0 || time.second > 59)
			{
				result = PSP_TIME_INVALID_SECONDS;
			}
			else if (time.microsecond < 0 || time.microsecond >= 1000000)
			{
				result = PSP_TIME_INVALID_MICROSECONDS;
			}
			else if (cal.Day != time.day)
			{ // Check if this is a valid day of the month
				result = PSP_TIME_INVALID_DAY;
			}

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcCheckValid time={0}, cal={1}, returning 0x{2:X8}", time, cal, result));
			}

			return result;
		}

		[HLEFunction(nid : 0x3A807CC8, version : 150)]
		public virtual int sceRtcSetTime_t(TPointer dateAddr, int time)
		{
			ScePspDateTime dateTime = ScePspDateTime.fromUnixTime(time);
			dateTime.write(dateAddr);

			return 0;
		}

		[HLEFunction(nid : 0x27C4594C, version : 150)]
		public virtual int sceRtcGetTime_t(ScePspDateTime dateTime, TPointer32 timeAddr)
		{
			DateTime cal = new DateTime();
			cal = new DateTime(dateTime.year, dateTime.month - 1, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
			int unixtime = (int)(cal.Ticks / 1000L);

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetTime_t returning {0:D}", unixtime));
			}

			timeAddr.setValue(unixtime);

			return 0;
		}

		[HLEFunction(nid : 0xF006F264, version : 150)]
		public virtual int sceRtcSetDosTime(TPointer dateAddr, int time)
		{
			ScePspDateTime dateTime = ScePspDateTime.fromMSDOSTime(time);
			dateTime.write(dateAddr);

			return 0;
		}

		[HLEFunction(nid : 0x74772CCC, version : 660)]
		public virtual int sceRtcSetDosTime_660(TPointer dateAddr, int time)
		{
			return sceRtcSetDosTime(dateAddr, time);
		}

		[HLEFunction(nid : 0x36075567, version : 150)]
		public virtual int sceRtcGetDosTime(ScePspDateTime dateTime, TPointer32 timeAddr)
		{
			DateTime cal = new DateTime();
			cal = new DateTime(dateTime.year, dateTime.month - 1, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
			int dostime = (int)(cal.Ticks / 1000L);

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetDosTime returning {0:D}", dostime));
			}

			timeAddr.setValue(dostime);

			return 0;
		}

		[HLEFunction(nid : 0xA4A5BF1B, version : 660)]
		public virtual int sceRtcGetDosTime_660(ScePspDateTime dateTime, TPointer32 timeAddr)
		{
			return sceRtcGetDosTime(dateTime, timeAddr);
		}

		[HLEFunction(nid : 0x7ACE4C04, version : 150)]
		public virtual int sceRtcSetWin32FileTime(TPointer dateAddr, long time)
		{
			ScePspDateTime dateTime = ScePspDateTime.fromFILETIMETime(time);
			dateTime.write(dateAddr);

			return 0;
		}

		[HLEFunction(nid : 0xCF561893, version : 150)]
		public virtual int sceRtcGetWin32FileTime(ScePspDateTime dateTime, TPointer64 timeAddr)
		{
			DateTime cal = new DateTime();
			cal = new DateTime(dateTime.year, dateTime.month - 1, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
			int filetimetime = (int)(cal.Ticks / 1000L);

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetWin32FileTime returning {0:D}", filetimetime));
			}

			timeAddr.Value = filetimetime;

			return 0;
		}

		/// <summary>
		/// Set a pspTime struct based on ticks. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEFunction(nid = 0x7ED29E40, version = 150) public int sceRtcSetTick(@BufferInfo(lengthInfo=pspsharp.HLE.BufferInfo.LengthInfo.fixedLength, Length=16, usage=pspsharp.HLE.BufferInfo.Usage.out) pspsharp.HLE.TPointer timeAddr, @BufferInfo(usage=pspsharp.HLE.BufferInfo.Usage.in) pspsharp.HLE.TPointer64 ticksAddr)
		[HLEFunction(nid : 0x7ED29E40, version : 150)]
		public virtual int sceRtcSetTick(TPointer timeAddr, TPointer64 ticksAddr)
		{
			long ticks = ticksAddr.Value - rtcMagicOffset;
			ScePspDateTime time = ScePspDateTime.fromMicros(ticks);
			time.write(timeAddr);

			return 0;
		}

		/// <summary>
		/// Set ticks based on a pspTime struct. </summary>
		[HLEFunction(nid : 0x6FF40ACC, version : 150)]
		public virtual int sceRtcGetTick(ScePspDateTime time, TPointer64 ticksAddr)
		{
			// use java library to convert a date to seconds, then multiply it by the tick resolution
			DateTime cal = new GregorianCalendar(time.year, time.month - 1, time.day, time.hour, time.minute, time.second);
			cal.set(DateTime.MILLISECOND, time.microsecond / 1000);
			cal.TimeZone = ScePspDateTime.GMT;
			long ticks = rtcMagicOffset + (cal.Ticks * 1000L) + (time.microsecond % 1000);
			ticksAddr.Value = ticks;

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetTick returning {0:D}", ticks));
			}

			return 0;
		}

		[HLEFunction(nid : 0x9ED0AE87, version : 150)]
		public virtual int sceRtcCompareTick(TPointer64 firstPtr, TPointer64 secondPtr)
		{
			long tick1 = firstPtr.Value;
			long tick2 = secondPtr.Value;

			if (tick1 < tick2)
			{
				return -1;
			}
			if (tick1 > tick2)
			{
				return 1;
			}
			return 0;
		}

		[HLEFunction(nid : 0x44F45E05, version : 150)]
		public virtual int sceRtcTickAddTicks(TPointer64 dstPtr, TPointer64 srcPtr, long value)
		{
			System.Console.WriteLine("sceRtcTickAddTicks redirecting to hleRtcTickAdd64(1)");
			return hleRtcTickAdd64(dstPtr, srcPtr, value, 1);
		}

		[HLEFunction(nid : 0x26D25A5D, version : 150)]
		public virtual int sceRtcTickAddMicroseconds(TPointer64 dstPtr, TPointer64 srcPtr, long value)
		{
			System.Console.WriteLine("sceRtcTickAddMicroseconds redirecting to hleRtcTickAdd64(1)");
			return hleRtcTickAdd64(dstPtr, srcPtr, value, 1);
		}

		[HLEFunction(nid : 0xF2A4AFE5, version : 150)]
		public virtual int sceRtcTickAddSeconds(TPointer64 dstPtr, TPointer64 srcPtr, long value)
		{
			System.Console.WriteLine("sceRtcTickAddSeconds redirecting to hleRtcTickAdd64(1000000)");
			return hleRtcTickAdd64(dstPtr, srcPtr, value, 1000000L);
		}

		[HLEFunction(nid : 0xE6605BCA, version : 150)]
		public virtual int sceRtcTickAddMinutes(TPointer64 dstPtr, TPointer64 srcPtr, long value)
		{
			System.Console.WriteLine("sceRtcTickAddMinutes redirecting to hleRtcTickAdd64(60*1000000)");
			return hleRtcTickAdd64(dstPtr, srcPtr, value, PSP_TIME_SECONDS_IN_MINUTE * 1000000L);
		}

		[HLEFunction(nid : 0x26D7A24A, version : 150)]
		public virtual int sceRtcTickAddHours(TPointer64 dstPtr, TPointer64 srcPtr, int value)
		{
			System.Console.WriteLine("sceRtcTickAddHours redirecting to hleRtcTickAdd32(60*60*1000000)");
			return hleRtcTickAdd32(dstPtr, srcPtr, value, PSP_TIME_SECONDS_IN_HOUR * 1000000L);
		}

		[HLEFunction(nid : 0xE51B4B7A, version : 150)]
		public virtual int sceRtcTickAddDays(TPointer64 dstPtr, TPointer64 srcPtr, int value)
		{
			System.Console.WriteLine("sceRtcTickAddDays redirecting to hleRtcTickAdd32(24*60*60*1000000)");
			return hleRtcTickAdd32(dstPtr, srcPtr, value, PSP_TIME_SECONDS_IN_DAY * 1000000L);
		}

		[HLEFunction(nid : 0xCF3A2CA8, version : 150)]
		public virtual int sceRtcTickAddWeeks(TPointer64 dstPtr, TPointer64 srcPtr, int value)
		{
			System.Console.WriteLine("sceRtcTickAddWeeks redirecting to hleRtcTickAdd32(7*24*60*60*1000000)");
			return hleRtcTickAdd32(dstPtr, srcPtr, value, PSP_TIME_SECONDS_IN_WEEK * 1000000L);
		}

		[HLEFunction(nid : 0xDBF74F1B, version : 150)]
		public virtual int sceRtcTickAddMonths(TPointer64 dstPtr, TPointer64 srcPtr, int value)
		{
			System.Console.WriteLine("sceRtcTickAddMonths redirecting to hleRtcTickAdd32(30*24*60*60*1000000)");
			return hleRtcTickAdd32(dstPtr, srcPtr, value, PSP_TIME_SECONDS_IN_MONTH * 1000000L);
		}

		[HLEFunction(nid : 0x42842C77, version : 150)]
		public virtual int sceRtcTickAddYears(TPointer64 dstPtr, TPointer64 srcPtr, int value)
		{
			System.Console.WriteLine("sceRtcTickAddYears redirecting to hleRtcTickAdd32(365*24*60*60*1000000)");
			return hleRtcTickAdd32(dstPtr, srcPtr, value, PSP_TIME_SECONDS_IN_YEAR * 1000000L);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC663B3B9, version = 150) public int sceRtcFormatRFC2822()
		[HLEFunction(nid : 0xC663B3B9, version : 150)]
		public virtual int sceRtcFormatRFC2822()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7DE6711B, version = 150) public int sceRtcFormatRFC2822LocalTime()
		[HLEFunction(nid : 0x7DE6711B, version : 150)]
		public virtual int sceRtcFormatRFC2822LocalTime()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x0498FB3C, version = 150) public int sceRtcFormatRFC3339()
		[HLEFunction(nid : 0x0498FB3C, version : 150)]
		public virtual int sceRtcFormatRFC3339()
		{
			return 0;
		}

		[HLEFunction(nid : 0x27F98543, version : 150)]
		public virtual int sceRtcFormatRFC3339LocalTime(TPointer resultString, TPointer64 srcPtr)
		{
			DateTime date = getDateFromTick(srcPtr.Value);
			string result = formatRFC3339(date);

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcFormatRFC3339LocalTime src={0:D}, returning '{1}'", srcPtr.Value, result));
			}

			resultString.StringZ = result;

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xDFBC5F16, version = 150) public int sceRtcParseDateTime()
		[HLEFunction(nid : 0xDFBC5F16, version : 150)]
		public virtual int sceRtcParseDateTime()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x28E1E988, version = 150) public int sceRtcParseRFC3339()
		[HLEFunction(nid : 0x28E1E988, version : 150)]
		public virtual int sceRtcParseRFC3339()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7D1FBED3, version = 150) public int sceRtcSetAlarmTick(@CanBeNull pspsharp.HLE.TPointer64 srcPtr)
		[HLEFunction(nid : 0x7D1FBED3, version : 150)]
		public virtual int sceRtcSetAlarmTick(TPointer64 srcPtr)
		{
			if (log.DebugEnabled && srcPtr.NotNull)
			{
				System.Console.WriteLine(string.Format("sceRtcSetAlarmTick src=0x{0:X}", srcPtr.Value));
			}

			return 0;
		}

		[HLEFunction(nid : 0x203CEB0D, version : 200)]
		public virtual int sceRtcGetLastReincarnatedTime(TPointer64 tickAddr)
		{
			// Returns the last tick that was saved upon a battery shutdown.
			// Just return our current tick, since there's no need to mimick such behavior.
			tickAddr.Value = hleGetCurrentTick();

			return 0;
		}

		[HLEFunction(nid : 0x62685E98, version : 200)]
		public virtual int sceRtcGetLastAdjustedTime(TPointer64 tickAddr)
		{
			// Returns the last time that was manually set by the user.
			// Just return our current tick, since there's no need to mimick such behavior.
			tickAddr.Value = hleGetCurrentTick();

			return 0;
		}

		[HLEFunction(nid : 0x1909C99B, version : 200)]
		public virtual int sceRtcSetTime64_t(TPointer dateAddr, long time)
		{
			ScePspDateTime dateTime = ScePspDateTime.fromUnixTime(time);
			dateTime.write(dateAddr);

			return 0;
		}

		[HLEFunction(nid : 0xE1C93E47, version : 200)]
		public virtual int sceRtcGetTime64_t(ScePspDateTime dateTime, TPointer64 timeAddr)
		{
			DateTime cal = new DateTime();
			cal = new DateTime(dateTime.year, dateTime.month - 1, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
			long unixtime = cal.Ticks / 1000L;

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetTime64_t psptime={0} returning unixtime={1:D}", dateTime, unixtime));
			}

			timeAddr.Value = unixtime;

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFB3B18CD, version = 271) public int sceRtcRegisterCallback(int callbackId)
		[HLEFunction(nid : 0xFB3B18CD, version : 271)]
		public virtual int sceRtcRegisterCallback(int callbackId)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6A676D2D, version = 271) public int sceRtcUnregisterCallback(int callbackId)
		[HLEFunction(nid : 0x6A676D2D, version : 271)]
		public virtual int sceRtcUnregisterCallback(int callbackId)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xF5FCC995, version = 150) public int sceRtcGetCurrentNetworkTick(pspsharp.HLE.TPointer64 networkTick)
		[HLEFunction(nid : 0xF5FCC995, version : 150)]
		public virtual int sceRtcGetCurrentNetworkTick(TPointer64 networkTick)
		{
			networkTick.Value = hleGetCurrentTick();

			//if (log.DebugEnabled)
			{
				System.Console.WriteLine(string.Format("sceRtcGetCurrentNetworkTick returning {0:D}", networkTick.Value));
			}

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC2DDBEB5, version = 150) public int sceRtcGetAlarmTick(pspsharp.HLE.TPointer64 alarmTick)
		[HLEFunction(nid : 0xC2DDBEB5, version : 150)]
		public virtual int sceRtcGetAlarmTick(TPointer64 alarmTick)
		{
			alarmTick.Value = 0L;

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE09880CF, version = 660) public int sceRtcSetAlarmTick_660(@CanBeNull pspsharp.HLE.TPointer64 srcPtr)
		[HLEFunction(nid : 0xE09880CF, version : 660)]
		public virtual int sceRtcSetAlarmTick_660(TPointer64 srcPtr)
		{
			return sceRtcSetAlarmTick(srcPtr);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xCEEF238F, version = 150) public int sceRtcGetCurrentSecureTick(pspsharp.HLE.TPointer64 currentTick)
		[HLEFunction(nid : 0xCEEF238F, version : 150)]
		public virtual int sceRtcGetCurrentSecureTick(TPointer64 currentTick)
		{
			return sceRtcGetCurrentTick(currentTick);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x759937C5, version = 150) public int sceRtcSetConf(int unknown1, int unknown2, int unknown3, int unknown4)
		[HLEFunction(nid : 0x759937C5, version : 150)]
		public virtual int sceRtcSetConf(int unknown1, int unknown2, int unknown3, int unknown4)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xDFF30673, version = 660) public int sceRtcSetConf_660(int unknown1, int unknown2, int unknown3, int unknown4)
		[HLEFunction(nid : 0xDFF30673, version : 660)]
		public virtual int sceRtcSetConf_660(int unknown1, int unknown2, int unknown3, int unknown4)
		{
			return sceRtcSetConf(unknown1, unknown2, unknown3, unknown4);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x508BA64B, version = 150) public int sceRtc_508BA64B(@CanBeNull @BufferInfo(usage=pspsharp.HLE.BufferInfo.Usage.in) pspsharp.HLE.TPointer64 unknown)
		[HLEFunction(nid : 0x508BA64B, version : 150)]
		public virtual int sceRtc_508BA64B(TPointer64 unknown)
		{
			return 0;
		}

		[HLEFunction(nid : 0xE7B3ABF4, version : 660)]
		public virtual int sceRtcSetTick_660(TPointer timeAddr, TPointer64 ticksAddr)
		{
			return sceRtcSetTick(timeAddr, ticksAddr);
		}
	}
}