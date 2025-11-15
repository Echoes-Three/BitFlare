using BitFlare.Logic.Input_Logic;

namespace BitFlare.Logic;

public static class IntegerConverter
{
    public static (string, string) BasicConverter(string inputBoxText)
    {
        var toConvert = uint.Parse(inputBoxText);
        
        if (toConvert == 0) return ("0", "8-BIT");
        
        var bitLimit = toConvert switch
                { 
                    <= byte.MaxValue
                        => 8,
                    <= ushort.MaxValue
                        => 16,
                    > ushort.MaxValue
                        => 32,
                };
        
        var converted = new int[bitLimit];


        for (var bit = 1; toConvert > 0; bit++)
        {
            converted[^bit] = (int)toConvert % 2;
            toConvert /= 2;
        }

        return (string.Join("",converted), OutputTitleUpdater.UpdateTitleWithBit(inputBoxText,bitLimit));
    }
}