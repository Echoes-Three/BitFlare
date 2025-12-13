using System.Windows;
using System.Windows.Controls;

namespace BitFlare.Model.Input_Logic;

public static class TextBoxCaretBehavior
{
    public static readonly DependencyProperty CaretIndexProperty =
        DependencyProperty.RegisterAttached(
            "CaretIndex",
            typeof(int),
            typeof(TextBoxCaretBehavior),
            new FrameworkPropertyMetadata(0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCaretIndexChanged));

    public static int GetCaretIndex(DependencyObject obj)
        => (int)obj.GetValue(CaretIndexProperty);

    public static void SetCaretIndex(DependencyObject obj, int value)
        => obj.SetValue(CaretIndexProperty, value);

    private static void OnCaretIndexChanged(
        DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox)
        {
            textBox.CaretIndex = (int)e.NewValue;
        }
    }
}
