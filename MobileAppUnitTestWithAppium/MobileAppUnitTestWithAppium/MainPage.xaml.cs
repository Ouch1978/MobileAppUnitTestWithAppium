using System.Collections.Generic;
using MobileAppUnitTestWithAppium.Models;
using Xamarin.Forms;

namespace MobileAppUnitTestWithAppium
{
    public partial class MainPage : ContentPage
    {
        public MainPage( IEnumerable<Book> books )
        {
            InitializeComponent();

            listView.ItemsSource = books;
        }

        private void listView_OnItemSelected( object sender , SelectedItemChangedEventArgs e )
        {
            if( e.SelectedItem != null )
            {
                var detailPage = new DetailPage( e.SelectedItem as Book );

                Navigation.PushAsync( detailPage );
            }
        }

    }
}
