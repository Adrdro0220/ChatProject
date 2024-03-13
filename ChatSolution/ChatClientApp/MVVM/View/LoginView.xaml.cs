using System.Windows;
using System.Windows.Controls;
using ChatClientApp.Core;
using ChatClientApp.MVVM.ViewModel;
using ChatProtocol;

namespace ChatClientApp.MVVM.View;

public partial class LoginView 
{
    
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

        int initialPacketCount = Client.GetConnectionInstance().PacketCount;
        for (;;)
        {
            if (initialPacketCount < Client.GetConnectionInstance().PacketCount)
            {
                break;
            }
        }
        if (Client.GetConnectionInstance().acces)
        {
            MessageBox.Show("Zalogowano pomyślnie!");
            
        }
        else
        {
            MessageBox.Show("Błąd logowania. Sprawdź dane i spróbuj ponownie.");
        }
        PasswordBox.Password = "";
        LoginTextBox.Text = "";
        
        
        
    }
    
}