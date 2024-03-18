using System.Windows;
using System.Windows.Controls;
using ChatClientApp.Core;
using ChatClientApp.MVVM.ViewModel;
using ChatProtocol;
using User;

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

        Conn.Password = password;
        Conn.Username = username;
        Conn.Email = email;
        Client.GetConnectionInstance().SendRegisterRequest(username, password, email);
        
        int initialPacketCount = Client.GetConnectionInstance().PacketCount;
        for (;;)
        {
            if (initialPacketCount < Client.GetConnectionInstance().PacketCount)
            {
                break;
            }
        }

        RegisterResponse registerResponse = (RegisterResponse)Client.GetConnectionInstance()._handler.PacketRead;
        
        if (registerResponse.Message == "Registration successful")
        {
            ErrorTextBlockRegister.Text = registerResponse.Message;
            GoToLogin();
        }
        
        else
        {
            ErrorTextBlockRegister.Text = registerResponse.Message;
        }
        
        PasswordBox.Password = "";
        UsernameBox.Text = "";
        EmailBox.Text = "";
        
 
    }
    private void GoToLogin()
    {
        var mainWindow = Application.Current.MainWindow;
            
        if (mainWindow.DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.CurrentView = new LoginViewModel();
        }
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