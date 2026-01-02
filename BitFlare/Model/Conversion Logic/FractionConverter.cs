namespace BitFlare.Logic;

public static class FractionConverter
{
    private static bool ReadyToCount { get; set; }
    private static bool IsFull { get; set; } = false;
    private static bool IsFinite{ get; set; } = false;
    private static int MantissaBitCounter { get; set; }
    private static string Converted { get; set; } = string.Empty;

    public static void BasicConverter(decimal parsed)
    {
        while (!IsFinite && !IsFull)
        {
            parsed *= 2;
            
            switch (decimal.Truncate(parsed))
            {
                case 1:
                    Converted += "1";
                    parsed -= decimal.Truncate(parsed);
                    ReadyToCount = true;
                    break;
                case 0:
                    Converted += "0";
                    break;
            }

            MantissaBitCounter += ReadyToCount ? 1 : 0;
            
            switch (parsed == 0, MantissaBitCounter == 27)
            {
                case (true, _):
                    IsFinite = true;
                    break;
                case (_, true):
                    IsFull = true;
                    break;
            }
        }
        
    }
}