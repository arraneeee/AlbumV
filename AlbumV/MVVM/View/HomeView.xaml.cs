using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlbumV.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void AlbumItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Albums selectedAlbum)
            {

                Window mainWindow = Window.GetWindow(this);
                if (mainWindow is MainWindow)
                {
                    (mainWindow as MainWindow).SetSelectedAlbum(selectedAlbum);
                }
            }
        }
    }
}