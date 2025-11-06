namespace BitFlare.Logic.Input_Logic;

public class InputValidationState
{
    private readonly char[] _allowedCharacters = ['1','2','3','4','5','6','7','8','9','0','-','e','E',',','.',' '];

    public bool IsValid(string textBoxContent)
    {
        var hasValid = true;
        var hasInvalid = false;
        
        foreach (var character in textBoxContent)
        {
            switch (_allowedCharacters.Contains(character))
            {
                case true:
                    hasValid = true;
                    break;
                case false:
                    hasInvalid = true;
                    break;
            }
        }
        
        return hasValid && !hasInvalid;
    }

}


