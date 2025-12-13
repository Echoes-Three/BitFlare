using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BitFlare.ViewModel;

namespace BitFlare.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel();
        DataContext = viewModel;
        viewModel.BinaryCopyAnimation = BinaryCopyButtonAnimation;
        viewModel.HexadecimalCopyAnimation = HexadecimalCopyButtonAnimation;
        viewModel.ConvertAnimation = ConvertAnimation;
    }
    
    private void Close_Click(object sender, RoutedEventArgs e) => Close();
    
    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    
    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed) DragMove();
    }
    
    private static async void ClickAnimation(Button button)
    {
        button.Margin = button.Margin with { Top = 10 };
        await Task.Delay(100);
        button.Margin = button.Margin with { Top = 0 };
    }
    
    private async void BinaryCopyButtonAnimation()
    {
        ClickAnimation(BinaryCopyButton);
        BinaryCopyButton.Content = "COPIED";
        await Task.Delay(1000);
        BinaryCopyButton.Content = "COPY";
    }

    private async void HexadecimalCopyButtonAnimation()
    {
        ClickAnimation(HexadecimalCopyButton);
        HexadecimalCopyButton.Content = "COPIED";
        await Task.Delay(1000);
        HexadecimalCopyButton.Content = "COPY";
    }
    
    private void Utilities_OnKeyUp(object sender, KeyEventArgs e)
    {
        
        if (e.Key == Key.Enter)
            Keyboard.ClearFocus();
    }
    
    private async void ConvertAnimation()
    {
        ClickAnimation(ConvertButton);
        ConvertButton.Content = "...";
        await Task.Delay(1000);
        ConvertButton.Content = "CONVERT";
    }
    
    /* Select input box text on focus*/
    private void InputBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.OriginalSource is TextBox textBox)
        {
            textBox.Dispatcher.BeginInvoke(() => textBox.SelectAll());
        }
    }
    
}
