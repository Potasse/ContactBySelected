using System;
using System.Collections.Generic;
using System.ComponentModel;
using ContactListSample.Services;
using Xamarin.Forms;

namespace ContactListSample.Views
{
    public partial class WelcomePage : ContentPage
    {


        IContactService _contactsService;

        public WelcomePage(IContactService contactsService)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            _contactsService = contactsService;

        }

        void Launch_Click(System.Object sender, System.EventArgs e)
        {

            this.Navigation.PushAsync(new ContactsListPage(_contactsService));

        }
    }
}
