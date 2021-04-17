using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{

    public class JCDecauxItem
    {
        public int number { get; set; }
        public String contract_name { get; set; }
        public String name { get; set; }
        public String address { get; set; }
        public Position position { get; set; }
        public Boolean banking { get; set; }
        public Boolean bonus { get; set; }
        public String status { get; set; }
        public Stands overflowStands { get; set; }
        public Stands mainStands { get; set; }
        public Stands totalStands { get; set; }

        public JCDecauxItem()
        {
        }
    }

    public class Position
    {
        public double lat { get; set; }
        public double lng { get; set; }

        public Position()
        {
        }

        public Position(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        public double distance(Position pos)
        {
            return Math.Sqrt(Math.Pow(pos.lat - lat, 2) + Math.Pow(pos.lng - lng, 2));
        }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public Availabilities()
        {

        }
    }

    public class Stands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
        public Stands()
        {

        }
    }
}
