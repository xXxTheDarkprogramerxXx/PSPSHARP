//---------------------------------------------------------------------------------------------------------
//	Copyright © 2018 The Darkprogramer aka (Eon Van Wyk) @thedarkprogr on twitter (xDPx)
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to replace some calls to java.Double methods with the C# equivalent.
//---------------------------------------------------------------------------------------------------------

using System;

internal static class Long
{
    public static long parseLong(string s, int radix)
    {
        long @return;
        long.TryParse(s, out @return);
        return @return;
    }
}