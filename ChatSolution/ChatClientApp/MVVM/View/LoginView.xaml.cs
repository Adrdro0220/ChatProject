using System.Windows;
using System.Windows.Controls;
using ChatClientApp.Core;
using ChatClientApp.MVVM.ViewModel;
using ChatProtocol;
using ChatProtocol.ChatHistory;
using User;

namespace ChatClientApp.MVVM.View;

public partial class LoginView 
{
    private Conn client = Client.GetConnectionInstance();
    public LoginView()
    {
        InitializeComponent();
    }
    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Pobierz wprowadzone dane z TextBoxów
        string login = LoginTextBox.Text;
        string password = PasswordBox.Password;
        
        Client.GetConnectionInstance().SendLoginRequest(login, password);
        // Przykładowa logika uwierzytelniania

        int initialPacketCount = client.PacketCount;
        for (;;)
        {
            if (initialPacketCount < client.PacketCount)
            {
                break;
            }
        }
        
        
        if (Client.GetConnectionInstance().acces)
        {
            ErrorTextBlockLogin.Text = "Zalogowano pomyślne!";
            client.ChatHistory = new History(login);
            MoveToMessages();
        }
        else
        {
            ErrorTextBlockLogin.Text = "Błąd logowania, Spróbuj ponownie.";
            PasswordBox.Password = "";
            LoginTextBox.Text = "";
        }
        
        
        
        
    }
    
    private void MoveToMessages()
    {
        var mainWindow = Application.Current.MainWindow;
            
        if (mainWindow.DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.CurrentView = new MessagesViewModel();
        }
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Application.Current.MainWindow;
            
        if (mainWindow.DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.CurrentView = new RegisterViewModel();
        }
    }
}