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
        var rawConversion = IntegerConverter.BasicConverter(ConversionUtilities.ToConvertInt);
        var paddedResult = new int[(int)ConversionUtilities.BitMagnitude];

        for (var bit = 1; rawConversion.Length >= bit; bit++)
        {
            paddedResult[^bit] = (int)char.GetNumericValue(rawConversion[^bit]);
        }

        if (!ConversionUtilities.IsNegative) return ConversionUtilities.FormatedOutput(paddedResult);

        var joined = string.Join("", paddedResult);
        var converted = IntegerConverter.TwosComplement(joined);
        paddedResult = converted.Select(c => c - '0').ToArray();

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
        }
        else 
            binary = FloatingPointConverter.Ieee754(
                FloatingPointConverter.BasicConverter(Math.Abs(toConvert)));
        
        return binary;
    } 
}