using System.Text.RegularExpressions;
namespace BitFlare.Model.Conversion_Helper;

public static class ENotationUtilities
{
    private const string GroupCapturing =
        @"^(?<sign>-?)(?<fixedDigit>[1-9])(?:\.(?<varyingDigits>[0-9]+))?e-?(?<coefficient>[0-9]{1,2})$";
    
    public static string ToBaseTen(string input)
    {
        var normalizedInput = input;
        var eNotation = Regex.Match(normalizedInput, GroupCapturing);
        var baseTen = new List<string[]>();

        if (normalizedInput == "0.0e0") return "0";
        if (normalizedInput.Contains('.'))
        {
            //5.5e5 => 550000
            var exponent = int.Parse(normalizedInput[(normalizedInput.IndexOf('e') + 1)..]);
            
            normalizedInput = normalizedInput[..normalizedInput.IndexOf('e')];
            normalizedInput += string.Concat(Enumerable.Repeat("0", 40 - normalizedInput.Length ));
            
            if (exponent != 0)
            {
                normalizedInput = normalizedInput.Insert(normalizedInput.IndexOf('.', StringComparison.Ordinal) + exponent + 1, ".");
                normalizedInput = normalizedInput.Remove(normalizedInput.IndexOf('.', StringComparison.Ordinal), 1);
            }

            for (var bit = 1;; bit++)
            {
                if (normalizedInput[^bit] != '0')
                {
                    normalizedInput = normalizedInput[..(normalizedInput.Length - bit + 1)]; 
                    break;   
                }
                if (normalizedInput[^bit] == '.')
                {
                    normalizedInput = normalizedInput[..(normalizedInput.IndexOf('.') + 2)];
                    break; 
                }
            }
            return normalizedInput;
            
        }
        //5e5 => 500000
        baseTen =
        [
            [$"{eNotation.Groups["fixedDigit"].Value}"],
            [$"{string.Concat(Enumerable.Repeat("0", int.Parse($"{eNotation.Groups["coefficient"].Value}")))}"],
            [""]
        ];
        
        return string.Join("",baseTen[0]) + string.Join("",baseTen[1]) + string.Join("",baseTen[2]);
    }

    public static string ToNormalized(string input)
    {
        //Zero Handling
        const string zeroPattern = @"^[+-]?0+(?:\.0+)?e";
        var zeroFilter = new Regex(zeroPattern);
        if (zeroFilter.IsMatch(input)) return "0e0";
    
        //Removes negative sign temporarily
        (var isNegative, input) = input.StartsWith('-')
            ? (true, input.Remove(0, 1)) : (false, input);
    
        var result = input;
        var isFraction = result[result.IndexOf('e') + 1] == '-';
        string snapshot;
        int marker;
    
        //Positive sign: +5e+5 => 5e5
        result = input.Contains('+') ? result.Replace("+", "") : result;
        
        //Remove leading zeros: 0005.5e5 => 5.5e5 or 000.5e5 => .5e5
        while (result[0] == '0')
            result = result.Remove(0, 1);
        
        //Remove trailing zeros: 5.500e5 => 5.5e5 or 5.00e5 => 5e5
        while (result[result.IndexOf('e') - 1] == '0' && result.Contains('.'))
            result = result.Remove(result.IndexOf('e') - 1, 1);
        
        if (input.Contains('.'))
        { 
            //No leading digit: .5e5 => 5e4
            if (result.StartsWith('.'))
            {
                snapshot = result;
                marker = snapshot.IndexOf('e');
                
                for (var digit = 1;; digit++)
                {
                    if (result[digit] != '0')
                    {
                        var leadingDigit = result[digit];
                        
                        if (isFraction)
                        {
                            //.5e-5 => 5e-6
                            result = result[result.IndexOf(leadingDigit)..(result.IndexOf('e') + 2)];
                            result += (int.Parse(snapshot[(marker + 2)..]) + snapshot.IndexOf(leadingDigit))
                                .ToString();
                            
                            if (result[..result.IndexOf('e')].Length != 1)
                                result = result.Insert(1, ".");
                            
                            break;
                        }

                        //.5e5 => 5e4
                        result = result[result.IndexOf(leadingDigit)..(result.IndexOf('e') + 1)];
                        result += (int.Parse(snapshot[(snapshot.IndexOf('e') + 1)..]) - snapshot.IndexOf(leadingDigit))
                            .ToString();
                            
                        if (result[..result.IndexOf('e')].Length != 1)
                            result = result.Insert(1, ".");
                        
                        break;
                    }
                    
                    //Catches .0ex scenarios
                    if (input[digit] == 'e') result = "0.0e0";
                }
            }
            
            //If the radix is in index 1, no correction is required. e.g. 5.001e5 1.2e4 1.e4
            if (result[result.IndexOf('e') - 1] == '.' || (result.IndexOf('.') != 1 && result.Contains('.')))
            {
                snapshot = result;
                marker = snapshot.IndexOf('e');
                
                //No empty or zeroed exponent digits: 500.e5 => 5e7 or 500.1e5 => 5.001e7
                if (isFraction)
                {
                    //500.e-5 => 5e-3
                    result = result.Replace(".", "");
                    result = result[..(result.IndexOf('e') + 1)];
                    result += (int.Parse(snapshot[(marker + 1)..]) + snapshot[1..snapshot.IndexOf('.')].Length)
                        .ToString();
                }
                else
                {
                    //500.e5 => 5e7
                    result = result.Replace(".", "");
                    result = result[..(result.IndexOf('e') + 1)];
                    result += (int.Parse(snapshot[(marker + 1)..]) + snapshot[1..snapshot.IndexOf('.')].Length)
                        .ToString();
                }
                
                
                while (result[result.IndexOf('e') - 1] == '0')
                    result = result.Remove(result.IndexOf('e') - 1, 1);
                
                if (result[..result.IndexOf('e')].Length != 1)
                    result = result.Insert(1, ".");
            }
        }
        else if (result.IndexOf('e') != 1)
        {
            //condition changed, so does the snapshot
            snapshot = result;
            marker = snapshot.IndexOf('e');
            
            //500e5 => 5e7
            var exponent = int.Parse(snapshot[(marker + 1)..]);
            
            result = result[..(marker + 1)];
            
            while (result[result.IndexOf('e') - 1] == '0')
            {
                result = result.Remove(result.IndexOf('e') - 1, 1);
                exponent += 1;
            }
            
            exponent += result[1..result.IndexOf('e')].Length;
            
            if (result.IndexOf('e') != 1) 
                result = result.Insert(1, ".");
            
            result += exponent.ToString();
           
        }
        
        snapshot = result;
        
        //Removes leading zeros from exponent: e0005 => e5
        result = result[..(result.IndexOf('e') + 1)];
        var power = int.Parse(snapshot[(snapshot.IndexOf('e') + 1)..]).ToString();
        result += power;
        
        return isNegative ? $"-{result}" : result;
        
        }
}