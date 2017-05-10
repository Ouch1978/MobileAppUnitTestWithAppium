using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileAppUnitTestWithAppium.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppUnitTestWithAppium
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class DetailPage : ContentPage
    {
        public DetailPage(Book book)
        {
            InitializeComponent();

            this.BindingContext = book;
        }
    }
}
