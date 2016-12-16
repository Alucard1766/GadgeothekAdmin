using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Configuration;
using System;

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
            LoansItem = new ObservableCollection<Loan>();
            FillData();
            InitializeWebSocket();
            DataContext = this;
        }

        private void InitializeWebSocket()
        {
            var url = ConfigurationManager.AppSettings["server"];
            var client = new ch.hsr.wpf.gadgeothek.websocket.WebSocketClient(url);
            client.NotificationReceived += (o, e) =>
            {
                if (e.Notification.Target == typeof(Loan).Name.ToLower())
                    FillData();
            };
            client.ListenAsync();
        }



        private void FillData()
        {
            LoansItem.Clear();
            var url = ConfigurationManager.AppSettings["server"];
            var service = new LibraryAdminService(url);
            List<Loan> loans = service.GetAllLoans();
            foreach (Loan l in loans)
            {
                if(l.IsLent)
                    LoansItem.Add(l);
            }
        }
    }
}
