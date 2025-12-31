namespace BitFlare.Logic.Input_Logic;

public static class CharacterValidation
{
    private const string AllowedCharacters = "1234567890-eE,.+ ";
    private static bool HasValid { get; set; } = true;
    private static bool HasInvalid { get; set; }
    
    public static bool IsValid(string input)
    {
        foreach (var character in input)
        {
            (HasValid, HasInvalid) = 
                AllowedCharacters.Contains(character) ? (true, false) : (false, true);
        }
        return HasValid && !HasInvalid;
    }
}


