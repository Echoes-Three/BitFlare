using BitFlare.Logic.Input_Logic;

namespace BitFlare.Logic.Conversion_Helper;

public static class ConversionUtilities
{
    internal static bool IsNegative { get; set; }
    internal static ulong ReadyToConvert {get; set; }
    internal static BitDefinition BitMagnitude {get; set;}
    internal static TypeDefinition InputType { get; set; } = TypeDefinition.Integer;

    internal static bool SignChecker(string inputBoxText) =>
         IsNegative = inputBoxText.StartsWith('-');
    
    internal static ulong InputParser(string toParse)
    {
        if (InputTypeDefinition.InputFilter(toParse) == TypeDefinition.InvalidType)
            return 0;
        if (IsNegative)
            toParse = toParse.Replace("-","");
        if (toParse.Contains(','))
            toParse = toParse.Replace(",","");

        return ReadyToConvert = ulong.Parse(toParse);
    }
    
    internal static BitDefinition MagnitudeChecker()
    {
        var bit = ReadyToConvert switch
        { 
            <= byte.MaxValue
                => BitDefinition.EightBit,
            <= ushort.MaxValue
                => BitDefinition.SixteenBit,
            > ushort.MaxValue
                => BitDefinition.ThirtyTwoBit
        };

        return BitMagnitude = bit;
    }
    
    public static BitDefinition GetMagnitude(string inputBoxText)
    {
        InputParser(inputBoxText);
        return MagnitudeChecker();
    }
    
    public static ulong GetParsed(string inputBoxText)
    {
        SignChecker(inputBoxText);
        var parsed = InputParser(inputBoxText);
        return parsed;
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
