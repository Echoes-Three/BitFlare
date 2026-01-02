using BitFlare.Logic;

namespace BitFlare.Model.Input_Logic;

public static class InputSanitizer
{
    public static (string input, int caretIndex) Sanitizers(string input, int caretIndex)
    {
        if (string.IsNullOrWhiteSpace(input))
            return ("", 0);
        
        if (input.Contains("++"))
            (input, caretIndex) = 
                (input.Replace("++", "+"), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (input.Contains(' '))
            (input, caretIndex) =
                (input.Replace(" ", ""), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (input.Contains("--"))
            (input, caretIndex) =
                (input.Replace("--", "-"), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (input.Contains(",,"))
            (input, caretIndex) =
                (input.Replace(",,", ","), caretIndex < 1 ? caretIndex : caretIndex - 1);
        
        if (input.Contains('E'))
            input = input.ToLower();
        
        if (input.StartsWith('.'))
            (input, caretIndex) = ($"0{input}", + 2);
        
        return DuplicateSanitizer(input, caretIndex);
    }
    
    private static (string, int) DuplicateSanitizer(string input, int caretIndex)
    {
        var sanitizedInputBoxText = "";

        if (!input.Contains('.') && !input.Contains('e'))
            return LenghtSanitizer(input, caretIndex);
        
        foreach (var character in input)
        {
            if (sanitizedInputBoxText.Contains(character) && character is 'e' or '.') 
                caretIndex -= 1;
            else 
                sanitizedInputBoxText += character;
        }
        return LenghtSanitizer(sanitizedInputBoxText, caretIndex);
    }

    private static (string, int) LenghtSanitizer(string input, int caretIndex)
    {
        //prepare better the decimal limiter using precision
        if (TypeClassification.Current != DefinedTypes.ENotation && input.Length > 15)
        {
            return (input[..15], caretIndex);
        }
        else
        {
            //prepare e-notation limiter
        }
        return (input, caretIndex);
    }
}
