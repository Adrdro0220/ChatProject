using ChatClientApp.Core;
using ChatClientApp.MVVM.View;

namespace ChatClientApp.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    
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
<<<<<<< HEAD
=======
        
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
>>>>>>> 76aec74a5652788737cd89ea6400a7aea357c0b6
    }
    
}