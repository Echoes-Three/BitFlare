using System.Text.RegularExpressions;
using BitFlare.Logic;

namespace BitFlare.Model.Input_Logic;

public static class TypeClassification
{
    private const string IntegerRegexFilter = "^[+-]?(?:[0-9]{1,3}(?:,[0-9]{3})*|[0-9]+)$";
    private const string FloatingPointRegexFilter = @"^[+-]?(?:[0-9]{1,3}(?:,[0-9]{3})*|[0-9]+)(?:\.[0-9]+)?$";
    private const string ENotationRegexFilter = @"^[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)e[+-]?[0-9]{1,2}$";

    public static DefinedTypes ClassifiedType { get; private set; }

    public static void ClassifyInputType(string input)
    {
        var filters =
            (IntegerFilter: new Regex(IntegerRegexFilter),
            FloatingPointFilter: new Regex(FloatingPointRegexFilter),
            ENotationFilter: new Regex(ENotationRegexFilter));
        
        ClassifiedType = input switch
        {
            _ when filters.IntegerFilter.IsMatch(input) => DefinedTypes.Integer,
            _ when filters.FloatingPointFilter.IsMatch(input) => DefinedTypes.FloatingPoint,
            _ when filters.ENotationFilter.IsMatch(input) => DefinedTypes.ENotation,
            _ => DefinedTypes.InvalidType
        };
    }
}
