using ChatClientApp.Core;

namespace ChatClientApp.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand MessagesViewCommand { get; set; }
    public RelayCommand LoginViewCommand { get; set; }
    public RelayCommand RegisterViewCommand { get; set; }
    
    public HomeViewModel HomeVM { get; set; }
    public MessagesViewModel MessagesVM { get; set; }
    private object _currentview;
    public object Currentview {
        get
        {
            return _currentview;
        }
        set
        {
            _currentview = value;
            OnPropertyChanged();
        }
    }
    
    
    public MainViewModel()
    {
        HomeVM = new HomeViewModel();
        MessagesVM = new MessagesViewModel();
        Currentview = HomeVM;
        HomeViewCommand = new RelayCommand(o =>
            {
            Currentview = HomeVM;
        });
        MessagesViewCommand = new RelayCommand(o =>
            {
            Currentview = MessagesVM;
        });
        LoginViewCommand = new RelayCommand(o =>
            {
            Currentview = new LoginViewModel();
        });
        RegisterViewCommand = new RelayCommand(o =>
            {
            Currentview = new RegisterViewModel();
        });
    }
    
    
}