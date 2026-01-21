using System.Globalization;
using BitFlare.Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConverterPointer
{
    public static string CallPointer()
    {
        string conversionResult;

        if (ConversionUtilities.InputType == DefinedTypes.Integer)
            conversionResult = IntegerPointer();
        else 
            conversionResult = FloatingPointPointer();
       
        return conversionResult;
    }

    private static string IntegerPointer()
    {
        var paddedResult = IntegerConverter.PaddedBasicConverter(ConversionUtilities.ToConvertInt);
        
        if (!ConversionUtilities.IsNegative) return ConversionUtilities.FormatedOutput(paddedResult);
        
        paddedResult = string.Join("", IntegerConverter.TwosComplement(paddedResult));

        return ConversionUtilities.FormatedOutput(paddedResult);
    }
    
    private static string FloatingPointPointer()
    {
        string binary;
        var toConvert = ConversionUtilities.ToConvertFloat;
        
        if (!toConvert.ToString(CultureInfo.InvariantCulture).StartsWith('0'))
        {
            var integerPart = (uint)Math.Abs(Math.Truncate(toConvert));
            var floatPart = toConvert - integerPart;
            
            binary = FloatingPointConverter.Ieee754(
                $"{IntegerConverter.BasicConverter(integerPart)}.{FloatingPointConverter.BasicConverter(floatPart)}");
            // 5.75 101.11 = 2 = exponent 0.75 = 0000001010100101
        }
        else 
            binary = FloatingPointConverter.Ieee754(
                FloatingPointConverter.BasicConverter(Math.Abs(toConvert)));
        
        return binary;
    } 
}