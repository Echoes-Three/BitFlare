using BitFlare.Logic;
using BitFlare.Logic.Input_Logic;
using BitFlare.Model.Input_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConversionUtilities
{
    public static bool IsNegative { get; private set; }
    public static ulong ReadyToConvert { get; private set; }
    public static DefinedBits BitMagnitude { get; private set; }
    public static DefinedTypes InputType { get; private set; } = DefinedTypes.Integer;

    public static void Initializers(string input)
    {
        if (input.StartsWith('-'))
        {
            IsNegative = true;
            input = input.Replace("-","");
        }
        if (input.Contains(','))
            input = input.Replace(",","");
        //add the 
        ReadyToConvert = ulong.Parse(input);
        
        BitMagnitude = ReadyToConvert switch
        { 
            <= byte.MaxValue
                => DefinedBits.EightBit,
            <= ushort.MaxValue
                => DefinedBits.SixteenBit,
            > ushort.MaxValue
                => DefinedBits.ThirtyTwoBit
        };
    }
    
    internal static string FormatedOutput(int[] paddedResult)
    {
        var formatted = string.Join("", paddedResult);

        switch (InputType)
        {
            case DefinedTypes.Integer:
            case DefinedTypes.FloatingPoint:
                // 0000 0000 0000 0000
                var spaceLimiter = BitMagnitude switch
                {
                    DefinedBits.EightBit => 0,
                    DefinedBits.SixteenBit => 2,
                    DefinedBits.ThirtyTwoBit => 6
                };
        
                for (var i = (bitSpaces: 4, accounter: 0);
                     i.accounter <= spaceLimiter;
                     i.bitSpaces += 4, i.accounter++)
                {
                    formatted = formatted.Insert(i.bitSpaces + i.accounter, " ");
                }
                break;
            case DefinedTypes.ENotation:
                // 0 00000000 00000000000000000000000
                break;
        }
        return formatted;
    }
}
