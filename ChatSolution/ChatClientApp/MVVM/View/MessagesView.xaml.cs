using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ChatClientApp.Core;
using ChatClientApp.MVVM.ViewModel;
using User;

namespace ChatClientApp.MVVM.View;
public partial class MessagesView : UserControl
{
    Conn client = Client.GetConnectionInstance();
    public MessagesView()
    {
        InitializeComponent();
        this.DataContext = new MessagesViewModel();
    }

    private void SendMessage(object sender, RoutedEventArgs e)
    {
        try
        {
            client.ChatHistory.AppendLineToFile(MessageTextBox.Text);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
        this.DataContext = new MessagesViewModel();
        MessageTextBox.Clear();

        try
        {
            if (messagesListBox.Items.Count > 0)
            {
                messagesListBox.ScrollIntoView(messagesListBox.Items[messagesListBox.Items.Count - 1]);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}
