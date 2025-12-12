using System.Collections.ObjectModel;
using System.Windows;
using BitFlare.Model;
using BitFlare.MVVM;

namespace BitFlare.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public RelayCommand CopyBinary { get; }
    public RelayCommand CopyHexadecimal { get; }

    public MainWindowViewModel()
    {
        CopyBinary = new RelayCommand(execute: _ => OnBinaryCopy());
        CopyHexadecimal = new RelayCommand(execute: _ => OnHexadecimalCopy());
    }

    public Action BinaryCopyAnimation;
    public Action HexadecimalCopyAnimation;
    
    private void OnBinaryCopy()
    {
        Clipboard.SetText(BinaryOutput);
        BinaryCopyAnimation?.Invoke();
    }
    private void OnHexadecimalCopy()
    {
        Clipboard.SetText(HexadecimalOutput);
        HexadecimalCopyAnimation?.Invoke();
    }
    
    private string _input = string.Empty;
    public string Input
    {
        get => _input;
        set
        {
            if (Equals(_input, value)) return;
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

    private string _binaryCopyButtonContent = "COPY";
    public string BinaryCopyButtonContent
    {
        get => _binaryCopyButtonContent;
        set
        {
            _binaryCopyButtonContent = value;
            OnPropertyChanged();
        }
    }
    
    private string _hexadecimalCopyButtonContent = "COPY";
    public string HexadecimalCopyButtonContent
    {
        get => _hexadecimalCopyButtonContent;
        set
        {
            _hexadecimalCopyButtonContent = value;
            OnPropertyChanged();
        }
    }
    
    private string _convertButtonContent = "CONVERT";
    public string ConvertButtonContent
    {
        get => _convertButtonContent;
        set
        {
            _convertButtonContent = value;
            OnPropertyChanged();
        }
    }


}