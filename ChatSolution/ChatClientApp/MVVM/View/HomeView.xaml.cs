<<<<<<< HEAD
﻿using System.Windows;
=======
﻿
using System.Windows;
>>>>>>> 76aec74a5652788737cd89ea6400a7aea357c0b6
using ChatClientApp.MVVM.ViewModel;

namespace ChatClientApp.MVVM.View
{
    public partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }
        
        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            
            if (mainWindow.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.CurrentView = new LoginViewModel();
            }
        }
    }
}