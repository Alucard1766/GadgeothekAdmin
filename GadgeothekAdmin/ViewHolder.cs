namespace GadgeothekAdmin
{


    public class ViewHolder
    {
        public LoansControl MyLoansControl { get; set; }
        public GadgetsControl MyGadgetControl { get; set; }

        public ViewHolder()
        {
            MyLoansControl = new LoansControl();
            MyGadgetControl = new GadgetsControl();
        }
    }
}