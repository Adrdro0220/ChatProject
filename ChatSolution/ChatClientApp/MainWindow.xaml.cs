using System.Windows;
using System.Windows.Input;
using ChatClientApp.Core;
using User;
namespace ChatClientApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
<<<<<<< HEAD

}
=======
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }
}
>>>>>>> 76aec74a5652788737cd89ea6400a7aea357c0b6
