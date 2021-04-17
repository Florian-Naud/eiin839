using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Route
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public String route { get; set; }
        public List<Position> positions { get; set; } = new List<Position>();

        public void addRoute(Route r)
        {
            this.distance += r.distance;
            this.duration += r.duration;
            this.route += r.route;
            this.positions.AddRange(r.positions);
        }

        public void addInstruction(String instruction, double distance)
        {
            if(distance == 0)
            {
                this.route = this.route + instruction + ". ";
            } else
            {
                this.route = this.route + instruction + " for " + distance + ". ";
            }
            
        }

        public void addPosition(double lat, double lng)
        {
            Position pos = new Position(lat, lng);
            this.positions.Add(pos);
        }
    }
}
