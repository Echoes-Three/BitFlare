using System.Text;
using System.Text.RegularExpressions;

namespace BitFlare.Model.Conversion_Helper;

public static class ENotationDecoder
{
    private const string GroupCapturing =
        @"^(?<sign>-?)(?<fixedDigit>[1-9])(?:\.(?<varyingDigits>[0-9]+))?e-?(?<coefficient>[0-9]{1,2})$";
    
    public static string ToBaseTen(this string input)
    {
        var normalized = input.Normalize();
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

    private static string Normalization(this string input)
    {
        var normalized = input;
        
        if (input.Contains('+')) input = input.Replace("+", "");

        if (input.Contains('.'))
        {
            if (input.StartsWith('.') && input[input.IndexOf('e') + 1] == '-')
            {
                for (var digit = 1;; digit++)
                {
                    if (input[digit] != '0')
                    {
                        var leadingDigit = input[digit];
                        if (input[input.IndexOf('e') + 1] == '-')
                        {
                            normalized = normalized[normalized.IndexOf(leadingDigit)..(normalized.IndexOf('e') + 2)];
                            normalized += (int.Parse(input[(input.IndexOf('e') + 2)..]) + input.IndexOf(leadingDigit))
                                .ToString();
                            normalized = normalized.Insert(1, ".");
                            break;
                        }

                        normalized = normalized[normalized.IndexOf(leadingDigit)..(normalized.IndexOf('e') + 1)];
                        normalized += (int.Parse(input[(input.IndexOf('e') + 1)..]) - input.IndexOf(leadingDigit))
                            .ToString();
                        normalized = normalized.Insert(1, ".");
                        break;
                    }

                    if (input[digit] == 'e') normalized = "0.0e0";
                }
            }
        }
        else
        {
            
        }

        if (input.Contains('.'))
        {
            if (input[input.IndexOf('e') + 1] == '-')
            {
                
                //create a cleaner that gets the 0s before a norma digit in a negative(fraction) notation with "." and removes it
                //2.0455000e-6
                // 1
            }
        }

        return normalized;
    }
    
    
}