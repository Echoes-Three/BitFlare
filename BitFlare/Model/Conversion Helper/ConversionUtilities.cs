using BitFlare.Logic;
using BitFlare.Model.Input_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConversionUtilities
{
    public static bool IsNegative { get; private set; }
    public static uint ToConvertInt { get; private set; }
    public static decimal ToConvertFloat { get; private set; }
    public static DefinedBits BitMagnitude { get; private set; }
    public static DefinedTypes InputType { get; set; } = DefinedTypes.Integer;

    public static void Initializers(string input)
    {
        (input, IsNegative) = input.StartsWith('-')
            ? (input.Replace("-",""), true)
            : (input, IsNegative) ;
        
        if (input.Contains(','))
            input = input.Replace(",","");
        
        if (input.Contains('.'))
            (ToConvertFloat, InputType) = (decimal.Parse(input), DefinedTypes.FloatingPoint);
        else
            (ToConvertInt, InputType) = (uint.Parse(input), DefinedTypes.Integer);
        
        BitMagnitude = ToConvertInt switch
        { 
            <= byte.MaxValue
                => DefinedBits.EightBit,
            <= ushort.MaxValue
                => DefinedBits.SixteenBit,
            > ushort.MaxValue
                => DefinedBits.ThirtyTwoBit
        };
    }
    
    internal static string FormatedOutput(string paddedResult)
    {
        // 0000 0000 0000 0000
        var result = "";

        for (var bit = 0; bit <= paddedResult.Length - 1 ; bit++)
        {
            if (bit % 4 == 0)
                result += " ";
            
            result += paddedResult[bit];
        }
        
        return result;
    }
}
