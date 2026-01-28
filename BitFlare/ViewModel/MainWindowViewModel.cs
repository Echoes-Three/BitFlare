using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BitFlare.Logic;
using BitFlare.Model.Conversion_Helper;
using BitFlare.Model.Input_Logic;
using BitFlare.MVVM;

namespace BitFlare.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public RelayCommand CopyBinary { get; }
    public RelayCommand CopyHexadecimal { get; }
    public RelayCommand ConvertInput { get; }
    public RelayCommand IntegerSwitch { get; }
    public RelayCommand Ieee754Switch { get; }
    
    public MainWindowViewModel()
    {
        CopyBinary = new RelayCommand(execute: _ => OnBinaryCopy());
        CopyHexadecimal = new RelayCommand(execute: _ => OnHexadecimalCopy());
        ConvertInput = new RelayCommand(execute: _ => OnConvertInput(), canExecute: _ => IsValidToConvert );
        IntegerSwitch = new RelayCommand(execute: _ => OnIntegerSwitch());
        Ieee754Switch = new RelayCommand(execute: _ => OnIeee754Switch());
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
            
            if (TypeClassification.ClassifiedType != DefinedTypes.InvalidType)
            {
                var isENotation = TypeClassification.ClassifiedType == DefinedTypes.ENotation ||
                                  TypeClassification.ClassifiedType == DefinedTypes.IntENotation;
                if (isENotation)
                    input = ENotationUtilities.ToNormalized(input);
                
                if (input.IsWithinLimit())
                {
                    switch (TypeClassification.ClassifiedType)
                     {
                         case DefinedTypes.FloatingPoint:
                         case DefinedTypes.Integer:
                             ConversionUtilities.Initializers(input);
                             break;
                         case DefinedTypes.IntENotation:
                         case DefinedTypes.ENotation:
                             ConversionUtilities.Initializers(ENotationUtilities.ToBaseTen(input));
                             break;
                     }
                    IsValid();
                    WarningText = "INVALID CHARACTER!";
                }
                else
                {
                    IsInvalid();
                    WarningText = CheckMagnitudeLimit(input);
                }
            }
            else
            {
                IsInvalid();
                WarningText = "INVALID NUMBER FORMAT!";
            }
        }
        else
        {
            IsInvalid();
            WarningText = "INVALID CHARACTER!";
        }
    }
    
    private void IsInvalid()
    {
        InputBoxBorder = Brushes.Yellow;
        WarningVisibility = Visibility.Visible;
        IsValidToConvert = false;
    }
    private void IsValid()
    {
        InputBoxBorder = Brushes.White;
        WarningVisibility = Visibility.Hidden;
        IsValidToConvert = true;
    }
    
    private static string CheckMagnitudeLimit(string input)
    {
        var limitMessage = "";
        switch (TypeClassification.ClassifiedType)
        {
            case DefinedTypes.IntENotation:
            case DefinedTypes.Integer:
                limitMessage = input.StartsWith('-')
                    ? "MIN NEGATIVE INTEGER VALUE IS -2,147,483,648 OR -2.14e9"
                    : "MAX POSITIVE INTEGER VALUE IS 4,294,967,295 OR 4.29e9";
                break;
            
            case DefinedTypes.ENotation:
                limitMessage = "INVALID E-NOTATION RANGE: ±1.4e-45 TO ±3.4e38";
                break;
            
            case DefinedTypes.FloatingPoint:
                //
                break;
        }
        
        return limitMessage;
    }
    
    private void OnConvertInput()
    {
        ConvertAnimation.Invoke();
        BinaryOutput = ConverterPointer.CallPointer();
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

    private void OnIntegerSwitch()
    {
        if (IsInteger)
        {
        }
        else
        {
            IntegerClickAnimation.Invoke();
            IsInteger = true;
            IsIeee754 = false;
        }
    }

    private void OnIeee754Switch()
    {
        if (IsIeee754)
        {
        }
        else
        {
            Ieee754ClickAnimation.Invoke();
            IsIeee754 = true;
            IsInteger = false;
        }
    }
    
    ////////////// Properties

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
                InputBoxBorder = Brushes.White;
                WarningVisibility = Visibility.Hidden;
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
    
    private string _binaryOutput = "testing";
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

    
    private string _warningText = "INVALID CHARACTER OR FORMAT";
    public string WarningText
    {
        get => _warningText;
        set
        {
            if (Equals(_warningText, value)) return;
            _warningText = value;
            OnPropertyChanged();
        }
    }


    private Visibility _warningVisibility = Visibility.Hidden;
    public Visibility WarningVisibility
    {
        get => _warningVisibility;
        set
        {
            if (Equals(_warningVisibility, value)) return;
            _warningVisibility = value;
            OnPropertyChanged();
        }
    }


    private Brush _inputBoxBorder = (Brush)Application.Current.FindResource("WhiteBrush");
    public Brush InputBoxBorder
    {
        get => _inputBoxBorder;
        set
        {
            if (Equals(_inputBoxBorder, value)) return;
            _inputBoxBorder = value;
            OnPropertyChanged();
        }
    }

    
    private bool _isValidToConvert;
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
}