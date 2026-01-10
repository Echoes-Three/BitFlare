using System.Text;
using System.Text.RegularExpressions;

namespace BitFlare.Model.Conversion_Helper;

public static class ENotationDecoder
{
    private const string GroupCapturing =
        @"^(?<sign>-?)(?<fixedDigit>[1-9])(?:\.(?<varyingDigits>[0-9]+))?e-?(?<coefficient>[0-9]{1,2})$";
    
    public static string ToBaseTen(this string input)
    {
        var normalized = input.ToNormalized();
        var eNotation = Regex.Match(normalized, GroupCapturing);
        var baseTen = new List<string[]>();

        if (normalized == "0.0e0") return "0";
        
        if (normalized.Contains('.'))
        {
            if (normalized[normalized.IndexOf('e') + 1] == '-')
                baseTen =
                [
                    [$"{eNotation.Groups["sign"].Value}", "0", "."],
                    [$"{string.Concat(Enumerable.Repeat("0", int.Parse($"{eNotation.Groups["coefficient"].Value}") - 1))}"],
                    [$"{eNotation.Groups["fixedDigit"].Value}", $"{eNotation.Groups["varyingDigits"].Value}"]
                ];
            else
                baseTen =
                [
                    [$"{eNotation.Groups["fixedDigit"].Value}"],
                    [$"{eNotation.Groups["varyingDigits"].Value}"],
                    [$"{string.Concat(Enumerable.Repeat("0", int.Parse($"{eNotation.Groups["coefficient"].Value}") -
                                                             $"{eNotation.Groups["varyingDigits"].Value}".Length))}"]
                ];
        }
        else
        {
            if (normalized[normalized.IndexOf('e') + 1] == '-')
                baseTen =
                [
                    [$"{eNotation.Groups["sign"].Value}", "0", "."],
                    [$"{string.Concat(Enumerable.Repeat("0", int.Parse($"{eNotation.Groups["coefficient"].Value}") - 1))}"],
                    [$"{eNotation.Groups["fixedDigit"].Value}", $"{eNotation.Groups["varyingDigits"].Value}"]
                ];
            else
                baseTen =
                [
                    [$"{eNotation.Groups["fixedDigit"].Value}"],
                    [$"{string.Concat(Enumerable.Repeat("0", int.Parse($"{eNotation.Groups["coefficient"].Value}")))}"],
                    [""]
                    
                ];
            
        }

        return string.Join("",baseTen[0]) + string.Join("",baseTen[1]) + string.Join("",baseTen[2]);
    }

    private static string ToNormalized(this string input)
    {
        //Zero Handling
    var zeroPattern = @"^[+-]?0+(?:\.0+)?e";
    var zeroFilter = new Regex(zeroPattern);
    if (zeroFilter.IsMatch(input)) return "0e0";
    
    //Removes negative sign temporary
    (var isNegative, input) = input.StartsWith('-')
        ? (true, input.Remove(0, 1)) : (false, input);
    
    var normalized = input;
    var hasNegativeExponent = normalized[normalized.IndexOf('e') + 1] == '-';
    string snapshot;
    int marker;
    
    //Positive sign: +5e+5 => 5e5
    normalized = input.Contains('+') ? normalized.Replace("+", "") : normalized;
    
    //Remove leading zeros: 0005.5e5 => 5.5e5 or 000.5e5 => .5e5
    while (normalized[0] == '0')
        normalized = normalized.Remove(0, 1);
    
    //Remove trailing zeros: 5.500e5 => 5.5e5 or 5.00e5 => 5e5
    while (normalized[normalized.IndexOf('e') - 1] == '0' && normalized.Contains('.'))
        normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);
    
    if (input.Contains('.'))
    { 
        //No leading digit: .5e5 => 5e4
        if (normalized.StartsWith('.'))
        {
            snapshot = normalized;
            marker = snapshot.IndexOf('e');
            
            for (var digit = 1;; digit++)
            {
                if (normalized[digit] != '0')
                {
                    var leadingDigit = normalized[digit];
                    
                    if (hasNegativeExponent)
                    {
                        //.5e-5 => 5e-6
                        normalized = normalized[normalized.IndexOf(leadingDigit)..(normalized.IndexOf('e') + 2)];
                        normalized += (int.Parse(snapshot[(marker + 2)..]) + snapshot.IndexOf(leadingDigit))
                            .ToString();
                        
                        if (normalized[..normalized.IndexOf('e')].Length != 1)
                            normalized = normalized.Insert(1, ".");
                        
                        break;
                    }

                    //.5e5 => 5e4
                    normalized = normalized[normalized.IndexOf(leadingDigit)..(normalized.IndexOf('e') + 1)];
                    normalized += (int.Parse(snapshot[(snapshot.IndexOf('e') + 1)..]) - snapshot.IndexOf(leadingDigit))
                        .ToString();
                        
                    if (normalized[..normalized.IndexOf('e')].Length != 1)
                        normalized = normalized.Insert(1, ".");


                    break;
                }
                
                //Catches .0ex scenarios
                if (input[digit] == 'e') normalized = "0.0e0";
            }
        }
        
        //If the radix is in index 1, no correction is required. e.g. 5.001e5 1.2e4 1.e4
        if (normalized[normalized.IndexOf('e') - 1] == '.' || (normalized.IndexOf('.') != 1 && normalized.Contains('.')))
        {
            snapshot = normalized;
            marker = snapshot.IndexOf('e');
            
            //No empty or zeroed exponent digits: 500.e5 => 5e7 or 500.1e5 => 5.001e7
            if (normalized[normalized.IndexOf('e') - 1] == '.' || normalized[normalized.IndexOf('e') - 2] == '.')
            {
                if (hasNegativeExponent)
                {
                    //500.e-5 => 5e-3
                    normalized = normalized.Replace(".", "");
                    normalized = normalized[..(normalized.IndexOf('e') + 1)];
                    normalized += (int.Parse(snapshot[(marker + 1)..]) + snapshot[1..snapshot.IndexOf('.')].Length)
                        .ToString();
                }
                else
                {
                    //500.e5 => 5e7
                    normalized = normalized.Replace(".", "");
                    normalized = normalized[..(normalized.IndexOf('e') + 1)];
                    normalized += (int.Parse(snapshot[(marker + 1)..]) + snapshot[1..snapshot.IndexOf('.')].Length)
                        .ToString();
                }
            }
            
            while (normalized[normalized.IndexOf('e') - 1] == '0')
                normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);
            
            if (normalized[..normalized.IndexOf('e')].Length != 1)
                normalized = normalized.Insert(1, ".");
        }
    }
    else if (normalized.IndexOf('e') != 1)
    {
        //condition changed, so does the snapshot
        snapshot = normalized;
        marker = snapshot.IndexOf('e');
        
        //500e5 => 5e7
        var exponent = int.Parse(snapshot[(marker + 1)..]);
        
        normalized = normalized[..(marker + 1)];
        
        while (normalized[normalized.IndexOf('e') - 1] == '0')
        {
            normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);
            exponent += 1;
        }
        
        exponent += normalized[1..normalized.IndexOf('e')].Length;
        
        if (normalized.IndexOf('e') != 1) 
            normalized = normalized.Insert(1, ".");
        
        normalized += exponent.ToString();
       
    }
    
    snapshot = normalized;
    
    //Removes leading zeros from exponent
    normalized = normalized[..(normalized.IndexOf('e') + 1)];
    var power = int.Parse(snapshot[(snapshot.IndexOf('e') + 1)..]).ToString();
    normalized += power;
    
    return isNegative ? $"-{normalized}" : normalized;
    }
    
    
}