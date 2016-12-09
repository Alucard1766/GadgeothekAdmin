using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace GadgeothekAdmin
{
    /// <summary>
    /// Interaction logic for LoansControl.xaml
    /// </summary>
    public partial class LoansControl : UserControl
    {
        public ObservableCollection<Loan> LoansItem { get; set; }

        public LoansControl()
        {
            InitializeComponent();
            DataContext = LoansItem;
        }

        public void InitializeList()
        {
            var service = new LibraryAdminService("http://localhost:8080");
            List<Loan> loans = service.GetAllLoans();
            foreach (Loan l in loans)
            {
                LoansItem.Add(l);
            }
        }
    }
}
