using BitFlare.Logic;
using BitFlare.Model.Input_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConverterPointer
{
    public static string PointerCaller(string inputBoxText)
    {
        var conversionResult = "";
        
        switch (TypeClassification.Current)
        {
            case DefinedTypes.Integer:
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