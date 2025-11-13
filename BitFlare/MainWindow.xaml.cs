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

    private async void BinaryCopyButton_Click(object sender, RoutedEventArgs e)
    {
        ClickAnimation(BinaryCopyButton);
        if (!string.IsNullOrEmpty(BinaryOutputTextBox.Text))
        {
            Clipboard.SetText(BinaryOutputTextBox.Text);
            BinaryCopyButton.Content = "COPIED";
            await Task.Delay(1000);
            BinaryCopyButton.Content = "COPY";
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
        /*Warning level UI helper*/
        var validator = new InputValidationState();
        
        (InputBoxBorder.BorderBrush, IsValidToConvert) = validator.IsValid(InputBox.Text) switch
        {
            true => (InputBoxBorder.BorderBrush = Brushes.White, true),
            false => (InputBoxBorder.BorderBrush = Brushes.Yellow, false)
        };
        
        /*Sanitizer*/
        (InputBox.Text, InputBox.CaretIndex) =
            InputSanitizer.Sanitize(InputBox.Text, InputBox.CaretIndex);
        
        /*Enables conversion on textbox blur*/
        if (e.Key == Key.Enter)
        {
            Keyboard.ClearFocus();
            
            switch (validator.IsValid(InputBox.Text) && Canonicalization.InputFilter(InputBox.Text) != "INVALID")
            {
                case true:
                    InputBoxBorder.BorderBrush = Brushes.White;
                    InvalidCharacterWarning.Visibility = Visibility.Hidden;
                    IsValidToConvert = true;
                    break;
                case false:
                    InputBoxBorder.BorderBrush = Brushes.Red;
                    InvalidCharacterWarning.Visibility = Visibility.Visible;
                    IsValidToConvert = false;
                    break;
            }

            BinaryOutputTextBox.Text = Canonicalization.InputFilter(InputBox.Text);
            HexadecimalOutputTextBox.Text = validator.IsValid(InputBox.Text) ? "true" : "false";
        }
        
        /*Updates the output text title*/
        OutputBoxDynamicTitle.Text = OutputTitleUpdater.UpdateTitle(InputBox.Text);
    }
    
    private void ConvertButton_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        ClickAnimation(ConvertButton);

        // (BinaryOutputTextBox.Text, OutputBoxDynamicTitle.Text ) = (IntegerConverter.BasicConverter(InputBox.Text), OutputTitleUpdater.UpdateTitleWithBit());
    }

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
