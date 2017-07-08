using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Module2.DataModels;

namespace Module2
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<colourbuddyinformation> notHotDogTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://colourbuddy.azurewebsites.net");
            this.notHotDogTable = this.client.GetTable<colourbuddyinformation>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<colourbuddyinformation>> GetHotDogInformation()
        {
            return await this.notHotDogTable.ToListAsync();
        }
    }
}