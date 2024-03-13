using ChatClientApp.Core;
using ChatClientApp.MVVM.View;

namespace ChatClientApp.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand MessagesViewCommand { get; set; }
    public RelayCommand LoginViewCommand { get; set; }
    public RelayCommand RegisterViewCommand { get; set; }
    
    private object _currentView;
    public object CurrentView {
        get
        {
            return _currentView;
        }
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        CurrentView = new HomeViewModel();
        
        HomeViewCommand = new RelayCommand(o =>
        {
            CurrentView = new HomeViewModel();
        });
        MessagesViewCommand = new RelayCommand(o =>
        {
            CurrentView = new MessagesViewModel();
        });
        LoginViewCommand = new RelayCommand(o =>
        {
            CurrentView = new LoginViewModel();
        });
        RegisterViewCommand = new RelayCommand(o =>
        {
            CurrentView = new RegisterViewModel();
        });
    }
    
}