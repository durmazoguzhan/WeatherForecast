using Newtonsoft.Json;
using RestSharp;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WeatherForecast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> cityNames = new List<string>();

        private async void cboxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboxCity.Enabled = false;
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            cboxCity.Items.Clear();

            string countryName = cboxCountry.Text;

            RestRequest request = new RestRequest(Method.GET); //creating get RestRequest
            request.AddHeader("apikey", "bxfF4nSrkpHHHjW8mWGThEP3IN5oNuWV"); //apikey for authentication

            await Task.Run(()=> CountryIsoToCities(request, CountryNameToCountryIso(request, countryName))); //run methods for get cities

            foreach (string item in cityNames) //add cities to combobox
            {
                if (!cboxCity.Items.Contains(item))
                    cboxCity.Items.Add(item);
            }

            cboxCity.Enabled = true;
            progressBar1.Visible = false;
        }

        private string CountryNameToCountryIso(RestRequest request,string countryName)
        {
            string countryIsoCode = "";

            RestClient countryToIsoClient = new RestClient("https://api.apilayer.com/geo/country/name/" + countryName); //country name to
                                                                                                                        //iso codes
                                                                                                                        //restclient for api
            dynamic cityIsoResponse = JsonConvert.DeserializeObject(countryToIsoClient.Execute(request).Content);
            foreach (dynamic i in cityIsoResponse)
            {
                try
                {
                    countryIsoCode = i.alpha3code; //getting iso code by country
                }
                catch { }
            }
            return countryIsoCode;
        }

        private void CountryIsoToCities(RestRequest request,string countryIsoCode)
        {
            
            string cityName = "";

            RestClient isoToCityClient = new RestClient("https://api.apilayer.com/geo/country/cities/" + countryIsoCode); //iso code to cities
                                                                                                                          //restclient for api
            dynamic cityResponse = JsonConvert.DeserializeObject(isoToCityClient.Execute(request).Content); //getting and editing data
            foreach (dynamic i in cityResponse) //selecting data required(city names)
            {
                try
                {
                    cityName = i.state_or_region; //getting cities by country iso code
                    cityNames.Add(cityName);
                }
                catch { }
            }
        }
    }
}
