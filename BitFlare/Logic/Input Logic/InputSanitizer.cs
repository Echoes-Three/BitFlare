namespace BitFlare.Logic.Input_Logic;

public static class InputSanitizer
{
    
    public static (string, int) DuplicateSanitizer(string inputBoxText, int caretIndex)
    {
        if (inputBoxText.Contains('+'))
            (inputBoxText, caretIndex) = 
                (inputBoxText.Replace("+", ""), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains(' '))
            (inputBoxText, caretIndex) =
                (inputBoxText.Replace(" ", ""), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains("--"))
            (inputBoxText, caretIndex) =
                (inputBoxText.Replace("--", "-"), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains(",,"))
            (inputBoxText, caretIndex) =
                (inputBoxText.Replace(",,", ","), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains('E'))
            inputBoxText = inputBoxText.ToLower();
        
        if (inputBoxText.StartsWith('.'))
            (inputBoxText, caretIndex) = ($"0{inputBoxText}", + 2);
        
        return InvalidSanitizer(inputBoxText, caretIndex);
    }
    
    private static (string, int) InvalidSanitizer(string inputBoxText, int caretIndex)
    {
        var sanitizedInputBoxText = "";

        if (!inputBoxText.Contains('.') && !inputBoxText.Contains('e'))
            return LenghtSanitizer(inputBoxText, caretIndex);
        
        foreach (var character in inputBoxText)
        {
            if (sanitizedInputBoxText.Contains(character) && character is 'e' or '.') 
                caretIndex -= 1;
            else 
                sanitizedInputBoxText += character;
        }
        return LenghtSanitizer(sanitizedInputBoxText, caretIndex);
    }

    private static (string, int) LenghtSanitizer(string inputBoxText, int caretIndex)
    {
        if (InputTypeDefinition.InputFilter(inputBoxText) != TypeDefinition.ENotation && inputBoxText.Length > 15)
        {
            return (inputBoxText[..15], caretIndex);
        }
        else
        {
            //prepare e-notation limiter
        }
        return (inputBoxText, caretIndex);
    }
}
