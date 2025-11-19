namespace BitFlare.Logic.Input_Logic;

public static class InputConverterHelper
{
    private static bool IsNegative { get; set; }
    private static ulong ToConvert {get; set; }
    private static BitDefinition BitMagnitude {get; set;}

    private static bool SignChecker(string inputBoxText) =>
         IsNegative = inputBoxText.StartsWith('-');
    
    private static ulong InputParser(string inputBoxText)
    {
        var toParse = inputBoxText;
        
        if (IsNegative)
            toParse = inputBoxText.Replace("-","");
        if (inputBoxText.Contains(','))
            toParse = toParse.Replace(",","");

        return ToConvert = ulong.Parse(toParse);
    }
    
    private static BitDefinition MagnitudeChecker()
    {
        var bit = ToConvert switch
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
        SignChecker(inputBoxText);
        InputParser(inputBoxText);
        return MagnitudeChecker();
    }
    
    public static ulong GetParsed(string inputBoxText)
    {
        var parsed = SignChecker(inputBoxText) 
            ? uint.Parse($"-{InputParser(inputBoxText)}") 
            : InputParser(inputBoxText);
        
        return parsed;
    }
    


    public static string ConverterCaller(string inputBoxText)
    {
        var conversionResult = "";
        
        switch (Canonicalization.InputFilter(inputBoxText))
        {
            case "INTEGER":
                SignChecker(inputBoxText);
                InputParser(inputBoxText);
                MagnitudeChecker();
                conversionResult = IntegerConverterPointer();
                break;
            /*case "FLOATING POINT":
                conversionResult = FractionConverterPointer(inputBoxText);
                break;
            case "E-NOTATION":
                conversionResult = ENotationDecoder(inputBoxText);
                break;*/
        }
        
        return conversionResult;
    }
    

    private static void ENotationDecoder(string inputBoxText)
    {
        //create method
    }
    
    private static string IntegerConverterPointer()
    {
        var conversionResult = IntegerConverter.BasicConverter(ToConvert);
        
        var paddedResult = new int[(int)BitMagnitude];

        for (var bit = 1; conversionResult.Length >= bit; bit++)
        {
            paddedResult[^bit] = (int)char.GetNumericValue(conversionResult[^bit]);
        }

        return FormatedOutput(paddedResult);
    }

    private static string FormatedOutput(int[] paddedResult)
    {
        // in the future format based of the output time
        var toFormat = string.Join("", paddedResult);
        
        // 0000 0000 0000 0000
        var spaceLimiter = BitMagnitude switch
        {
            BitDefinition.EightBit => 0,
            BitDefinition.SixteenBit => 2,
            BitDefinition.ThirtyTwoBit => 6
        };
        
        for (var i = (bitSpaces: 4, accounter: 0); i.accounter <= spaceLimiter ; i.bitSpaces += 4, i.accounter++)
        {
            toFormat = toFormat.Insert(i.bitSpaces + i.accounter, " ");
        }
        
        return toFormat;
    }
    
    private static void FractionConverterPointer(string inputBoxText)
    {
        if (inputBoxText.Contains(','))
        {
            // FractionConverter(inputBoxText.Replace(',', '_'));
        }
    }
    
    
    
    
    
}