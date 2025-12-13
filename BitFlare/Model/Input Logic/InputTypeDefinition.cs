using System.Text.RegularExpressions;
using BitFlare.Logic;

namespace BitFlare.Model.Input_Logic;

public static class InputTypeDefinition
{
    private const string IntegerRegexFilter = "^-?(?:[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*)$";
    private const string FloatingPointRegexFilter = @"^-?(?:[0-9]+\.[0-9]+|[0-9]{1,3}(?:,[0-9]{3})*\.[0-9]+)$";
    private const string ENotationRegexFilter = @"^-?[1-9](?:\.[0-9]+)?e-?[0-9]{1,3}$";

    public static TypeDefinition Current { get; private set; }

    public static void InputFilter(string input)
    {
        var filters =
            (IntegerFilter: new Regex(IntegerRegexFilter),
            FloatingPointFilter: new Regex(FloatingPointRegexFilter),
            ENotationFilter: new Regex(ENotationRegexFilter));
        
        Current = input switch
        {
            _ when filters.IntegerFilter.IsMatch(input) => TypeDefinition.Integer,
            _ when filters.FloatingPointFilter.IsMatch(input) => TypeDefinition.FloatingPoint,
            _ when filters.ENotationFilter.IsMatch(input) => TypeDefinition.ENotation,
            _ => TypeDefinition.InvalidType
        };
    }
}