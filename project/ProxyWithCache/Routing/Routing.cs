using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Routing : IRouting
    {
        public String GetDirection(String depart, String arrive, String laVille)
        {

            Task<String> json = askInRest("http://localhost:8733/Design_Time_Addresses/Biking/Proxy/GetJCDecauxItemsByCity?name=" + laVille.ToLower());

            List<JCDecauxItem> stations = JsonConvert.DeserializeObject<List<JCDecauxItem>>(json.Result);

            Position departPos = getPositionOfAddress(depart);
            Position arrivePos = getPositionOfAddress(arrive);

            JCDecauxItem departStation = GetNearbyStation(departPos, stations);
            JCDecauxItem arriveStation = GetNearbyStation(arrivePos, stations);

            String itineraire = "";

            if(departStation.Equals(arriveStation))
            {
                itineraire = GetItineraire(departPos, arrivePos);
            } else
            {
                itineraire = GetItineraire(departPos, departStation.position) + " " + GetItineraire(departStation.position, arriveStation.position) + " " + GetItineraire(arriveStation.position, arrivePos);

            }

            return itineraire;
        }

        private async Task<String> askInRest(String adresse)
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync(adresse);
        }

        private Position getPositionOfAddress(String adresse)
        {
            Task<String> json = askInRest("https://api.openrouteservice.org/geocode/search/structured?api_key=5b3ce3597851110001cf6248c377b7ce2b7540f980be7db94b99482e&address=" + adresse);
            json.Wait();
            dynamic txt = JsonConvert.DeserializeObject(json.Result);
            Position pos = new Position((double)txt.features[0].geometry.coordinates[1], (double)txt.features[0].geometry.coordinates[0]);
            return pos;
        }

        private JCDecauxItem GetNearbyStation(Position pos, List<JCDecauxItem> stations)
        {
            JCDecauxItem nearby = null;
            double distance = Double.PositiveInfinity;
            foreach(var o in stations)
            {
                double dist = pos.distance(o.position);
                if (distance > dist)
                {
                    nearby = o;
                    distance = dist;
                }
            }

            return nearby;
        }

        private String GetItineraire(Position depart, Position arrive)
        {
            String dep = depart.lng.ToString().Replace(",", ".") + "," + depart.lat.ToString().Replace(",", ".");
            String arr = arrive.lng.ToString().Replace(",", ".") + "," + arrive.lat.ToString().Replace(",", ".");
            String address = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf6248c377b7ce2b7540f980be7db94b99482e&start=" + dep +"&end=" + arr;
            Task<String> json = askInRest(address);
            json.Wait();

            return "oui";
        }
        
    }
}
