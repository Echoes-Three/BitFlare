using BitFlare.Model.Conversion_Logic;
using static BitFlare.Model.Conversion_Logic.IntegerConverter;

namespace BitFlare.Model.Conversion_Helper;

public static class ConverterPointer
{
    public static (string, string) CallPointer()
    {
        return ConversionUtilities.InputType == DefinedTypes.Integer
            ? IntegerPointer()
            : IeeePointer();
    }

    private static (string, string) IntegerPointer()
    {
        var input = ConversionUtilities.ToConvertInt;
        var paddedOutput = ConversionUtilities.IsNegative
            ? string.Join("", TwosComplement(Convert(input)))
            : Convert(input);

        var raw = -(int)input;
        var hexadecimal = ConversionUtilities.IsNegative
            ? ((uint)raw).ToString("X")
            : input.ToString("X");
        
        return (ConversionUtilities.FormatedOutput(paddedOutput), hexadecimal);
    }
    
    private static (string, string) IeeePointer()
    {
        return IeeeConverter.Ieee754(ConversionUtilities.ToConvertIeee);
    } 
}