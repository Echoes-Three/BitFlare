using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BitFlare.Logic.Input_Logic;

public static class Canonicalization
{
    private static readonly string IntegerFilter = "^-?(?:[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*)$";
    private static readonly string FloatingPointFilter = @"^-?(?:[0-9]+\.[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*\.[0-9]+)$";
    private static readonly string ENotationFilter = @"^-?[1-9](?:\.[0-9]+)?e-?[0-9]{1,3}$";

    public static string InputFilter(string inputBoxText)
    {
        var filters = (new Regex(IntegerFilter), new Regex(FloatingPointFilter), new Regex(ENotationFilter));
        
        return inputBoxText switch
        {
            var filterType when filters.Item1.IsMatch(inputBoxText) => "INTEGER",
            var filterType when filters.Item2.IsMatch(inputBoxText) => "FLOATING POINT",
            var filterType when filters.Item3.IsMatch(inputBoxText) => "E-NOTATION",
            _ => "INVALID"
        };
    }
}