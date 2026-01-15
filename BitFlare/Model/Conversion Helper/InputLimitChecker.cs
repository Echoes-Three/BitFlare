using BitFlare.Logic;
using BitFlare.Model.Input_Logic;

namespace BitFlare.Model.Conversion_Helper;

public static class InputLimitChecker
{
    public static bool IsWithinLimit(this string input)
    {
        var isWithinLimit = false;
        bool isNegative;
        
        switch (TypeClassification.ClassifiedType)
        {
            case DefinedTypes.Integer:
                
                string integerLimit;
                
                if (input.StartsWith('-'))
                    (integerLimit, isNegative) =
                        input.Contains(',') ? ("2,147,483,648", true) : ("2147483648", true);
                else
                    (integerLimit, isNegative) =
                        input.Contains(',') ? ("4,294,967,295", false) : ("4294967295", false);
                
                input = isNegative ? input[1..] : input;
                var limitLength = input.Contains(',') ? 13 : 10;
            
                // Has one less digit than the limit, automatically smaller: input < limit 
                if (input.Length < limitLength)
                    isWithinLimit = true;
                // Has the same amount of digits, requiring iteration: input == limit
                else if (input.Length == limitLength)
                {
                    for (var index = 0; index <= limitLength - 1; index++)
                    {
                        var inputDigit = input[index] - '0';
                        var limitDigit = integerLimit[index] - '0';
                   
                        if (inputDigit <= limitDigit) 
                            isWithinLimit = true;
                        else
                        {
                            isWithinLimit = false;
                            break;
                        }
                    }
                }
                // Has more digits, automatically bigger: input > limit
                else 
                    isWithinLimit = false;
                break;
            
            case DefinedTypes.ENotation:
                
                isNegative = input.StartsWith('-');
                input = isNegative ? input[1..] : input;
                
                var eNotationLimit =
                    input[input.IndexOf('e') + 1] == '-'
                        ? (Mantissa: 1.4, Exponent: 45)
                        : (Mantissa: 3.4, Exponent: 38);
                
                var inputMantissa = double.Parse(input[..input.IndexOf('e')]);
                var inputExponent = Math.Abs(int.Parse(input[(input.IndexOf('e') + 1)..]));

                if ((inputMantissa <= eNotationLimit.Mantissa && inputExponent <= eNotationLimit.Exponent) ||
                    (inputMantissa > eNotationLimit.Mantissa && inputExponent < eNotationLimit.Exponent)) 
                    isWithinLimit = true;
                else 
                    isWithinLimit = false;
                
                break;
            
            case DefinedTypes.FloatingPoint:
                //code
                break;
        }
        return isWithinLimit;
    }
}