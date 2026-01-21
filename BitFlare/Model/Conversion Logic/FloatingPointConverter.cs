using System.Globalization;
using System.Windows.Media.Animation;
using BitFlare.Model.Conversion_Helper;

namespace BitFlare.Logic;

public static class FloatingPointConverter
{
    private static bool ReadyToCount { get; set; }
    private static bool IsFull { get; set; }
    private static bool IsFinite{ get; set; }
    private static int MantissaBitCounter { get; set; }
    private static string Converted { get; set; } = string.Empty;
    private static string RoundingBits { get; set; } = "";

    public static string BasicConverter(decimal input)
    {
        while (!IsFinite && !IsFull)
        {
            if (MantissaBitCounter < 24)
            {
                input *= 2;
                            
                switch (decimal.Truncate(input))
                {
                    case 1:
                        Converted += "1";
                        input -= decimal.Truncate(input);
                        ReadyToCount = true;
                        break;
                    case 0:
                        Converted += "0";
                        break;
                }
            }
            else
            {
                if (RoundingBits.Length != 2)
                { 
                    switch (decimal.Truncate(input))
                       {
                           case 1:
                               RoundingBits += "1";
                               input -= decimal.Truncate(input);
                               ReadyToCount = true;
                               break;
                           case 0:
                               RoundingBits += "0";
                               break;
                       } 
                }
                else
                {
                    var stickyBit = "";
                    
                    switch (decimal.Truncate(input))
                    {
                        case 1:
                            stickyBit += "1";
                            input -= decimal.Truncate(input);
                            ReadyToCount = true;
                            break;
                        case 0:
                            stickyBit += "0";
                            break;
                    }

                    (RoundingBits, IsFull) = stickyBit.Contains('1')
                        ? (RoundingBits + "1", true)
                        : (RoundingBits, false); 
                }
            }
            
            MantissaBitCounter += ReadyToCount ? 1 : 0;
            
            switch (input == 0,  RoundingBits.Length < 3)
            {
                case (true, _):
                    IsFinite = true;
                    break;
                case (_, true):
                    IsFull = true;
                    break;
            }
        }

        return Converted;
    }

    public static string Ieee754(string binary)
    {
        var sign = ConversionUtilities.ToConvertFloat < 0 ? "1" : "0";
        string exponent;
        string mantissa;

        if (binary.Contains('.'))
        {
            exponent = IntegerConverter.PaddedBasicConverter((uint)(binary[1..binary.IndexOf('.')].Length + 127));
            binary = binary.Replace(".", "");
            mantissa = binary[1..];
            mantissa += string.Concat(Enumerable.Repeat("0", 23 - mantissa.Length));
        }
        else
        {
            for (var bit = 0;; bit++)
            {
                if (binary[bit] != '1') continue;
                exponent = IntegerConverter.PaddedBasicConverter(127 - (uint)(bit + 1));
                mantissa = binary[(bit + 1)..];
                mantissa += string.Concat(Enumerable.Repeat("0", 23 - mantissa.Length));
                break;
            }
        }
        
        // Add rounding mechanics 
        
        return $"{sign} {exponent} {mantissa}";
    }
}