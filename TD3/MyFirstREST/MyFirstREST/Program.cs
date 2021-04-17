using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace MyFirstREST
{
    class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
           
            try
            {
                Task<String> contrats = getAllContract();
                dynamic obj = JsonConvert.DeserializeObject(contrats.Result);
                Console.WriteLine(obj); 

                Task <String> stations = getAllStationWithContract(obj[0].contract_name.ToString());
                dynamic obj2 = JsonConvert.DeserializeObject(stations.Result);
                Console.WriteLine(obj2);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nRIP petit program!" + e);
            }
            Console.ReadLine();
            
        }

        public static async Task<String> getAllContract()
        {
            return await httpGetRequest("https://api.jcdecaux.com/vls/v1/stations?apiKey=aadb7f161ebe9eb9358509283772daab3be2898b");
        }

        public static async Task<String> getAllStationWithContract(String contrat)
        {
            String url = "https://api.jcdecaux.com/vls/v1/stations?contract=" + contrat + "&apiKey=aadb7f161ebe9eb9358509283772daab3be2898b";
            return await httpGetRequest(url);
        }

        public static async Task<String> httpGetRequest(String request)
        {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
