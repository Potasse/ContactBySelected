using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using ContactListSample.Models;
using ContactListSample.Services;
using ContactListSample.Views;
using Xamarin.Forms;

namespace ContactListSample.ViewModels
{

    public class SelectedContactsViewModel : Contact

    {
        IContactService _contactsService;
        INavigation _navigation;

        public int NbrContact; //nombre de contact selected

        public int NbrContactEdit;

        public ICommand NavigationCommandEdit { get; set; }
        

        private ObservableCollection<Contact> _selectedContactList;
        public ObservableCollection<Contact> SelectedContactList
        {
            get
            {
                return _selectedContactList;
            }
            set
            {
                _selectedContactList = value;
                OnPropertyChanged("SelectedContactList");
            }
        }
        

        public SelectedContactsViewModel(List<Contact> SelectedList, IContactService _contactsService, INavigation _navigation, int NbrContact)
        {
            SelectedContactList = new ObservableCollection<Contact>(SelectedList);

            /*_navigation = navigation;
            _contactService = contactsService;*/

#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            NavigationCommandEdit = new Command(() => Navigation_PageAsync());
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel


        }

        void Navigation_PageAsync()
        {

            UpdateContact();

            if (SelectedContactList.Count > 0)
            {

                var SelectedContactEdit = SelectedContactList.Where(x => x.IsSelected == true);
                //int total = 0;
                //NbrContactEdit = SelectedContactEdit.Count;

                SelectedContactEdit.ToArray();

                Console.WriteLine(SelectedContactEdit);

                /*foreach (var element in SelectedContactEdit.ToList())
                {


                    total++;
                    //Console.WriteLine(element.contactId);
                    Console.WriteLine(element.Name);
                    Console.WriteLine(element.PhoneNumbers[0]);

                    var delta = element.Name;
                    var beta = element.PhoneNumbers[0];

                }*/

                this._navigation.PopToRootAsync();
            }
            else
            {

                Console.WriteLine("ERREUR");
            }

            
         }

        //LOCATION PERMISSION A VOIR
        void UpdateContact()
        {

            //contactId
            //string firstname = "CHRIS";
            //string number = "000000000";
                                   
            foreach (var element in SelectedContactList.ToList())
            {

                List<ContentProviderOperation> ops = new List<ContentProviderOperation>();

                // Name 
                string nameSelection = ContactsContract.Data.InterfaceConsts.Id + " = ? AND "
                                  + ContactsContract.Data.InterfaceConsts.Mimetype + " = ? ";
                string[] nameSelectionArgs = {

                                    element.Name[0].ToString(),
                                    ContactsContract.CommonDataKinds.StructuredName.ContentItemType
                                    };

                //PARTIE UPDATE NAME
                ContentProviderOperation.Builder builder = ContentProviderOperation.NewUpdate(ContactsContract.Data.ContentUri);
                builder.WithSelection(nameSelection, nameSelectionArgs);
                builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, "chris");
                ops.Add(builder.Build());

                // Phone
                string phoneSelection = ContactsContract.Data.InterfaceConsts.Id + " = ? AND "
                                        + ContactsContract.Data.InterfaceConsts.Mimetype + " = ? ";
                string[] phoneelectionArgs = {

                                   element.PhoneNumbers[0].ToString(),
                                   ContactsContract.CommonDataKinds.Phone.ContentItemType
                                   };

                //PARTIE UPDATE NUMBER
                builder = ContentProviderOperation.NewUpdate(ContactsContract.Data.ContentUri);
                builder.WithSelection(phoneSelection, phoneelectionArgs);
                builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, "999999");
                ops.Add(builder.Build());

                // Update the contact
                ContentProviderResult[] result;

                try
                {
                    result = Android.App.Application.Context.ContentResolver.ApplyBatch(ContactsContract.Authority, ops);

                    System.Diagnostics.Debug.WriteLine("CONTACT MODIFIÉ AVEC SUCCES");
                    //DisplayAlert("Erreur", "Erreur reseau s'est produite:" + ex.Message, "OK");
                }
                catch
                {

                    System.Diagnostics.Debug.WriteLine("CONTACT NON MODIFIÉ ");
                }
            }
        }

    }

}
