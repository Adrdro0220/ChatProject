using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        DataContext = new MessagesViewModel();
    }

    private void SendMessage(object sender, RoutedEventArgs e)
    {
        if (MessageTextBox.Text == "")
        {
            return;
        }
        
        try
        {
            client.ChatHistory.AppendLineToFile(MessageTextBox.Text);
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
        
        DataContext = new MessagesViewModel();
        
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
        MessageTextBox.Text = "";
    }

    private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SendMessage(sender, e);
        }
    }
}
