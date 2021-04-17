using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class RoutingImpl : IRouting, IRoutingREST
    {
        public WebResult GetDirectionREST(string depart, string arrive, string laVille)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return GetDirection(depart, arrive, laVille);
        }

        public WebResult GetDirectionSOAP(string depart, string arrive, string laVille)
        {
            return GetDirection(depart, arrive, laVille);
        }

        public Position GetPositionCityREST(string laVille)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return GetPositionCity(laVille);
        }

        public Position GetPositionCitySOAP(string laVille)
        {
            return GetPositionCity(laVille);
        }

        public WebResult GetDirection(string depart, string arrive, string laVille)
        {
            //Cherche les coordonnées des adresses
            Position departPos = getPositionOfAddress(depart, laVille);
            Position arrivePos = getPositionOfAddress(arrive, laVille);

            if(departPos == null || arrivePos == null)
            {
                return new WebResult((int)WebCode.BADADDRESS, new Route());
            }

            //Cherche un itinéraire sans passer par les stations
            Route itineraireWithoutBike = new Route();
            itineraireWithoutBike.addRoute(GetItineraire(departPos, arrivePos, "foot-walking"));

            //Recupère toutes les stations de la ville
            Task<String> json = askInRest("http://localhost:8733/Design_Time_Addresses/Biking/Proxy/GetJCDecauxItemsByCity?name=" + laVille.ToLower());

            List<JCDecauxItem> stations = JsonConvert.DeserializeObject<List<JCDecauxItem>>(json.Result);

            //Si pas de station dans la ville alors on renvoie l'itinéraire sans station
            if(stations.Count == 0)
            {
                return new WebResult((int)WebCode.NOSTATION, itineraireWithoutBike);
            }

            //Cherche les stations la plus proche du point de départ et d'arrivée
            JCDecauxItem departStation = GetNearbyStation(departPos, stations);
            JCDecauxItem arriveStation = GetNearbyStation(arrivePos, stations);

            //Si on prend et on dépose un vélo a la meme station, alors elle sert à rien
            if (departStation.Equals(arriveStation))
            { 
                return new WebResult((int)WebCode.WITHOUTBIKE, itineraireWithoutBike);
            }

            //Calcul de l'itinéraire en passant par les stations de départ et d'arrivée
            Route itineraireWithBike = new Route();
            itineraireWithBike.addRoute(GetItineraire(departPos, departStation.position, "foot-walking"));
            itineraireWithBike.addInstruction("Take your bike here", 0);
            itineraireWithBike.addRoute(GetItineraire(departStation.position, arriveStation.position, "cycling-regular"));
            itineraireWithBike.addInstruction("Give your bike here", 0);
            itineraireWithBike.addRoute(GetItineraire(arriveStation.position, arrivePos, "foot-walking"));

            //Verifie si c'est mieux de passer par l'itinéraire avec ou sans vélo
            Boolean withoutBikeIsBetter = itineraireWithBike.distance > itineraireWithoutBike.distance;
            if (withoutBikeIsBetter)
            {
                return new WebResult((int)WebCode.WITHOUTBIKE, itineraireWithoutBike);
            }

            //Bravo l'itinéraire avec le vélo est mieux!!
            return new WebResult((int)WebCode.WITHBIKE, itineraireWithBike);
        }

        public Position GetPositionCity(string laVille)
        {
            Task<String> json = askInRest("https://api.openrouteservice.org/geocode/search/structured?api_key=5b3ce3597851110001cf6248c377b7ce2b7540f980be7db94b99482e&locality=" + laVille);
            json.Wait();
            dynamic txt = JsonConvert.DeserializeObject(json.Result);
            if (txt.features.Count == 0)
            {
                return null;
            }
            return new Position((double)txt.features[0].geometry.coordinates[1], (double)txt.features[0].geometry.coordinates[0]);
        }

        private async Task<String> askInRest(String adresse)
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync(adresse);
        }

        private Position getPositionOfAddress(String adresse, String city)
        {
            Task<String> json = askInRest("https://api.openrouteservice.org/geocode/search/structured?api_key=5b3ce3597851110001cf6248c377b7ce2b7540f980be7db94b99482e&address=" + adresse + "&locality=" + city);
            json.Wait();
            dynamic txt = JsonConvert.DeserializeObject(json.Result);
            if(txt.features.Count == 0)
            {
                return null;
            } else if (txt.features[0].properties.layer.Equals("Locality"))
            {
                return null;
            }
            return new Position((double)txt.features[0].geometry.coordinates[1], (double)txt.features[0].geometry.coordinates[0]);
        }

        private JCDecauxItem GetNearbyStation(Position pos, List<JCDecauxItem> stations)
        {
            JCDecauxItem nearby = null;
            double distance = Double.PositiveInfinity;
            foreach(var o in stations)
            {
                double dist = pos.distance(o.position);
                if (distance > dist && o.mainStands.availabilities.bikes > 0)
                {
                    nearby = o;
                    distance = dist;
                }
            }

            return nearby;
        }

        private Route GetItineraire(Position depart, Position arrive, String type)
        {
            String dep = depart.longitude.ToString().Replace(",", ".") + "," + depart.latitude.ToString().Replace(",", ".");
            String arr = arrive.longitude.ToString().Replace(",", ".") + "," + arrive.latitude.ToString().Replace(",", ".");
            String address = "https://api.openrouteservice.org/v2/directions/"+ type +"?api_key=5b3ce3597851110001cf6248c377b7ce2b7540f980be7db94b99482e&start=" + dep +"&end=" + arr;
            Task<String> json = askInRest(address);
            json.Wait();
            dynamic txt = JsonConvert.DeserializeObject(json.Result);
            Route route = new Route();
            route.duration = txt.features[0].properties.segments[0].duration;
            route.distance = txt.features[0].properties.segments[0].distance;
            foreach(dynamic pos in txt.features[0].properties.segments[0].steps){
                double dist = pos.distance;
                string inst = pos.instruction;
                route.addInstruction(inst, dist);
            }
            foreach (dynamic pos in txt.features[0].geometry.coordinates)
            {
                double lat = pos[1];
                double lng = pos[0];
                route.addPosition(lat, lng);
            }

            return route;
        }
        
    }
}
