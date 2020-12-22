using System;
using System.Collections.Generic;
using ContactListSample.Models;
using ContactListSample.Services;
using ContactListSample.ViewModels;
using Xamarin.Forms;

namespace ContactListSample.Views
{
    public partial class EditNumberPage : ContentPage
    {


        IContactService _contactService;
        public int NbrContactEdit;


        public EditNumberPage(List<Contact> SelectedEdit, IContactService contactsService, int NbrContactEdit)
        {


            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            _contactService = contactsService;

            BindingContext = new EditNumberViewModel(SelectedEdit, contactsService, NbrContactEdit);

            nbrEditContact.Text = "Le nombre de contact modifié est: " + NbrContactEdit;


        }
    }
}
