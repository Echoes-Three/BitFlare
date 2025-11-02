namespace BitFlare.Logic.Input_Logic;

public static class InputCanonicalization
{

    private static (string, int) DoubleRemover(string inputBoxText, char characterToRemove, int caretIndex)
    {
        var unique = "";
        foreach (var character in inputBoxText)
        {
            if (unique.Contains(character) && character == characterToRemove)
            {
            }
            else
                unique += character;
        }
        
        var normalizedInputBoxText = unique;
        var newCaretIndex = normalizedInputBoxText.Length;
        
        return (normalizedInputBoxText, newCaretIndex);
    }
    
    public static (string, int) IsDoubled(string inputBoxText, int caretIndex)
    {
        var normalizedInputBoxText = inputBoxText;
        var newCaretIndex = caretIndex;

        if (inputBoxText.Contains('.'))
            (normalizedInputBoxText, newCaretIndex) = DoubleRemover(inputBoxText, '.', caretIndex);
        
        if (inputBoxText.Contains('-'))
            (normalizedInputBoxText, newCaretIndex) = DoubleRemover(inputBoxText, '-', caretIndex);
        
        if (inputBoxText.Contains('+'))
            (normalizedInputBoxText, newCaretIndex) = DoubleRemover(inputBoxText, '+', caretIndex);
        
        if (inputBoxText.Contains('e'))
            (normalizedInputBoxText, newCaretIndex) = DoubleRemover(inputBoxText, 'e', caretIndex);
        
        return (normalizedInputBoxText, newCaretIndex);
    }
    
    private static (string, int) Replacer(string oldText, string toReplace, int caretIndex) =>
        (oldText.Replace(toReplace, toReplace.Remove(1)), caretIndex--);
    
    public static (string, int) RepeatedCharacterNormalizer(string inputBoxText, int caretIndex)
    {
        var normalizedInputBoxText = inputBoxText;
        var newCaretIndex = caretIndex;
        
        if (inputBoxText.Contains(",,"))
            (normalizedInputBoxText, newCaretIndex) = Replacer(inputBoxText, ",,", caretIndex);
        
        if (inputBoxText.Contains('E'))
            normalizedInputBoxText = inputBoxText.Replace('E', 'e');
        
        if (inputBoxText.Contains(' '))
            normalizedInputBoxText = inputBoxText.Replace(" ", "");

        if (inputBoxText.StartsWith('.'))
            (normalizedInputBoxText, newCaretIndex) = ($"0{inputBoxText}", newCaretIndex++);
        
        return (normalizedInputBoxText, newCaretIndex);
    }
}