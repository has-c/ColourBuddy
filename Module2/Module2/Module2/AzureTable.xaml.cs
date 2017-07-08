using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Module2.DataModels;

namespace Module2

{
    public partial class AzureTable : ContentPage
    {

        public AzureTable()
        {
            InitializeComponent();

        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<colourbuddyinformation> notHotDogInformation = await AzureManager.AzureManagerInstance.GetHotDogInformation();

            HotDogList.ItemsSource = notHotDogInformation;
        }

    }
}