using System.ComponentModel;
using System.Text.RegularExpressions;
namespace BitFlare.Logic.Input_Logic;

public static class InputTypeDefinition
{
    private const string IntegerRegexFilter = "^-?(?:[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*)$";
    private const string FloatingPointRegexFilter = @"^-?(?:[0-9]+\.[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*\.[0-9]+)$";
    private const string ENotationRegexFilter = @"^-?[1-9](?:\.[0-9]+)?e-?[0-9]{1,3}$";

    public static TypeDefinition InputFilter(string inputBoxText)
    {
        var filters =
            (IntegerFilter: new Regex(IntegerRegexFilter),
            FloatingPointFilter: new Regex(FloatingPointRegexFilter),
            ENotationFilter: new Regex(ENotationRegexFilter));
        
        return inputBoxText switch
        {
            _ when filters.IntegerFilter.IsMatch(inputBoxText) => TypeDefinition.Integer,
            _ when filters.FloatingPointFilter.IsMatch(inputBoxText) => TypeDefinition.FloatingPoint,
            _ when filters.ENotationFilter.IsMatch(inputBoxText) => TypeDefinition.ENotation,
            _ => TypeDefinition.InvalidType
        };
    }
}