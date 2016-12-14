using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GadgeothekAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isLoansVisible;

        public bool IsLoansVisible
        {
            get { return _isLoansVisible; }
            set
            {
                _isGadgetsVisible = !value;
                SetProperty(ref _isLoansVisible, value, nameof(IsLoansVisible), nameof(IsGadgetsVisible));
            }
        }

        private bool _isGadgetsVisible;

        public bool IsGadgetsVisible
        {
            get { return _isGadgetsVisible; }
            set
            {
                _isLoansVisible = !value;
                SetProperty(ref _isGadgetsVisible, value, nameof(IsLoansVisible), nameof(IsGadgetsVisible));
            }
        }


        public MainWindow()
        {
            IsGadgetsVisible = false;
            IsLoansVisible = true;
            DataContext = this;
            InitializeComponent();
        }



        private void SetProperty<T>(ref T field, T value, string name, string otherName)
        {
            field = value;
            OnPropertyChanged(name);
            OnPropertyChanged(otherName);
        }

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void btnMenuLoan_Click(object sender, RoutedEventArgs e)
        {
            IsLoansVisible = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsGadgetsVisible = true;
        }
    }
}
