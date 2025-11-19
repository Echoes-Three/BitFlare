using BitFlare.Logic.Input_Logic;

namespace BitFlare.Logic;

public static class IntegerConverter
{
    public static string BasicConverter(ulong parsedInput)
    {
        if (parsedInput == 0) return "0";
        
        var converted = "";
        
        while (parsedInput > 0)
        {
            converted = parsedInput % 2 + converted;
            parsedInput /= 2;
        }

        return converted;
    }
}