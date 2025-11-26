namespace BitFlare.Logic.Input_Logic;

public static class InputValidation
{
    private const string AllowedCharacters = "1234567890-eE,. ";
    private static bool HasValid { get; set; }
    private static bool HasInvalid { get; set; }
    
    public static bool IsValid(string textBoxContent)
    {
        foreach (var character in textBoxContent)
        {
            (HasValid, HasInvalid) = 
                AllowedCharacters.Contains(character) ? (true, false) : (false, true);
        }
        return HasValid && !HasInvalid;
    }
}


