using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MobileAppUnitTestWithAppium.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MobileAppUnitTestWithAppium
{
    public partial class App : Application
    {

        private IEnumerable<Book> _books;

        public App()
        {
            InitializeComponent();

            if( _books == null )
            {
                var assembly = typeof( App ).GetTypeInfo().Assembly;

                Stream stream = assembly.GetManifestResourceStream( "MobileAppUnitTestWithAppium.Data.Books.json" );

                using( StreamReader streamReader = new StreamReader( stream ) )
                {
                    using( JsonReader reader = new JsonTextReader( streamReader ) )
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        _books = serializer.Deserialize< IEnumerable<Book>>( reader );
                    }
                }
            }

            MainPage = new NavigationPage( new MainPage( _books ) );
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
