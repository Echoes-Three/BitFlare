using System.Globalization;
using BitFlare.Logic;
using BitFlare.Model.Conversion_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class ConverterPointer
{
    public static string CallPointer()
    {
        string conversionResult;
        //DO NOT USE TERNARY OPERATOR
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
        var floatConverter = new FloatingPointConverter();
        string binary;
        var toConvert = ConversionUtilities.ToConvertFloat;
        var hasNoIntegerPart = Math.Abs(toConvert) < 1;
        
        if (hasNoIntegerPart)
            binary = floatConverter.Ieee754(floatConverter.BasicConverter(Math.Abs(toConvert))); 
        else
        {
            var integerPart = (uint)Math.Abs(Math.Truncate(toConvert));
            var floatPart = toConvert - integerPart;
            
            binary = floatConverter.Ieee754(
                $"{IntegerConverter.BasicConverter(integerPart)}.{floatConverter.NonZeroBasicConverter(floatPart)}");
        }
        
        return binary;
    } 
}