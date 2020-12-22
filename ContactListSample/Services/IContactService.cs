using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContactListSample.Models;
using Plugin.ContactService.Shared;
using Contact = ContactListSample.Models.Contact;

namespace ContactListSample.Services
{

    public class ContactEventArgs : EventArgs
    {
        public Contact Contact { get; }
        public ContactEventArgs(Contact contact)
        {
            Contact = contact;
        }
    }
    public interface IContactService
    {
        event EventHandler<ContactEventArgs> OnContactLoaded;
        bool IsLoading { get; }

        Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? token = null);

        //METHODE DE GETALL POUR TOUCHER LES ID
        //Task<IEnumerable<Contact>> GetAllContacts();


    }   
}
