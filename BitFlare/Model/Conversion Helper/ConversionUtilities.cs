using BitFlare.Model.Conversion_Logic;
using BitFlare.Model.Input_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConversionUtilities
{
    public static bool IsNegative { get; private set; }
    public static uint ToConvertInt { get; private set; }
    public static string ToConvertIeee { get; private set; }
    public static DefinedBits BitMagnitude { get; private set; }
    public static DefinedTypes InputType { get; private set; } = DefinedTypes.Integer;

    public static void Initializers(string input)
    {
        // Initializes flag and remove sign
        (input, IsNegative) = input.StartsWith('-')
            ? (input.Replace("-",""), true)
            : (input, IsNegative) ;
        
        // Removes commas
        if (input.Contains(','))
            input = input.Replace(",","");
        
        // Initializes input for conversion
        if (TypeClassification.ClassifiedType == DefinedTypes.Integer)
        {
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
        else
            (ToConvertIeee, InputType) = (input, DefinedTypes.ENotation);
        
    }
    
    internal static string FormatedOutput(string paddedResult)
    {
        // Formats into pattern: 0000 0000 0000 0000
        var result = "";
        
        for (var bit = 0; bit <= paddedResult.Length - 1 ; bit++)
        {
            if (bit % 4 == 0) result += " ";
            
            result += paddedResult[bit];
        }

        if (result[0] == ' ')
            result = result.Remove(0,1);
        
        return result;
    }
}
