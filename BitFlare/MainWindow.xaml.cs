using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BitFlare.Logic;
using BitFlare.Logic.Input_Logic;

namespace BitFlare;


public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
    }

    private static async void ClickAnimation(Button button)
    {
        button.Margin = button.Margin with { Top = 10 };
        await Task.Delay(100);
        button.Margin = button.Margin with { Top = 0 };
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed) DragMove();
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private async void OutputCopyButton_Click(object sender, RoutedEventArgs e)
    {
        ClickAnimation(OutputCopyButton);
        if (!string.IsNullOrEmpty(BinaryOutputTextBox.Text))
        {
            Clipboard.SetText(BinaryOutputTextBox.Text);
            OutputCopyButton.Content = "COPIED";
            await Task.Delay(1000);
            OutputCopyButton.Content = "COPY";
        }
    }

    private async void HexadecimalCopyButton_Click(object sender, RoutedEventArgs e)
    {
        ClickAnimation(HexadecimalCopyButton);
        if (!string.IsNullOrEmpty(HexadecimalOutputTextBox.Text))
        {
            Clipboard.SetText(HexadecimalOutputTextBox.Text);
            HexadecimalCopyButton.Content = "COPIED";
            await Task.Delay(1000);
            HexadecimalCopyButton.Content = "COPY";
        }
    }

    private bool _isValidToConvert = false;

    public bool IsValidToConvert
    {
        get => _isValidToConvert;
        set
        {
            if (_isValidToConvert != value)
            {
                _isValidToConvert = value;
                OnPropertyChanged(nameof(IsValidToConvert));
            }
        }
    }


    private void Utilities_OnKeyUp(object sender, KeyEventArgs e)
    {
        /*Sanitizer*/
        (InputBox.Text, InputBox.CaretIndex) =
            InputSanitizer.Sanitize(InputBox.Text, InputBox.CaretIndex);

        if (InputValidation.IsValid(InputBox.Text) &&
            (Canonicalization.InputFilter(InputBox.Text) != "INVALID" || string.IsNullOrEmpty(InputBox.Text)))
        {
            if ((Canonicalization.InputFilter(InputBox.Text) == "INTEGER"
                 && InputConverterHelper.GetParsed(InputBox.Text) <= uint.MaxValue) ||
                string.IsNullOrEmpty(InputBox.Text))
            {
                InputBoxBorder.BorderBrush = Brushes.White;
                InvalidCharacterWarning.Text = "INVALID CHARACTER OR FORMAT";
                InvalidCharacterWarning.Visibility = Visibility.Hidden;
                IsValidToConvert = true;
            }
            else
            {
                InputBoxBorder.BorderBrush = Brushes.Yellow;
                InvalidCharacterWarning.Text = "MAX POSITIVE INTEGER VALUE IS 4,294,967,295";
                InvalidCharacterWarning.Visibility = Visibility.Visible;
                IsValidToConvert = false;
            }
        }
        else
        {
            InputBoxBorder.BorderBrush = Brushes.Yellow;
            InvalidCharacterWarning.Text = "INVALID CHARACTER OR FORMAT";
            InvalidCharacterWarning.Visibility = Visibility.Visible;
            IsValidToConvert = false;
        }

        /*Updates the output text title*/
        OutputBoxDynamicTitle.Text = OutputTitleUpdater.UpdateTitle(InputBox.Text);

        if (e.Key == Key.Enter)
        {
            Keyboard.ClearFocus();
            ConversionAction();
        }
    }

    private void ConvertButton_Click(object sender, RoutedEventArgs routedEventArgs) =>
        ConversionAction();
    
    private async void ConversionAction()
    {
        ClickAnimation(ConvertButton);
        ConvertButton.Content = "...";
        await Task.Delay(1000);
        ConvertButton.Content = "CONVERT";

        BinaryOutputTextBox.Text =
            InputConverterHelper.ConverterCaller(InputBox.Text);
        OutputBoxDynamicTitle.Text =
            OutputTitleUpdater.UpdateTitleWithBit(InputBox.Text, InputConverterHelper.GetMagnitude(InputBox.Text));
    }

/* Select input box text on focus*/
    private void InputBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.OriginalSource is TextBox textBox)
        {
            textBox.Dispatcher.BeginInvoke(() => textBox.SelectAll());
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
