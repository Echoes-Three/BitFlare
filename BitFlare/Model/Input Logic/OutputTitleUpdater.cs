namespace BitFlare.Logic.Input_Logic;

public static class OutputTitleUpdater
{
    public static string UpdateTitle(string input, DefinedBits? bitDefinition = null)
    {
        var updatedTitle = input switch
        {
            _ when input.Contains("e-") || input.Contains('.') && !input.Contains('e') => "IEEE-754 SINGLE PRECISION",
            _ when input.StartsWith('-') && !input.Contains("e-") => "2'S COMPLEMENT",
            _ when !input.StartsWith('-') && !input.Contains("e-") => "UNSIGNED"
        } + $" {Bit(bitDefinition)}";
        return updatedTitle;
    }

    private static string Bit(DefinedBits? bitDefinition)
    {
        return bitDefinition switch 
        {
            DefinedBits.EightBit => "8-BIT",
            DefinedBits.SixteenBit => "16-BIT",
            DefinedBits.ThirtyTwoBit => "32-BIT",
            null => ""
        };
    }
}