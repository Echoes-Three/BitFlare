
using System.Globalization;
namespace BitFlare.Model.Conversion_Logic;

public static class IeeeConverter
{
    public static (string,string) Ieee754(string input)
    {
        if (!float.TryParse(
                input,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var value))
        {
            return ("invalid scientific notation", "");
        }
        var bits = BitConverter.SingleToInt32Bits(value);
        
        var sign = ((bits >> 31) & 1).ToString();
        var exponent = Convert.ToString((bits >> 23) & 0xFF, 2).PadLeft(8, '0');
        var mantissa = Convert.ToString(bits & 0x7FFFFF, 2).PadLeft(23, '0');
        
        return ($"{sign} {exponent} {mantissa}", bits.ToString("X"));
    }
}