using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using BitFlare.Logic;
using BitFlare.Logic.Input_Logic;
using BitFlare.Model.Conversion_Helper;
using BitFlare.Model.Input_Logic;
using BitFlare.MVVM;

namespace BitFlare.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public RelayCommand CopyBinary { get; }
    public RelayCommand CopyHexadecimal { get; }
    public RelayCommand ConvertInput { get; }
    
    public MainWindowViewModel()
    {
        CopyBinary = new RelayCommand(execute: _ => OnBinaryCopy());
        CopyHexadecimal = new RelayCommand(execute: _ => OnHexadecimalCopy());
        ConvertInput = new RelayCommand(execute: _ => OnConvertInput(), canExecute: _ => CanConvert() );
    }

    public Action BinaryCopyAnimation;
    public Action HexadecimalCopyAnimation;
    public Action ConvertAnimation;

    private bool CanConvert() =>
         !string.IsNullOrEmpty(Input) && Sanitizer();
    
    private bool Sanitizer()
    {
        (Input, CaretIndex) = InputSanitizer.Sanitizers(Input, CaretIndex);
        ConversionUtilities.Initializers(Input);
        InputTypeDefinition.InputFilter(Input);
        
        if (InputValidation.IsValid(Input) && InputTypeDefinition.Current != TypeDefinition.InvalidType)
            /*If Input has an invalid character OR is not a valid type*/
        {
            if (IsWithinLimit()) /*Checks if it's withing the limits of the conversion for its type*/
            {
                InputBoxBorder = Brushes.White;
                WarningText = "INVALID CHARACTER OR FORMAT";
                _warningVisibility = Visibility.Hidden;
                IsValidToConvert = true;
            }
            else /*If its not withing the limits gives the appropriate waring*/
            {
                InputBoxBorder = Brushes.Yellow;
                WarningText = DealsWithMagnitudeLimit();
                _warningVisibility = Visibility.Visible;
                IsValidToConvert = false;
                
            }
        }
        else /*If Input has an invalid character OR is not a valid type*/
        {
            InputBoxBorder = Brushes.Yellow;
            WarningText = "INVALID CHARACTER OR FORMAT";
            _warningVisibility = Visibility.Visible;
            IsValidToConvert = false;
        }
        
        return IsValidToConvert;
    }

    private static string DealsWithMagnitudeLimit()
    {
        var limitMessage = "";
        if (ConversionUtilities.IsNegative)
        {
            if (InputTypeDefinition.Current == TypeDefinition.Integer)
                limitMessage = "MIN NEGATIVE INTEGER VALUE IS -2,147,483,648";
            //else
                //Add limit to negative fractions
        }
        else
        {
            if (InputTypeDefinition.Current == TypeDefinition.Integer)
                limitMessage = "MAX POSITIVE INTEGER VALUE IS 4,294,967,295";
            //else
                //Add limit to positive fractions
        }

        return limitMessage;
    }

    private static bool IsWithinLimit()
    {
        var isWithinLimit = false;
        var parsedInput = ConversionUtilities.ReadyToConvert;
        switch (InputTypeDefinition.Current)
        {
            case TypeDefinition.Integer:
                isWithinLimit = ConversionUtilities.IsNegative switch
                {
                    true when parsedInput <= /*-*/2_147_483_648 => true,
                    false when parsedInput <= 4_294_967_295 => true,
                    _ => isWithinLimit
                };
                break;
        }
        
        return isWithinLimit;
    }
    
    private void OnConvertInput()
    {
        ConvertAnimation.Invoke();
        BinaryOutput = ConverterPointer.PointerCaller(Input);
        OutputDynamicTitle = OutputTitleUpdater.UpdateTitle(Input, ConversionUtilities.BitMagnitude);
    }
    private void OnBinaryCopy()
    {
        Clipboard.SetText(BinaryOutput);
        BinaryCopyAnimation.Invoke();
    }
    private void OnHexadecimalCopy()
    {
        Clipboard.SetText(HexadecimalOutput);
        HexadecimalCopyAnimation.Invoke();
    }
    
    ////////////// Properties

    private bool _isValidToConvert;

    public bool IsValidToConvert
    {
        get => _isValidToConvert;
        set
        {
            if (!Equals(_isValidToConvert, value)) return;
            _isValidToConvert = value;
            OnPropertyChanged();
        }
    }

    
    private Visibility _warningVisibility = Visibility.Hidden;
    public Visibility WarningVisibility
    {
        get => _warningVisibility;
        set
        {
            if (!Equals(_warningVisibility, value)) return;
            _warningVisibility = value;
            OnPropertyChanged();
        }
    }

    private Brush _inputBoxBorder = Brushes.White;
    public Brush InputBoxBorder
    {
        get => _inputBoxBorder;
        set
        {
            if (!Equals(_inputBoxBorder, value)) return;
            _inputBoxBorder = value;
            OnPropertyChanged();
        }
    }
    
    private string _warningText = "INVALID CHARACTER OR FORMAT";
    public string WarningText
    {
        get => _warningText;
        set
        {
            if (!Equals(_warningText, value)) return;
            _warningText = value;
            OnPropertyChanged();
        }
    }
    
    private int _caretIndex;
    public int CaretIndex
    {
        get => _caretIndex;
        set
        {
            if (!Equals(_caretIndex, value)) return;
            _caretIndex = value;
            OnPropertyChanged();
        }
    }
    
    private string _outputDynamicTitle = "OUTPUT";
    public string OutputDynamicTitle
    {
        get => _outputDynamicTitle;
        set
        {
            _outputDynamicTitle = value;
            OnPropertyChanged();
        }
    }

    private string _input = "";
    public string Input
    {
        get => _input;
        set
        {
            if (!Equals(_input, value)) return;
            _input = value;
            OnPropertyChanged();
        }
    }

    private string _binaryOutput = string.Empty;
    public string BinaryOutput
    {
        get => _binaryOutput;
        set
        {
            _binaryOutput = value;
            OnPropertyChanged();
        }
    }

    private string _hexadecimalOutput = string.Empty;
    public string HexadecimalOutput
    {
        get => _hexadecimalOutput;
        set
        {
            _hexadecimalOutput = value;
            OnPropertyChanged();
        }
    }
}