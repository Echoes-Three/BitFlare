namespace BitFlare.Logic.Input_Logic;

public class InputValidationState
{
    private bool Valid { get; set; } = true;
    private readonly char[] _allowedCharacters = ['1','2','3','4','5','6','7','8','9','0','-','e','E',',','.',' '];

    public bool IsValid(string textBoxContent)
    {
        foreach (var character in textBoxContent)
        {
            //failed logic, it's looking at the last character typed, not at the string. 
            Valid = _allowedCharacters.Contains(character) switch
            {
                false => false,
                _ => true
            };
        }
        return Valid;
    }

}


