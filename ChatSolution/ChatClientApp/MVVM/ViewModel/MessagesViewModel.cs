using System.IO;
using ChatClientApp.Core;
using User;

namespace ChatClientApp.MVVM.ViewModel;

public class MessagesViewModel: ObservableObject
{
    private Conn conn = Client.GetConnectionInstance();
    private List<string> _messages;
    public List<string> Messages
    {
        get { return _messages; }
        set
        {
            _messages = value;
            OnPropertyChanged("Messages");
        }
    }

    public MessagesViewModel()
    {
        Messages = new List<string>(conn.ChatHistory.ReadHistory());

    }
    public void AddMessage(string message)
    {
        OnPropertyChanged("Messages");
        Messages.Add(message);
    }
    
    
}