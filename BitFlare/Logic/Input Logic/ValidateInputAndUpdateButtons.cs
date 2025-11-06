namespace BitFlare.Logic.Input_Logic;

public static class ValidateInputAndUpdateButtons
{
    private static bool IsValid { get; set; } =  true;
    public static bool IsValidToConvert(string? inputBoxText)
    {
        if (inputBoxText == null) IsValid = false;
        return IsValid;
    }

    // public bool IsValidToCopy(string OutputBoxText)
    // {
    //     
    // }
}