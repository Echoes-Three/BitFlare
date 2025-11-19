using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BitFlare.Logic.Input_Logic;

public static class Canonicalization
{
    private static readonly string IntegerRegexFilter = "^-?(?:[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*)$";
    private static readonly string FloatingPointRegexFilter = @"^-?(?:[0-9]+\.[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*\.[0-9]+)$";
    private static readonly string ENotationRegexFilter = @"^-?[1-9](?:\.[0-9]+)?e-?[0-9]{1,3}$";

    public static string InputFilter(string inputBoxText)
    {
        var filters = (
            IntegerFilter: new Regex(IntegerRegexFilter),
            FloatingPointFilter: new Regex(FloatingPointRegexFilter),
            ENotationFilter: new Regex(ENotationRegexFilter)
            );
        
        return inputBoxText switch
        {
            _ when filters.IntegerFilter.IsMatch(inputBoxText) => "INTEGER",
            _ when filters.FloatingPointFilter.IsMatch(inputBoxText) => "FLOATING POINT",
            _ when filters.ENotationFilter.IsMatch(inputBoxText) => "E-NOTATION",
            _ => "INVALID"
        };
    }
}