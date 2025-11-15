namespace BitFlare.Logic.Input_Logic;

public static class OutputTitleUpdater
{
    public static string UpdateTitle(string textBoxInput)
    {
        var correctTitle = "";

        correctTitle = textBoxInput switch
        {
            _ when textBoxInput.Contains("e-") || textBoxInput.Contains('.') && !textBoxInput.Contains('e') => "IEEE-754 SINGLE PRECISION",
            _ when textBoxInput.StartsWith('-') && !textBoxInput.Contains("e-") => "2'S COMPLEMENT",
            _ when !textBoxInput.StartsWith('-') && !textBoxInput.Contains("e-") => "BASIC"
        };

        // work on the correcting unit that normalizes the e-notation and reject any other input to rightfully warn the user and update the title
        return correctTitle;
    }

    public static string UpdateTitleWithBit(string textBoxInput, int bitIdentification)
    {
        var correctTitleWithBit = "";
        return correctTitleWithBit = bitIdentification switch 
        {
            8 => $"{UpdateTitle(textBoxInput)} 8-BIT",
            16 => $"{UpdateTitle(textBoxInput)} 16-BIT",
            32 => $"{UpdateTitle(textBoxInput)} 32-BIT"
        };
        
        
    }
}