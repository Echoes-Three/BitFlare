using BitFlare.Logic.Input_Logic;

namespace BitFlare.Logic.Conversion_Helper;

public class ConverterPointer
{
    public static string PointerCaller(string inputBoxText)
    {
        var conversionResult = "";
        
        switch (InputTypeDefinition.InputFilter(inputBoxText))
        {
            case TypeDefinition.Integer:
                ConversionUtilities.SignChecker(inputBoxText);
                ConversionUtilities.InputParser(inputBoxText);
                ConversionUtilities.MagnitudeChecker();
                ConversionUtilities.InputType = TypeDefinition.Integer;
                conversionResult = IntegerConversionPointer();
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

    private static string IntegerConversionPointer()
    {
        var conversionResult = IntegerConverter.BasicConverter(ConversionUtilities.ReadyToConvert);
        var paddedResult = new int[(int)ConversionUtilities.BitMagnitude];

        for (var bit = 1; conversionResult.Length >= bit; bit++)
        {
            paddedResult[^bit] = (int)char.GetNumericValue(conversionResult[^bit]);
        }

        if (!ConversionUtilities.IsNegative) return ConversionUtilities.FormatedOutput(paddedResult);

        var joined = string.Join("", paddedResult);
        var converted = IntegerConverter.TwosComplement(joined);
        paddedResult = converted.Select(c => c - '0').ToArray();

        return ConversionUtilities.FormatedOutput(paddedResult);
    }
    
    private static void FractionConverterPointer(string inputBoxText)
    {
        if (inputBoxText.Contains(','))
        {
            // FractionConverter(inputBoxText.Replace(',', '_'));
        }
    } 
    
    private static void ENotationDecoder(string inputBoxText)
    {
        //create method
    }

}