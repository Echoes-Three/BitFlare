namespace BitFlare.Logic.Input_Logic;

public class ValidationState
{
    private bool Valid { get; set; } = true;
    private readonly char[] _allowedCharacters =
    [
        '1','2','3','4','5','6','7','8','9','0',
        '-','+',
        'e','E',
        ',','.',
        ' '
    ];

    public bool IsValid(string textBoxContent)
    {
        foreach (var character in textBoxContent)
        {
            Valid = _allowedCharacters.Contains(character) switch
            {
                false => false,
                _ => true
            };
        }
        return Valid;
    }

}


