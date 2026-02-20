using System.Text.RegularExpressions;
using BitFlare.Model.Conversion_Logic;

namespace BitFlare.Model.Input_Logic;

public static class TypeClassification
{
    private const string IntegerRegexFilter = "^[+-]?(?:[0-9]{1,3}(?:,[0-9]{3})*|[0-9]+)$";
    private const string ENotationRegexFilter = @"^[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)e[+-]?[0-9]{1,2}$";
    private const string IntENotationRegexFilter = @"^[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)e[+]?[0-9]{1,2}$";

    public static DefinedTypes ClassifiedType { get; private set; }

    public static void ClassifyInputType(string input, bool isInteger)
    {
        var filters =
            (IntegerFilter: new Regex(IntegerRegexFilter),
            ENotationFilter: new Regex(ENotationRegexFilter),
            IntegerENotationFilter: new Regex(IntENotationRegexFilter));
        
        ClassifiedType = isInteger
            ? input switch
            {
                _ when filters.IntegerFilter.IsMatch(input) => DefinedTypes.Integer,
                _ when filters.IntegerENotationFilter.IsMatch(input) => DefinedTypes.IntENotation,
                _ => DefinedTypes.InvalidType
            }
            : input switch
            {
                _ when filters.IntegerFilter.IsMatch(input) => DefinedTypes.Integer,
                _ when filters.ENotationFilter.IsMatch(input) => DefinedTypes.ENotation,
                _ => DefinedTypes.InvalidType
            };
    }
}
