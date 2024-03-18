using System.IO;
using System.Collections.ObjectModel;
using ChatClientApp.Core;
using ChatProtocol.ChatHistory;
using User;

namespace ChatClientApp.MVVM.ViewModel
{
    public class MessagesViewModel : ObservableObject
    {
        private Conn conn = Client.GetConnectionInstance();
        private ObservableCollection<string> _messages;
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        private FileSystemWatcher fileWatcher;

        public MessagesViewModel()
        {
            Messages = new ObservableCollection<string>(conn.ChatHistory.ReadHistory());

            fileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(History.filePath),
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = Path.GetFileName(History.filePath)
            };

            fileWatcher.Changed += OnChanged;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == History.filePath)
            {
                Messages = new ObservableCollection<string>(conn.ChatHistory.ReadHistory());
            }
        }
    }
}