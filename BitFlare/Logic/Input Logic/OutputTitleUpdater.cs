namespace BitFlare.Logic.Input_Logic;

public static class OutputTitleUpdater
{
    public static string UpdateTitle(string textBoxInput)
    {
        var correctTitle = "";

        correctTitle = textBoxInput switch
        {
            _ when textBoxInput.Contains("e-") || textBoxInput.Contains('.') && !textBoxInput.Contains('e') => "IEEE-754 SINGLE PRECISION",
            _ when textBoxInput.StartsWith('-') && !textBoxInput.Contains("e-") => "2'S COMPLEMENTS",
            _ when !textBoxInput.StartsWith('-') && !textBoxInput.Contains("e-") => "BASIC"
        };

        return correctTitle;
    }
}