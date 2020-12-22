using System;
using ContactListSample.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ContactListSample.Views;
using ContactListSample.Data;

namespace ContactListSample
{
    public partial class App : Application
    {

        static ContactDatabase database;
        IContactService _contactsService;

        public App(IContactService contactsService)
        {
            _contactsService = contactsService;

            InitializeComponent();

            MainPage = new NavigationPage(new WelcomePage(contactsService));
            
        }

        public static ContactDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ContactDatabase();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts


        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

        }
    }
}
