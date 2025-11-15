namespace BitFlare.Logic.Input_Logic;

public static class InputConverterHelper
{
    private static bool IsNegative { get; set; }

    private static bool SignChecker(string inputBoxText)
    {
        return IsNegative = inputBoxText.StartsWith('-');
    }

    public static (string, string) ConverterCaller(string inputBoxText)
    {
        SignChecker(inputBoxText);
        
        var conversionResult = Canonicalization.InputFilter(inputBoxText) switch 
        {
            "INTEGER" => IntegerConverterPointer(inputBoxText),
            // "FLOATING POINT" => FractionConverterPointer(inputBoxText),
            // "E-NOTATION" => ENotationDecoder(inputBoxText)
        };
        
        return conversionResult;
    }
    

    private static void ENotationDecoder(string inputBoxText)
    {
        //create method
    }
    private static (string, string) IntegerConverterPointer(string inputBoxText)
    {
        (string convertedBynary, string updatedTitle) conversionResult = (string.Empty, string.Empty);
        
        if (inputBoxText.Contains(','))
        {
            conversionResult = IntegerConverter.BasicConverter(inputBoxText.Replace(",",""));
        }
        else
        {
            conversionResult = IntegerConverter.BasicConverter(inputBoxText);
        }

        return conversionResult;
    }
    private static void FractionConverterPointer(string inputBoxText)
    {
        if (inputBoxText.Contains(','))
        {
            // FractionConverter(inputBoxText.Replace(',', '_'));
        }
    }
    
    
    
    
    
}