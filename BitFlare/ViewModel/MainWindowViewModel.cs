using System.Windows;
using BitFlare.Model.Conversion_Helper;
using BitFlare.Model.Conversion_Logic;
using BitFlare.Model.Input_Logic;
using BitFlare.MVVM;

namespace BitFlare.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public RelayCommand CopyBinary { get; }
    public RelayCommand CopyHexadecimal { get; }
    public RelayCommand ConvertInput { get; }
    public RelayCommand IntegerToggle { get; }
    public RelayCommand Ieee754Toggle { get; }
    
    public MainWindowViewModel()
    {
        CopyBinary = new RelayCommand(execute: _ => OnBinaryCopy());
        CopyHexadecimal = new RelayCommand(execute: _ => OnHexadecimalCopy());
        ConvertInput = new RelayCommand(execute: _ => OnConvertInput(), canExecute: _ => IsValidToConvert );
        IntegerToggle = new RelayCommand(execute: _ => OnIntegerToggle());
        Ieee754Toggle = new RelayCommand(execute: _ => OnIeee754Toggle());
    }

    public Action BinaryCopyAnimation;
    public Action HexadecimalCopyAnimation;
    public Action ConvertAnimation;
    public Action IntegerClickAnimation;
    public Action Ieee754ClickAnimation;
    
    private void InputValidation(string input)
    {
        if (input.HasOnlyValidChars())
        {
            TypeClassification.ClassifyInputType(input, IsInteger);
            var classifiedType = TypeClassification.ClassifiedType;
            
            if (InputMachesToggle(classifiedType))
            {
                if (classifiedType is DefinedTypes.ENotation or DefinedTypes.IntENotation)
                    input = ENotationUtilities.ToNormalized(input);
                
                if (input.IsWithinLimit())
                {
                    switch (classifiedType)
                     {
                         case DefinedTypes.Integer:
                             ConversionUtilities.Initializers(input);
                             break;
                         case DefinedTypes.IntENotation:
                             ConversionUtilities.Initializers(ENotationUtilities.ToBaseTen(input));
                             break;
                         case DefinedTypes.ENotation:
                             ConversionUtilities.Initializers(input);
                             break;
                     }

                    (IsValidToConvert, WarnTooltip) = (true, false);
                }
                else
                    (IsValidToConvert, WarnTooltip) = (false, true);
            }
            else
                (IsValidToConvert, WarnTooltip) = (false, true);
        }
        else
            IsValidToConvert = false;
    }

    private bool InputMachesToggle(DefinedTypes classifiedType)
    {
        var isvalid = false;
        switch (classifiedType)
        {
            case DefinedTypes.IntENotation or DefinedTypes.Integer when IsInteger:
            case DefinedTypes.ENotation when IsIeee754:
                isvalid = true;
                break;
        }

        return isvalid;
    }
    
    private void OnConvertInput()
    {
        ConvertAnimation.Invoke();
        (BinaryOutput, HexadecimalOutput) = ConverterPointer.CallPointer();
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
    
    private void OnIntegerToggle()
    {
        if (IsInteger)
        {
        }
        else
        {
            IntegerClickAnimation.Invoke();
            
            (IsInteger,IsIeee754) = (true, false);
            TooltipBlockOne = "-2,147,483,648 OR -2.14E9";
            TooltipBlockTwo = "4,294,967,295 OR 4.29E9";

            if (!string.IsNullOrEmpty(Input))
                InputValidation(Input);
        }
    }

    private void OnIeee754Toggle()
    {
        if (IsIeee754)
        {
        }
        else
        {
            Ieee754ClickAnimation.Invoke();
            
            (IsInteger,IsIeee754) = (false, true);
            TooltipBlockOne = "±1.4e-45 TO ±3.4e38";
            TooltipBlockTwo = "";
            
            if (!string.IsNullOrEmpty(Input))
                InputValidation(Input);
        }
    }
    
    ////////////// Properties
    
    private string _tooltipBlockOne = "-2,147,483,648 OR -2.14E9";
    public string TooltipBlockOne
    {
        get => _tooltipBlockOne;
        set
        {
            _tooltipBlockOne = value;
            OnPropertyChanged();
        }
    }
    
    private string _tooltipBlockTwo = "4,294,967,295 OR 4.29E9";
    public string TooltipBlockTwo
    {
        get => _tooltipBlockTwo;
        set
        {
            _tooltipBlockTwo = value;
            OnPropertyChanged();
        }
    }

    
    private bool _isInteger = true;
    public bool IsInteger
    {
        get => _isInteger;
        set
        {
            if (Equals(_isInteger, value)) return;
            _isInteger = value;
            OnPropertyChanged();
        }
    }

    private bool _isIeee754;
    public bool IsIeee754
    {
        get => _isIeee754;
        set
        {
            if (Equals(_isIeee754, value)) return;
            _isIeee754 = value;
            OnPropertyChanged();
        }
    }

    
    private string _input;
    public string Input
    {
        get => _input;
        set
        {
            if (Equals(_input, value)) return;
           
            var (sanitized, newCaret) = InputSafety.Sanitize(value, CaretIndex);

            _input = sanitized;
            CaretIndex = newCaret;
            
            if (string.IsNullOrEmpty(_input))
            {
                IsValidToConvert = false;
                return;
            }

            InputValidation(_input);
            OnPropertyChanged();
        }
    }


    private int _caretIndex;
    public int CaretIndex
    {
        get => _caretIndex;
        set
        {
            if (Equals(_caretIndex, value)) return;
            _caretIndex = value;
            OnPropertyChanged();
        }
    }
    
    private string _binaryOutput;
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
    
    private bool _isValidToConvert = true;
    public bool IsValidToConvert
    {
        get => _isValidToConvert;
        set
        {
            if (Equals(_isValidToConvert, value)) return;
            _isValidToConvert = value;
            OnPropertyChanged();
            ConvertInput.RaiseCanExecuteChanged();
        }
    }

    private bool _warnTooltip;
    public bool WarnTooltip
    {
        get => _warnTooltip;
        set
        {
            _warnTooltip = value;
            if(string.IsNullOrEmpty(Input))
                _warnTooltip = false;
            OnPropertyChanged();
        }
    }

}