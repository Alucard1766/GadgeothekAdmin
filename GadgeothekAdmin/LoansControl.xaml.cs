using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Configuration;

namespace GadgeothekAdmin
{
    /// <summary>
    /// Interaction logic for LoansControl.xaml
    /// </summary>
    public partial class LoansControl : UserControl
    {
        public ObservableCollection<Loan> LoansItem { get; set; }
        public int ItemsCount => LoansItem?.Count ?? 0;

        public LoansControl()
        {
            InitializeComponent();
            //TODO: Update List all seconds or realtime
            InitializeList();
        }

        private void InitializeList()
        {
            LoansItem = new ObservableCollection<Loan>();
            var url = ConfigurationManager.AppSettings["server"];
            var service = new LibraryAdminService(url);
            List<Loan> loans = service.GetAllLoans();
            foreach (Loan l in loans)
            {
                LoansItem.Add(l);
            }
        }
    }
}
