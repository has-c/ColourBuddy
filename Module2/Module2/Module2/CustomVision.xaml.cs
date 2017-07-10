using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Module2.Model;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using Module2.DataModels;

namespace Module2
{
    public partial class CustomVision : ContentPage
    {
        public CustomVision()

        {
            InitializeComponent();

        }

        private async void loadCamera(object sender, EventArgs e)
        {


            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            await MakePredictionRequest(file);


        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }


        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "604985757f4f4ed193bc3511407a4d76");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/2defb8ad-10d4-4b1c-90b7-bfffd52a91bf/image?iterationId=880590b0-79cc-4097-981b-eea2e2c5f377";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    int count = 0;
                    double value = 0;
                    int rounds = 0;
                    int turns = 0;
                    string state = "0.0845"; //random number set to declare the string
                    string phrase = "0.078"; // random number used to declare the string

                    double prob = 0;


                    var responseString = await response.Content.ReadAsStringAsync();

                    JObject rss = JObject.Parse(responseString);

                    //Querying with LINQ
                    //Get all Prediction Values
                    var Probability = from p in rss["Predictions"] select (string)p["Probability"];
                    var Tag = from p in rss["Predictions"] select (string)p["Tag"];

                    //Truncate values to labels in XAML
                    foreach (var item in Tag)
                    {
                        TagLabel.Text += item + ": \n";
                    }

                    foreach (var item in Probability)
                    {
                        prob = Convert.ToDouble(item);
                        prob = prob * 100;
                        string num = Convert.ToString(prob);
                        PredictionLabel.Text += (num + "\n");

                        if (count == 0)
                        {
                            value = prob;
                            count += 1;
                        }
                        else
                        {

                            if (value < prob)
                            {
                                value = prob;
                                rounds += 1;
                            }
                            count += 1;
                        }
                    }

                    phrase = Convert.ToString(value);

                    foreach (var item in Tag)
                    {


                        if (turns <= rounds)
                        {
                            state = item;
                            turns += 1;
                        }

                        if (turns == 0)
                        {
                            state = item;
                        }
                    }

                    Decision.Text = "This colour is " + state;

                    colourbuddyinformation model = new colourbuddyinformation()
                    {
                        Colour = state,
                        Probability = phrase
                    };

                    await AzureManager.AzureManagerInstance.PostColourInformation(model);







                }

                //Get rid of file once we have finished using it
                file.Dispose();
            }
        }
    }
}


