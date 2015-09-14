using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        private decimal storeProfit = 0;

        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();

        public ConsignmentShop()
        {
            InitializeComponent();
            SetupData();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListBox.DataSource = itemsBinding;

            itemsListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListBox.DataSource = cartBinding;

            shoppingCartListBox.DisplayMember = "Display";
            shoppingCartListBox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorsListBox.DataSource = vendorsBinding;

            vendorsListBox.DisplayMember = "Display";
            vendorsListBox.ValueMember = "Display";
        }

        private void SetupData()
        {
            store.Vendors.Add(new Vendor { FirstName = "Bill", LastName = "Smith" });
            store.Vendors.Add(new Vendor { FirstName = "Sue", LastName = "Jones" });

            store.Items.Add(new Item
            {
                Title = "Moby Dick",
                Description = "A book about a whale",
                Price = 4.50M,
                Owner = store.Vendors[0]
            });
            store.Items.Add(new Item
            {
                Title = "The Count of Monte Cristo",
                Description = "A book about revenge",
                Price = 7.99M,
                Owner = store.Vendors[1]
            });
            store.Items.Add(new Item
            {
                Title = "The Great Gatsby",
                Description = "A book about bourgeoise life in the 1920's",
                Price = 3.99M,
                Owner = store.Vendors[0]
            });
            store.Items.Add(new Item
            {
                Title = "Wuthering Heights",
                Description = "A book that will put you to sleep",
                Price = 0.99M,
                Owner = store.Vendors[1]
            });

            store.Name = "Book Bonanza";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            // Figure out what is selected from the items list
            // Copy that item to the shopping cart
            // Do we remove the item from the items list? - no

            Item selectedItem = (Item)itemsListBox.SelectedItem;

            shoppingCartData.Add(selectedItem);

            cartBinding.ResetBindings(false);
        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            // Mark each item in the cart as sold
            // Clear the cart

            foreach(Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeProfit += (1 - (decimal)item.Owner.Commission) * item.Price;
            }

            shoppingCartData.Clear();
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("${0}", storeProfit);

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);
        }
    }
}
