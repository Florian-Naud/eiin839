using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProxyWithCache
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Proxy : IProxy
    {
        ProxyCache<JCDecauxItem> cache;
        public Proxy()
        {
            
            cache = new ProxyCache<JCDecauxItem>();
            initCache();
        }

        private void initCache()
        {
            SetCache();
            var task = Task.Run(async () => {
                for (; ; )
                {
                    await Task.Delay(60000);
                    SetCache();
                }
            });
        }

        private void SetCache()
        {
            Task<String> json = Task.Run(() => cache.initialise());
            json.Wait();
            List<JCDecauxItem> items = JsonConvert.DeserializeObject<List<JCDecauxItem>>(json.Result);
            foreach (var o in items)
            {
                cache.Set(o.name, o, 60);
            }
        }

        public List<JCDecauxItem> GetJCDecauxItemsByCity(String name)
        {
            List<JCDecauxItem> stations = cache.GetAll();
            List<JCDecauxItem> stationsInCity = new List<JCDecauxItem>();
            foreach(var o in stations)
            {
                if (o.contractName.Equals(name))
                {
                    stationsInCity.Add(o);
                }
            }

            return stationsInCity;
        }
    }
}
