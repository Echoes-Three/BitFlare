using BitFlare.Logic;
using BitFlare.Logic.Input_Logic;
using BitFlare.Model.Input_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConversionUtilities
{
    public static bool IsNegative { get; private set; }
    public static ulong ReadyToConvert { get; private set; }
    public static BitDefinition BitMagnitude { get; private set; }
    public static TypeDefinition InputType { get; private set; } = TypeDefinition.Integer;

    public static void Initializers(string? input)
    {
        IsNegative = input.StartsWith('-');
        
        if (InputTypeDefinition.Current == TypeDefinition.InvalidType)
            ReadyToConvert = 0;
        if (IsNegative)
            input = input.Replace("-","");
        if (input.Contains(','))
            input = input.Replace(",","");
        
        ReadyToConvert = ulong.Parse(input);
        
        BitMagnitude = ReadyToConvert switch
        { 
            <= byte.MaxValue
                => BitDefinition.EightBit,
            <= ushort.MaxValue
                => BitDefinition.SixteenBit,
            > ushort.MaxValue
                => BitDefinition.ThirtyTwoBit
        };
    }
    
    internal static string FormatedOutput(int[] paddedResult)
    {
        var formatted = string.Join("", paddedResult);

        switch (InputType)
        {
            case TypeDefinition.Integer:
            case TypeDefinition.FloatingPoint:
                // 0000 0000 0000 0000
                var spaceLimiter = BitMagnitude switch
                {
                    BitDefinition.EightBit => 0,
                    BitDefinition.SixteenBit => 2,
                    BitDefinition.ThirtyTwoBit => 6
                };
        
                for (var i = (bitSpaces: 4, accounter: 0);
                     i.accounter <= spaceLimiter;
                     i.bitSpaces += 4, i.accounter++)
                {
                    formatted = formatted.Insert(i.bitSpaces + i.accounter, " ");
                }
                break;
            case TypeDefinition.ENotation:
                // 0 00000000 00000000000000000000000
                break;
        }
        return formatted;
    }
}
