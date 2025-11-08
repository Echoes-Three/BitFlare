namespace BitFlare.Logic.Input_Logic;

public static class InputCanonicalization
{
    private static readonly char[] ForbiddenChars = ['.', '+', 'e'];

    private static (string, int) DoubleRemover(string inputBoxText, int caretIndex)
    {
        var newCaretIndex = caretIndex;
        var normalizedInputBoxText = "";
        foreach (var character in inputBoxText)
        {
            if (normalizedInputBoxText.Contains(character) && ForbiddenChars.Contains(character)) 
                newCaretIndex = caretIndex - 1;
            else 
                normalizedInputBoxText += character;
            
        }
        return (normalizedInputBoxText, newCaretIndex);
    }
    
    public static (string, int) IsDoubled(string inputBoxText, int caretIndex)
    {
        var normalizedInputBoxText = inputBoxText;
        var newCaretIndex = caretIndex;
        
        //Use LINQ later
        if (inputBoxText.Contains('.') ||
            inputBoxText.Contains('-') ||
            inputBoxText.Contains('+') ||
            inputBoxText.Contains('e'))
            (normalizedInputBoxText, newCaretIndex) = DoubleRemover(inputBoxText, caretIndex);
        
        return (normalizedInputBoxText, newCaretIndex);
    }
    
    public static (string, int) CharacterNormalizer(string inputBoxText, int caretIndex)
    {
        var normalizedInputBoxText = inputBoxText;
        var newCaretIndex = caretIndex;
        
        if (inputBoxText.Contains(",,"))
            normalizedInputBoxText = inputBoxText.Replace(",,", ",");
        
        if (inputBoxText.Contains('E'))
            normalizedInputBoxText = inputBoxText.Replace('E', 'e');
        
        if (inputBoxText.Contains(' '))
            normalizedInputBoxText = inputBoxText.Replace(" ", "");

        if (inputBoxText.StartsWith('.'))
            (normalizedInputBoxText, newCaretIndex) = ($"0{inputBoxText}", + 2);
        
        return (normalizedInputBoxText, newCaretIndex);
    }
}