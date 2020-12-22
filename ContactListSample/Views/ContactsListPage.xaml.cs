
using ContactListSample.Models;
using ContactListSample.Services;
using ContactListSample.ViewModels;

using Xamarin.Forms;



namespace ContactListSample.Views
{
    public partial class ContactsListPage : ContentPage
    {

        IContactService _contactsService;

        INavigation navigation;

        public ContactsListPage(IContactService contactsService)
        {

            _contactsService = contactsService;

            BindingContext = new ContactsListViewModel(contactsService, Navigation);

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;


            myListView.ItemSelected += (sender, e) =>
            {

                var itemSelected = e.SelectedItem as Contact;
                if (itemSelected == null)
                {
                    return;
                }
                  ((ListView)sender).SelectedItem = null;

            };
        }
    }
}
