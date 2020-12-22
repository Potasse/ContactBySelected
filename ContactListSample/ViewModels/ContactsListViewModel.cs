using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ContactListSample.Models;
using ContactListSample.Services;
using Xamarin.Forms;

using System.Threading.Tasks;
using System.Collections;
using ContactListSample.Views;
using Android.Provider;
using Android.Content;


namespace ContactListSample.ViewModels
{
    public class ContactsListViewModel : BaseModel
    {

        IContactService _contactService;
        INavigation _navigation;

        //public ICommand ToggledCommand { get; private set; }       


        /*Definition des elements en rapport avec la Chekbox selection*/

        public ICommand NavigationCommand { get; set; }
        //public Command SelectContactAllCommand { get; set; }
        //public ICommand DeselectContactAllCommand { get; set; }

        /*----------------------------------------------------*/

        //Nombre de contact
        public int NbrContact;


        public string Title => "Contacts";

        string search;
        public string SearchText
        {

            get { return search; }

            set
            {

                if (search != value)
                {
                    search = value;

                    OnPropertyChanged("SearchText");

                    if (string.IsNullOrEmpty(SearchText))
                    {
                        FilteredContacts = new ObservableCollection<Contact>(Contacts);

                        
                    }

                    else
                    {

                        FilteredContacts = new ObservableCollection<Contact>(Contacts?.ToList()?.Where(s => !string.IsNullOrEmpty(s.Name) && s.Name.ToLower().Contains(SearchText.ToLower())));

                    }
                }
            }
        }


        /*TOGGLE BUTTON SELECT ALL*/

        private bool swithFive;
        public bool SwithFive
        {
            set
            {
                if (swithFive != value)
                {
                    swithFive = value;
                    OnPropertyChanged("SwithFive");
                    if (value == true)
                    {
                        foreach (var item in Contacts)
                        {
                            item.IsSelected = true;

                        }
                    }
                    else
                    {

                        foreach (var item in Contacts)
                        {
                            item.IsSelected = false;
                        }

                    }
                }
            }
            get
            {
                return swithFive;
            }
        }

      


        /*--Element contact pour la selection checkbox----*/

        ObservableCollection<Contact> _contacts;
        public ObservableCollection<Contact> Contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                _contacts = value;
                OnPropertyChanged("ContactList");
            }
        }



        //Methode de classement des contacts par ORDER

        ObservableCollection<Contact> filteredContacts;  
        public ObservableCollection<Contact> FilteredContacts
        {

            get { return filteredContacts; }


            set {

                if (filteredContacts != value)
                {
                    filteredContacts = value;

                    OnPropertyChanged("FilteredContacts");
                }

            }
        }



        //Fonction de base de la page
        public ContactsListViewModel(IContactService contactsService, INavigation navigation)
        {


            _navigation = navigation;
            _contactService = contactsService;
          

            Contacts = new ObservableCollection<Contact>();

            Xamarin.Forms.BindingBase.EnableCollectionSynchronization(Contacts, null, ObservableCollectionCallback);
            _contactService.OnContactLoaded += OnContactLoaded;

            LoadContacts();

            FilteredContacts = Contacts;

            /*Code pour les commandes de selected plugin indien*/           
                //GetContacts();

                //SelectContactCommand = new Command(() => SelectContact());
                NavigationCommand = new Command(() => Navigation_Page());



            //swithFive = false;
           

                //SelectContactAllCommand = new Command(() => SelectContactAll());
                //DeselectContactAllCommand = new Command(() => DeselectContactAll());
               
           


        }

        

        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            // `lock` ensures that only one thread access the collection at a time
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }

        private void OnContactLoaded(object sender, ContactEventArgs e)
        {
            Contacts.Add(e.Contact);
            //Contacts.IndexOf(e.Contact);
            

            
        }


        async void LoadContacts()
        {
               
            try
            {

                await _contactService.RetrieveContactsAsync();
                
                
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Task was cancelled");
            }
        }


        /**METHODE NAVIGATION***/


        void Navigation_Page()
        {
           

            if (Contacts.Count > 0) {

                
                    var SelectedContactList = Contacts.Where(x => x.IsSelected == true).ToList();
                    

                if (SelectedContactList.Count > 0)
                {

                     var NbrContact = SelectedContactList.Count;


                    if (SelectedContactList == null)
                    {

                        Console.WriteLine("ERREUR");

                    }
                    else
                    {
                                                
                        int total = 0;

                        foreach (var element in SelectedContactList.ToList())
                        {

                            total++;


                            Console.WriteLine(element.Name[0]);
                            Console.WriteLine(element.PhoneNumbers[0]);


                        }

                       
                        _navigation.PushAsync(new SelectedContactsPage(SelectedContactList, _contactService, _navigation, NbrContact));

                    }       
                                       
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("SELECTIONNER", "Veuillez selectionner les contacts", "Ok");
                }
            }
            else
            {
                App.Current.MainPage.DisplayAlert("GetContactsDemo", "Aucun contact dans le telephone", "Ok");
            }

        }



        /*public async Task AddContacts(string Name, string [] PhoneNumber)
        {

            List<ContentProviderOperation> ops = new List<ContentProviderOperation>();

            int rawContactInsertIndex = ops.Count;

            ContentProviderOperation.Builder builder =
            ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
            ops.Add(builder.Build());

            //Name
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, Name);


            ops.Add(builder.Build());

            //Number
            int i = 0;
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.Phone.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, PhoneNumber[i]);


            builder.WithValue(ContactsContract.CommonDataKinds.StructuredPostal.InterfaceConsts.Type, "mobile");
                   


            
                // 
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "Primary Phone");
            ops.Add(builder.Build());

            

            try
            {
                var res = Android.App.Application.Context.ContentResolver.ApplyBatch(ContactsContract.Authority, ops);

                Toast.MakeText(Android.App.Application.Context, "Contact Saved", ToastLength.Short).Show();
            }
            catch
            {
                Toast.MakeText(Android.App.Application.Context, "Contact Not Saved", ToastLength.Long).Show();

            }

        }*/




        //LOCATION PERMISSION A VOIR
        /*void UpdateContact()
        {

            //contactId
            //string firstname = "CHRIS";
            //string number = "000000000";


            //public Contact contact;

            Contacts = new ObservableCollection<Contact>();
            var SelectedContactList = Contacts.Where(x => x.IsSelected == true).ToList();



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

        }*/


       /* void CreateContact()
        {


            string lastName = "Leon ";
            string firstName = "Lu";
            string phone = "123456";

            List<ContentProviderOperation> ops = new List<ContentProviderOperation>();

            ContentProviderOperation.Builder builder =
            ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AggregationMode,
            AggregationMode.Disabled.GetHashCode());
            ops.Add(builder.Build());

            //Name
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                              ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.FamilyName, lastName);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, firstName);
            ops.Add(builder.Build());

            //Number
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                              ContactsContract.CommonDataKinds.Phone.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, phone);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type,
                              ContactsContract.CommonDataKinds.Phone.InterfaceConsts.TypeCustom);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "Work");
            ops.Add(builder.Build());


            //Add the new contact
            ContentProviderResult[] res;
            try
            {
                res = Android.App.Application.Context.ContentResolver.ApplyBatch(ContactsContract.Authority, ops);

                System.Diagnostics.Debug.WriteLine("CONTACT AJOUTÉ AVEC SUCCES");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("CONTACT NON AJOUTÉ");
            }

        }*/


    }
}
