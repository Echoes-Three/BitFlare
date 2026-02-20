namespace BitFlare.Model.Input_Logic;

public static class InputSafety
{
    private static string? Input { get; set; }
    private static int CaretIndex { get; set; }

    public static (string input, int caretIndex) Sanitize(string input, int caretIndex)
    {
        if (string.IsNullOrWhiteSpace(input))
            return ("", 0);
        
        (Input, CaretIndex) = (input, caretIndex);

        BasicSanitizer();
        DuplicateSanitizer();
        
        return (Input, CaretIndex);
    }
    
    private static void BasicSanitizer()
    {
        if (Input.Contains("++"))
            (Input, CaretIndex) = 
                (Input.Replace("++", "+"), CaretIndex < 1 ? CaretIndex : CaretIndex - 1);
        
        if (Input.Contains(' '))
            (Input, CaretIndex) =
                (Input.Replace(" ", ""), CaretIndex < 1 ? CaretIndex : CaretIndex - 1);
        
        if (Input.Contains("--"))
            (Input, CaretIndex) =
                (Input.Replace("--", "-"), CaretIndex < 1 ? CaretIndex : CaretIndex - 1);
        
        if (Input.Contains(",,"))
            (Input, CaretIndex) =
                (Input.Replace(",,", ","), CaretIndex < 1 ? CaretIndex : CaretIndex - 1);
        
        if (Input.Contains('E'))
            Input = Input.ToLower();
    }
    
    private static void DuplicateSanitizer()
    {
        var sanitizedInput = "";

        if (!Input.Contains('.') && !Input.Contains('e'))
            return;
        
        foreach (var character in Input)
        {
            if (sanitizedInput.Contains(character) && character is 'e' or '.') 
                CaretIndex -= 1;
            else 
                sanitizedInput += character;
        }
        
        Input = sanitizedInput;
    }
}
