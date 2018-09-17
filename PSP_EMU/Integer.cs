//---------------------------------------------------------------------------------------------------------
//	Copyright © 2018 The Darkprogramer aka (Eon Van Wyk) @thedarkprogr on twitter (xDPx)
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to replace some calls to java.Integer methods with the C# equivalent.
//---------------------------------------------------------------------------------------------------------

internal class Integer
{
    public static int numberOfLeadingZeros(int value)
    {
        // Shift right unsigned to work with both positive and negative values
        var uValue = (uint)value;
        int leadingZeros = 0;
        while (uValue != 0)
        {
            uValue = uValue >> 1;
            leadingZeros++;
        }

        return (32 - leadingZeros);
    }

    public static int parseInt(string s,int rint)
    {
        int @return;
        int.TryParse(s, out @return);
        return @return;
    }
}