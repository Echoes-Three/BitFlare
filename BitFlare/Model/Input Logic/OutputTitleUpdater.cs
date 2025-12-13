namespace BitFlare.Logic.Input_Logic;

public static class OutputTitleUpdater
{
    public static string UpdateTitle(string input, BitDefinition? bitDefinition = null)
    {
        var updatedTitle = input switch
        {
            _ when input.Contains("e-") || input.Contains('.') && !input.Contains('e') => "IEEE-754 SINGLE PRECISION",
            _ when input.StartsWith('-') && !input.Contains("e-") => "2'S COMPLEMENT",
            _ when !input.StartsWith('-') && !input.Contains("e-") => "BASIC"
        } + $" {Bit(bitDefinition)}";
        return updatedTitle;
    }

    private static string Bit(BitDefinition? bitDefinition)
    {
        return bitDefinition switch 
        {
            BitDefinition.EightBit => "8-BIT",
            BitDefinition.SixteenBit => "16-BIT",
            BitDefinition.ThirtyTwoBit => "32-BIT",
            null => ""
        };
    }
}