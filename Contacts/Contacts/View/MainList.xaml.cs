using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Contacts.View
{
    //Temporary
    public class Profile
    {
        public string Source { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public partial class MainList : ContentPage
    {
        //Temporary
        public List<Profile> Profiles { get; set; }

        public MainList()
        {
            InitializeComponent();

            //Temporary code just to test the DataTemplate
            Profiles = new List<Profile>
            {
                new Profile {Source = "avatar_icon.png", NickName = "Ivan2020", Name = "Ivan", Date = DateTime.Now},
            };
            this.BindingContext = this;
        }
    }
}
