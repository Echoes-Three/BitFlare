using BitFlare.Model.Conversion_Helper;
namespace BitFlare.Model.Conversion_Logic;

public static class IntegerConverter
{
    public static string Convert(uint input)
    {
        if (input == 0) return "0";
        var output = string.Empty;
        
        for (var i = (int)ConversionUtilities.BitMagnitude - 1; i >=0; i--)
        {
            output += (input >> i) & 1;
        }
        
        var paddedOutput = new int[(int)ConversionUtilities.BitMagnitude]; 
        for (var bit = 1; output.Length >= bit; bit++) 
        { 
            paddedOutput[^bit] = (int)char.GetNumericValue(output[^bit]); 
        }
        
        return string.Join("", paddedOutput);
    }
    
    public static char[] TwosComplement(string paddedOutput)
    {
        // Bit inversion 
        var inverted = string.Empty;
        
        for (var bit = 0; bit <= paddedOutput.Length - 1; bit++)
        {
            if (paddedOutput[bit] == '0')
                inverted += "1";
            else
                inverted += "0";
        }
        
        // MSD incrementation
        var incremented = inverted.ToCharArray();
        
        for (var bit = 1; ; bit++)
        {
            if (incremented[^bit] == '0')
            {
                incremented[^bit] = '1';
                break;
            }
            incremented[^bit] = '0';
        }
        
        return incremented;
    }
}