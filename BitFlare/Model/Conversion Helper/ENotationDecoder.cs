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
        if (input is "0e0" or "0.0e0") return "0.0e0";
    
        var normalized = input;
        var snapshot = normalized;
        var marker = snapshot.IndexOf('e');
        
        //Positive sign: +5e+5 => 5e5
        if (input.Contains('+')) normalized = normalized.Replace("+", "");
        
        if (input.Contains('.'))
        { 
            //Remove leading zeros: 0005.5e5 => 5.5e5 or 000.5e5 => .5e5
            while (normalized[0] == '0')
                normalized = normalized.Remove(0, 1);
            
            //Remove trailing zeros: 0005.5e5 => 5.5e5 or .5000e5 => 5.5e5
            while (normalized[normalized.IndexOf('e') - 1] == '0') 
                normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);
            
            //No leading digit: .5e5 => 5e4
            if (normalized.StartsWith('.'))
            {
                //condition changed, so does the snapshot
                snapshot = normalized;
                
                //Shifts the radix to the correct spot
                for (var digit = 1;; digit++)
                {
                    if (normalized[digit] != '0')
                    {
                        var leadingDigit = normalized[digit];
                        
                        if (normalized[normalized.IndexOf('e') + 1] == '-')
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
            
            //If the radix is in index 1, no correction is required. e.g. 5.001e5
            if (normalized.IndexOf('.') != 1 && normalized.IndexOf('e') != 1)
            {
                //condition changed, so does the snapshot
                snapshot = normalized;
                
                //No empty or zeroed exponent digits: 500.e5 => 5e7 or 500.1e5 => 5.001e7
                if (normalized[normalized.IndexOf('e') - 1] == '.')
                {
                    if (normalized[normalized.IndexOf('e') + 1] == '-')
                    {
                        //500.e-5 => 5e-3
                        normalized = normalized.Replace(".", "");
                        normalized = normalized[..(normalized.IndexOf('e') + 2)];

                        while (normalized[normalized.IndexOf('e') - 1] == '0')
                            normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);

                        normalized += (int.Parse(snapshot[(marker + 2)..]) - snapshot[1..marker].Length)
                            .ToString();
                    }
                    else
                    {
                        //500.e5 => 5e7
                        normalized = normalized.Replace(".", "");
                        normalized = normalized[..(normalized.IndexOf('e') + 1)];

                        while (normalized[normalized.IndexOf('e') - 1] == '0')
                            normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);

                        normalized += (int.Parse(snapshot[(marker + 1)..]) + snapshot[1..marker].Length)
                            .ToString();
                    }
                }
                else
                {
                    if (normalized[normalized.IndexOf('e') + 1] == '-')
                    {
                        //500.1e5 => 5.001e7
                        normalized = normalized.Replace(".", "");
                        normalized = normalized[..(normalized.IndexOf('e') + 2)];
                        normalized += (int.Parse(snapshot[(marker + 2)..]) - snapshot[1..marker].Length)
                            .ToString();
                    }
                    else
                    {
                        //500.1e-5 => 5.001e-3
                        normalized = normalized.Replace(".", "");
                        normalized = normalized[..(normalized.IndexOf('e') + 1)];
                        normalized += (int.Parse(snapshot[(marker + 1)..]) + snapshot[1..marker].Length)
                            .ToString();
                    }
                }

                if (normalized[..normalized.IndexOf('e')].Length != 1)
                    normalized = normalized.Insert(1, ".");
            }
        }
        else if (normalized.IndexOf('e') != 1)
        {
            //condition changed, so does the snapshot
            snapshot = normalized;
            
            //Remove leading zeros: 005e5 => 5e5
            while (normalized[0] == '0')
                normalized = normalized.Remove(0, 1);
            
            int exponent;

            exponent = int.Parse(snapshot[snapshot.IndexOf('e') + 1] != '-' ? snapshot[(marker + 1)..] : snapshot[(marker + 2)..]);
            
            normalized = normalized[..(marker + 1)];
            
            while (normalized[normalized.IndexOf('e') - 1] == '0')
            {
                normalized = normalized.Remove(normalized.IndexOf('e') - 1, 1);
                
                if (snapshot[snapshot.IndexOf('e') + 1] != '-')
                    exponent += 1;
            }
            
            normalized +=  exponent.ToString();
           
        }
        
        return normalized;
    }
    
    
}