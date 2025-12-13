using BitFlare.Logic.Input_Logic;

namespace BitFlare.Logic;

public static class IntegerConverter
{
    public static string BasicConverter(ulong parsedInput)
    {
        if (parsedInput == 0) return "0";

        var converted = string.Empty;
        
        while (parsedInput > 0)
        {
            converted = parsedInput % 2 + converted;
            parsedInput /= 2;
        }

        return converted;
    }

    public static char[] TwosComplement(string paddedResult)
    {
        //inverts the bits 
        var inverterd = string.Empty;
        
        for (var bit = 0; bit <= paddedResult.Length - 1; bit++)
        {
            if (paddedResult[bit] == '0')
                inverterd += "1";
            else
                inverterd += "0";
        }
        
        //Increments most significant digit
        var incremented = inverterd.ToCharArray();
        
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