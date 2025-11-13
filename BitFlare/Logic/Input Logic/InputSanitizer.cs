namespace BitFlare.Logic.Input_Logic;

public static class InputSanitizer
{
    
    public static (string, int) Sanitize(string inputBoxText, int caretIndex)
    {
        var sanitizedInputBoxText = inputBoxText;
        var newCaretIndex = caretIndex;

        if (inputBoxText.Contains('+'))
            (sanitizedInputBoxText, newCaretIndex) = 
                (inputBoxText.Replace("+", ""), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains(' '))
            (sanitizedInputBoxText, newCaretIndex) =
                (inputBoxText.Replace(" ", ""), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains("--"))
            (sanitizedInputBoxText, newCaretIndex) =
                (inputBoxText.Replace("--", "-"), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains(",,"))
            (sanitizedInputBoxText, newCaretIndex) =
                (inputBoxText.Replace(",,", ","), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (inputBoxText.Contains('E'))
            sanitizedInputBoxText = inputBoxText.ToLower();
        
        if (inputBoxText.StartsWith('.'))
            (sanitizedInputBoxText, newCaretIndex) = ($"0{inputBoxText}", + 2);
        
        return SanitizeHelper(sanitizedInputBoxText, newCaretIndex);
    }
    
    private static (string, int) SanitizeHelper(string inputBoxText, int caretIndex)
    {
        var sanitizedInputBoxText = "";
        var newCaretIndex = caretIndex;

        if (inputBoxText.Contains('.') || inputBoxText.Contains('e'))
        {
            foreach (var character in inputBoxText)
            {
                if (sanitizedInputBoxText.Contains(character) && character is 'e' or '.') 
                    newCaretIndex = caretIndex - 1;
                else 
                    sanitizedInputBoxText += character;
            }
            return (sanitizedInputBoxText, newCaretIndex);
        }
        return (inputBoxText, newCaretIndex);
    }
}
