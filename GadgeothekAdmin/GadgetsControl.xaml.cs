using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System;
using System.Globalization;

namespace GadgeothekAdmin
{
    /// <summary>
    /// Interaction logic for GadgetsControl.xaml
    /// </summary>
    public partial class GadgetsControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Gadget> GadgetItems { get; set; }
        public int ItemsCount => GadgetItems?.Count ?? 0;

        private ListSortDirection _sortHolder;
        public ListSortDirection SortHolder
        {
            get { return _sortHolder; }
            set { SetProperty(ref _sortHolder, value, nameof(SortHolder)); }
        }

        public GadgetsControl()
        {
            InitializeList();
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeList()
        {
            GadgetItems = new ObservableCollection<Gadget>();
            var url = ConfigurationManager.AppSettings["server"];
            var service = new LibraryAdminService(url);
            List<Gadget> gadgets = service.GetAllGadgets();
            foreach (Gadget g in gadgets)
            {
                GadgetItems.Add(g);
            }
        }

        private void Button_Delete_Event(object sender, RoutedEventArgs e)
        {
            if (GadgetListBox.SelectedIndex < 0)
            {
                return;
            }

            Gadget selectedGadget = (Gadget)GadgetListBox.SelectedValue;
            string messageBoxText = "Do you want to delete " + selectedGadget.ToString() + " ?";

            if (MessageBox.Show(messageBoxText, "Security", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var url = ConfigurationManager.AppSettings["server"];
                var service = new LibraryAdminService(url);
                if (!service.DeleteGadget(selectedGadget))
                {
                    System.Console.WriteLine("Could not delete Gadget " + GadgetListBox.SelectedValue.ToString());
                }
                else
                {
                    bool ret = GadgetItems.Remove(selectedGadget);
                }
            }
        }

        private void Button_Sort_Event(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBoxGadgetSort.SelectedItem;
            if (selectedItem.Content != null)
            {
                string sortProperty = selectedItem.Content.ToString();
                SortView(sortProperty, SortHolder == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);
            }
        }

        private void Button_Add_Event(object sender, RoutedEventArgs e)
        {
            AddGadget window = new AddGadget();
            window.ShowDialog();
            if(window.myGadget != null)
            {
                GadgetItems.Add(window.myGadget);
            }
        }


        private void Combobox_Sort_Event(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBoxGadgetSort.SelectedItem;
            if(selectedItem.Content != null)
            {
                string sortProperty = selectedItem.Content.ToString();
                SortView(sortProperty, ListSortDirection.Ascending);
            }
        }

        private void SortView(string sortProperty, ListSortDirection direction)
        {
            CollectionViewSource cvs = FindResource("SortedGadgets") as CollectionViewSource;
            cvs.SortDescriptions.Clear();
            cvs.SortDescriptions.Add(new SortDescription(sortProperty, direction));
            SortHolder = direction;
        }

        private void SetProperty<T>(ref T field, T value, string name)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

    }

    public class SortDirectionToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ListSortDirection && (ListSortDirection)value == ListSortDirection.Ascending ? "SortDown" : "SortUp";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
