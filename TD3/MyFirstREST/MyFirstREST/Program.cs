using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace MyFirstREST
{
    class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        static void Main()
        {
           
            try
            {
                String res = getAllContract().Result;
                //res.ToString()
                Console.WriteLine(JsonConvert.DeserializeObject(res));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nRIP petit program!");
            }
            Console.ReadLine();
            
        }

        public static async Task<String> getAllContract()
        {
            return await httpGetRequest("https://api.jcdecaux.com/vls/v1/stations?apiKey=aadb7f161ebe9eb9358509283772daab3be2898b");
        }

        public static async Task<String> httpGetRequest(String request)
        {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
