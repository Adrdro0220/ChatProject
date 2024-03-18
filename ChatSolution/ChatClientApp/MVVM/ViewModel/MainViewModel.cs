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
    }
    
}