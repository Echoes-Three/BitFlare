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
using BitFlare.Logic.Input_Logic;

namespace BitFlare;



/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

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

    private void WarningLevel_OnKeyUp(object sender, KeyEventArgs e)
    {
        var validator = new InputValidationState();
        
        if (!validator.IsValid(InputBox.Text))
            InputBoxBorder.BorderBrush = Brushes.Yellow;
        else
            InputBoxBorder.BorderBrush = Brushes.White;

        (InputBox.Text, InputBox.CaretIndex) = InputCanonicalization.RepeatedCharacterNormalizer(InputBox.Text, InputBox.CaretIndex);
        (InputBox.Text, InputBox.CaretIndex) = InputCanonicalization.IsDoubled(InputBox.Text, InputBox.CaretIndex);
    }
}