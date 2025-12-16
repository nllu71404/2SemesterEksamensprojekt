using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Services;
using _2SemesterEksamensProjekt.Views.Pages;

namespace _2SemesterEksamensProjekt.Views
{
 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Visibility _backButtonVisibility = Visibility.Collapsed;
        public Visibility BackButtonVisibility
        {
            get => _backButtonVisibility;
            set
            {
                _backButtonVisibility = value;
                OnPropertyChanged(nameof(BackButtonVisibility));
            }
        }

        public ICommand BackCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            AppNavigationService.Initialize(MainFrame);

            BackCommand = new RelayCommand(_ =>
            {
                AppNavigationService.GoBack();
            });

            // Hver gang der navigeres → tjek hvilken side man er på
            MainFrame.Navigated += OnNavigated;

            // Start på Menu
            AppNavigationService.Navigate(new MainMenuPage());
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            
            if (e.Content is MainMenuPage)
                BackButtonVisibility = Visibility.Collapsed;
            else
                BackButtonVisibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}