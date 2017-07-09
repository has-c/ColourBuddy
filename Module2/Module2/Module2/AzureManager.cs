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
        private IMobileServiceTable<colourbuddyinformation> colourTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://colourbuddy.azurewebsites.net");
            this.colourTable = this.client.GetTable<colourbuddyinformation>();
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

        public async Task<List<colourbuddyinformation>> GetColourInformation()
        {
            return await this.colourTable.ToListAsync();
        }

        public async Task PostColourInformation(colourbuddyinformation colourTable)
        {
            await this.colourTable.InsertAsync(colourTable);
        }

    }
}