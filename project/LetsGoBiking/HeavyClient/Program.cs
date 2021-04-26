using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeavyClient.RoutingService;

namespace HeavyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            bool leave = false;
            RoutingClient route = new RoutingClient();

            while (!leave)
            {
                Console.Clear();
                displayMenu();
                String command = Console.ReadLine();
                switch (command)
                {
                    case "q":
                        Console.WriteLine("leaving the programm, press enter.");
                        leave = true;
                        break;
                    case "stats":
                        Dictionary<JCDecauxItem, int> histo = route.GetHistoriqueStationsSOAP();
                        foreach(KeyValuePair<JCDecauxItem, int> o in histo)
                        {
                            Console.WriteLine("Station: {0} had {1} client", o.Key.name, o.Value);
                        }
                        Console.WriteLine("Please press enter.");
                        break;
                    default:
                        Console.WriteLine("Wrong command, press enter.");
                        break;
                }
                Console.ReadLine();
            }
        }

        static void displayMenu()
        {
            Console.WriteLine("----MENU----");
            Console.WriteLine("stats - Displays the number of times a station has been used ");
            Console.WriteLine("q - leave the programme");
            Console.WriteLine();
            Console.Write(">");
        }
    }
}
