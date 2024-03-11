using System.Windows;
using System.Windows.Controls;

namespace ChatClientApp.MVVM.View;

public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();
    }
    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameBox.Text;
        string password = PasswordBox.Password;
        string email = EmailBox.Text;

        // Add your registration logic here
    }
    private void EmailBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null)
        {
            if (!IsValidEmail(textBox.Text))
            {
                MessageBox.Show("Proszę wprowadzić prawidłowy adres e-mail.");
            }
        }
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}