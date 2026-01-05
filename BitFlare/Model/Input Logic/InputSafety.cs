using BitFlare.Logic;

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
        LengthSanitizer();
        
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
        
        if (Input.StartsWith('.'))
            (Input, CaretIndex) = ($"0{Input}", + 2);
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

    private static void LengthSanitizer()
    {
        string integerLimit;
        var isNegative = false;

        if (Input.StartsWith('-'))
        {
            integerLimit = Input.Contains(',') ? "2,147,483,648" : "2147483648";
            isNegative = true;
        }
        else
            integerLimit = Input.Contains(',') ? "4,294,967,295" : "4294967295";
        
        TypeClassification.TypeFilter(Input);

        if (TypeClassification.Current == DefinedTypes.Integer)
        {
            if (isNegative)
                Input.Remove('-');

            for (var i = 0; i <= integerLimit.Length; i++)
            {
                
            }

        }
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
