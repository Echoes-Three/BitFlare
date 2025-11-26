namespace BitFlare.Logic.Input_Logic;

public static class OutputTitleUpdater
{
    public static string UpdateTitle(string textBoxInput)
    {
        return textBoxInput switch
        {
            _ when textBoxInput.Contains("e-") ||textBoxInput.Contains('.') && !textBoxInput.Contains('e') => "IEEE-754 SINGLE PRECISION",
            _ when textBoxInput.StartsWith('-') && !textBoxInput.Contains("e-") => "2'S COMPLEMENT",
            _ when !textBoxInput.StartsWith('-') && !textBoxInput.Contains("e-") => "BASIC"
        };
    }

    public static string UpdateTitleWithBit(string textBoxInput, BitDefinition bitDefinition)
    {
        return bitDefinition switch 
        {
            BitDefinition.EightBit => $"{UpdateTitle(textBoxInput)} 8-BIT",
            BitDefinition.SixteenBit => $"{UpdateTitle(textBoxInput)} 16-BIT",
            BitDefinition.ThirtyTwoBit => $"{UpdateTitle(textBoxInput)} 32-BIT"
        };
    }
}