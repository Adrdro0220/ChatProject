using ChatClientApp.Core;
using ChatClientApp.MVVM.View;

namespace ChatClientApp.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand MessagesViewCommand { get; set; }
    public RelayCommand LoginViewCommand { get; set; }
    public RelayCommand RegisterViewCommand { get; set; }
    
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

        Currentview = new HomeViewModel();
        
        
        HomeViewCommand = new RelayCommand(o =>
        {
            Currentview = new HomeViewModel();
        });
        MessagesViewCommand = new RelayCommand(o =>
        {
            Currentview = new MessagesViewModel();
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