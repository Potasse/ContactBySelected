using System;
using System.ComponentModel;
using SQLite;

namespace ContactListSample.Models
{

   

    public class Contact : BaseModel
    {
        [PrimaryKey, AutoIncrement]

        public int ContactId { get; set; }
        public string Name { get; set; }
        //public string Image { get; set; }
        //public string[] Emails { get; set; }
        public string[] PhoneNumbers { get; set; }
        //public string Number { get; set; }
        //public int Value { get; set; }


        bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }


}
