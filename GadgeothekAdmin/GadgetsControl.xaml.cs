using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace GadgeothekAdmin
{
    /// <summary>
    /// Interaction logic for GadgetsControl.xaml
    /// </summary>
    public partial class GadgetsControl : UserControl
    {
        public ObservableCollection<Gadget> GadgetItems { get; set; }
        public int ItemsCount => GadgetItems?.Count ?? 0;

        public ListSortDirection sortHolder;

        public GadgetsControl()
        {
            InitializeList();
            InitializeComponent();
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
            if (GadgetListBox.SelectedIndex > 0)
            {
                Gadget selectedGadget = (Gadget)GadgetListBox.SelectedValue;
                string messageBoxText = "Do you want to delete "+selectedGadget.ToString()+" ?";

                if(MessageBox.Show(messageBoxText, "Security", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var url = ConfigurationManager.AppSettings["server"];
                    var service = new LibraryAdminService(url);
                    if(!service.DeleteGadget(selectedGadget))
                    {
                        System.Console.WriteLine("Could not delete Gadget "+ GadgetListBox.SelectedValue.ToString());
                    }
                    else
                    {
                        bool ret = GadgetItems.Remove(selectedGadget);
                        //TODO: Update Model after delete Gadget
                    }
                }
            }
        }

        private void Button_Sort_Event(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBoxGadgetSort.SelectedItem;
            if (selectedItem.Content != null)
            {
                string sortProperty = selectedItem.Content.ToString();
                if(sortHolder == ListSortDirection.Ascending)
                {
                    SortView(sortProperty, ListSortDirection.Descending);
                }
                else
                {
                    SortView(sortProperty, ListSortDirection.Ascending);
                }
            }
        }

        private void Button_Add_Event(object sender, RoutedEventArgs e)
        {
            AddGadget window = new AddGadget();
            window.ShowDialog();
            if(window.myGadget != null)
            {
                //TODO: Update Model after create new Gadget
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
            sortHolder = direction;
        }
    }
}
