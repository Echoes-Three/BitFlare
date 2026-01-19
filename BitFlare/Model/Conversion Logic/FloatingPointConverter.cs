using System.Globalization;
using BitFlare.Model.Conversion_Helper;

namespace BitFlare.Logic;

public static class FloatingPointConverter
{
    private static bool ReadyToCount { get; set; }
    private static bool IsFull { get; set; } = false;
    private static bool IsFinite{ get; set; } = false;
    private static int MantissaBitCounter { get; set; }
    private static string Converted { get; set; } = string.Empty;

    public static string BasicConverter(double input)
    {
        while (!IsFinite && !IsFull)
        {
            input *= 2;
            
            switch (double.Truncate(input))
            {
                case 1:
                    Converted += "1";
                    input -= double.Truncate(input);
                    ReadyToCount = true;
                    break;
                case 0:
                    Converted += "0";
                    break;
            }

            MantissaBitCounter += ReadyToCount ? 1 : 0;
            
            switch (input == 0, MantissaBitCounter == 27)
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

        var result = "";

        if (binary.IndexOf('.') > 1)
        {
            exponent = IntegerConverter.BasicConverter((uint)(binary[1..binary.IndexOf('.')].Length + 127));
            binary = binary.Replace(".", "");
            mantissa = binary[1..];
            mantissa = mantissa + string.Concat(Enumerable.Repeat("0", 23 - mantissa.Length));

            result = $"{sign} {exponent} {mantissa}";
        }
        return result;
    }
}