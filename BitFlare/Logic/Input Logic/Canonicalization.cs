namespace BitFlare.Logic.Input_Logic;

public static class Canonicalization
{
    private static (string, int) Replacer(string oldText, string toReplace, int caretIndex)
    {
        return (oldText = oldText.Replace(toReplace, toReplace = toReplace.Remove(1)), caretIndex--);
    }
    
    public static (string, int) ToName(string inputBoxText, int caretIndex)
    {
        var normalizedInputBoxText = inputBoxText;
        var newCaretIndex = caretIndex;
        
        if (inputBoxText.Contains(",,"))
            (normalizedInputBoxText, newCaretIndex) = Replacer(inputBoxText, ",,", caretIndex);
        
        if (inputBoxText.Contains(".."))
            (normalizedInputBoxText, newCaretIndex) = Replacer(inputBoxText, "..", caretIndex);
        
        if (inputBoxText.Contains("--"))
            (normalizedInputBoxText, newCaretIndex) = Replacer(inputBoxText, "--", caretIndex);
        
        if (inputBoxText.Contains("++"))
            (normalizedInputBoxText, newCaretIndex) = Replacer(inputBoxText, "++", caretIndex);
        
        if (inputBoxText.Contains("ee"))
            (normalizedInputBoxText, newCaretIndex) = Replacer(inputBoxText, "ee", caretIndex);
        
        if (inputBoxText.Contains('E'))
            normalizedInputBoxText = inputBoxText.Replace('E', 'e');
        
        if (inputBoxText.Contains(' '))
            normalizedInputBoxText = inputBoxText.Replace(" ", "");
        
        if (inputBoxText.StartsWith('.'))
        {
            normalizedInputBoxText = $"0{inputBoxText}";
            newCaretIndex++;
        }
        
        return (normalizedInputBoxText, newCaretIndex);
    }
}