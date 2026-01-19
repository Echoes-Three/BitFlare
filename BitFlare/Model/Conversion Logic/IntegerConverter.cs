using BitFlare.Logic.Input_Logic;

namespace BitFlare.Logic;

public static class IntegerConverter
{
    public static string BasicConverter(uint input)
    {
        if (input == 0) return "0";

        var converted = string.Empty;
        
        while (input > 0)
        {
            converted = input % 2 + converted;
            input /= 2;
        }

        return converted;
    }

    public static char[] TwosComplement(string paddedResult)
    {
        // Bit inversion 
        var inverted = string.Empty;
        
        for (var bit = 0; bit <= paddedResult.Length - 1; bit++)
        {
            if (paddedResult[bit] == '0')
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