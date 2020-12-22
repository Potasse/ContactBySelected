using System;
using System.Collections.Generic;

using ContactListSample.Models;
using ContactListSample.Services;
using ContactListSample.ViewModels;
using Xamarin.Forms;

namespace ContactListSample.Views
{
    public partial class SelectedContactsPage : ContentPage
    {

        IContactService _contactService;
      
        INavigation _navigation;

        int NbrContact;
       

        public SelectedContactsPage(List<Contact> SelectedList, IContactService contactsService, INavigation navigation, int NbrContact)
        {                  

            InitializeComponent();

            _contactService = contactsService;
            _navigation = navigation;

            BindingContext = new SelectedContactsViewModel(SelectedList, contactsService, navigation, NbrContact);

            nbrSelect.Text = "Le nombre de contact selectioné est: " + NbrContact;
        }

        /*void Button_Synchro(System.Object sender, System.EventArgs e)
        {
                        
            this.Navigation.PushAsync(new EditNumberPage(contactsService));
        }*/
    }
}

