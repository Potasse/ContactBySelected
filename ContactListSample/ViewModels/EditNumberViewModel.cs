using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContactListSample.Models;
using ContactListSample.Services;

namespace ContactListSample.ViewModels
{


    public class EditNumberViewModel:Contact 
    {

        IContactService _contactsService;
        public int NbrContactEdit;




        private ObservableCollection<Contact> _selectedContactEdit;
        public ObservableCollection<Contact> SelectedContactEdit
        {
            get
            {
                return _selectedContactEdit;
            }
            set
            {
                _selectedContactEdit = value;
                OnPropertyChanged("SelectedContactEdit");
            }
        }

        public EditNumberViewModel(List<Contact> SelectedEdit, IContactService _contactsService, int NbrContactEdit)
        {

            SelectedContactEdit = new ObservableCollection<Contact>(SelectedEdit);

        }


    }
}
