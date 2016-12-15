using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace GadgeothekAdmin
{
    /// <summary>
    /// Interaction logic for AddGadget.xaml
    /// </summary>
    public partial class AddGadget : Window
    {
        public Gadget myGadget {get;set;}
        public AddGadget()
        {
            InitializeComponent();
            myGadget = null;
        }

        private void Button_Gadget_Create(object sender, RoutedEventArgs e)
        {
            string name = Text_Name.Text;
            string manufacturer = Text_Manufacturer.Text;
            double price = System.Convert.ToDouble(Text_Price.Text);
            ch.hsr.wpf.gadgeothek.domain.Condition condition = (ch.hsr.wpf.gadgeothek.domain.Condition) System.Enum.Parse(typeof(ch.hsr.wpf.gadgeothek.domain.Condition), ((ComboBoxItem)Combo_Condition.SelectedItem).Content.ToString());

            var url = ConfigurationManager.AppSettings["server"];
            var service = new LibraryAdminService(url);
            myGadget = new Gadget(name);
            myGadget.Condition = condition;
            myGadget.Manufacturer = manufacturer;
            myGadget.Price = price;

            if (service.AddGadget(myGadget))
            {
                System.Console.WriteLine("Add Gadget "+myGadget.ToString()+" done");
                this.Close();
            }
        }

    }
}
