using BitFlare.Logic;
using BitFlare.Model.Conversion_Helper;

namespace BitFlare.Model.Conversion_Logic;

public class FloatingPointConverter
{
    private bool ReadyToCount { get; set; }
    private bool IsFull { get; set; }
    private bool IsFinite { get; set; }
    private int MantissaBitCounter { get; set; }
    private string Converted { get; set; } = string.Empty;
    private string RoundingBits { get; set; } = "";
    private string StickyBits { get; set; } = "";

    public string NonZeroBasicConverter(decimal input)
    {
        while (!IsFinite && !IsFull)
        {
            if (Converted.Length < 23)
            {
                var result = ConvertingMechanism(input);
                input = result.Item1;
                Converted += result.Item2;
            }
            else
            {
                if (RoundingBits.Length != 2)
                {
                    var result = ConvertingMechanism(input);
                    input = result.Item1;
                    RoundingBits += result.Item2;
                }
                else
                {
                    var result = ConvertingMechanism(input);
                    input = result.Item1;
                    StickyBits += result.Item2;

                    (RoundingBits, IsFull) =
                        StickyBits.Contains('1') ? (RoundingBits + "1", true) : (RoundingBits, false);
                }
            }

            switch (input == 0, RoundingBits.Length == 3)
            {
                case (true, _):
                    IsFinite = true;
                    break;
                case (_, true):
                    IsFull = true;
                    break;
            }
        }

        RoundingBits += IsFinite && RoundingBits.Length == 2 ? "0" : "";
        return Converted;
        
    }

    public string BasicConverter(decimal input)
    {
        while (!IsFinite && !IsFull)
        {
            if (MantissaBitCounter < 24)
            {
                var result = ConvertingMechanism(input);
                input = result.Item1;
                Converted += result.Item2;
            }
            else
            {
                if (RoundingBits.Length != 2)
                {
                    var result = ConvertingMechanism(input);
                    input = result.Item1;
                    RoundingBits += result.Item2;
                }
                else
                {
                    var result = ConvertingMechanism(input);
                    input = result.Item1;
                    StickyBits += result.Item2;

                    (RoundingBits, IsFull) =
                        StickyBits.Contains('1') ? (RoundingBits + "1", true) : (RoundingBits, false);
                }
            }

            MantissaBitCounter += ReadyToCount ? 1 : 0;
            switch (input == 0, RoundingBits.Length == 3)
            {
                case (true, _):
                    IsFinite = true;
                    break;
                case (_, true):
                    IsFull = true;
                    break;
            }
        }

        RoundingBits += IsFinite && RoundingBits.Length == 2 ? "0" : "";
        return Converted;
    }

    public string Ieee754(string binary)
    {
        var sign = ConversionUtilities.ToConvertFloat < 0 ? "1" : "0";
        string exponent;
        string mantissa;

        if (binary.Contains('.'))
        {
            exponent = IntegerConverter.PaddedBasicConverter((uint)(binary[1..binary.IndexOf('.')].Length + 127));
            binary = binary.Replace(".", "");
            mantissa = binary[1..24];
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

        if (RoundingBits.Length != 3) return $"{sign} {exponent} {mantissa}";

        var (g, r, s) = (RoundingBits[0], RoundingBits[1], RoundingBits[2]);

        if (g == '0')
        {
        }
        else if (r == '1' || s == '1' || mantissa[23] == '1')
            mantissa = RoundingMechanism(mantissa);

        return $"{sign} {exponent} {mantissa}";
    }

    private static string RoundingMechanism(string mantissa)
    {
        var rounded = mantissa.ToCharArray();

        for (var bit = 1;; bit++)
        {
            if (rounded[^bit] == '0')
            {
                rounded[^bit] = '1';
                break;
            }

            rounded[^bit] = '0';
        }

        return string.Join("", rounded);
    }

    private (decimal, string) ConvertingMechanism(decimal toConvert)
    {
        var toStore = "";

        toConvert *= 2;
        switch (decimal.Truncate(toConvert))
        {
            case 1:
                toStore += "1";
                toConvert -= decimal.Truncate(toConvert);
                ReadyToCount = true;
                break;
            case 0:
                toStore += "0";
                break;
        }

        return (toConvert, toStore);
    }
}