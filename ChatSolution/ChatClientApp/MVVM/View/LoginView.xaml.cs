using System.Windows;
using System.Windows.Controls;

namespace ChatClientApp.MVVM.View;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }
    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Pobierz wprowadzone dane z TextBoxów
        string email = EmailTextBox.Text;
        string password = PasswordBox.Password;

        // Przykładowa logika uwierzytelniania
        if (AuthenticateUser(email, password))
        {
            MessageBox.Show("Zalogowano pomyślnie!");
            // Tutaj możesz otworzyć nowe okno lub wykonać inne operacje po zalogowaniu
        }
        else
        {
            MessageBox.Show("Błąd logowania. Sprawdź dane i spróbuj ponownie.");
        }
    }

    private bool AuthenticateUser(string email, string password)
    {
        // Tutaj zaimplementuj logikę uwierzytelniania
        // Możesz sprawdzić dane w bazie danych, pliku konfiguracyjnym itp.
        // W tym przykładzie zawsze zwracamy true dla celów demonstracyjnych
        return true;
    }
}