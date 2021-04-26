﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;

namespace RoutingSelfHosted
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a ServiceHost for the CalculatorService type.  
            using (ServiceHost host = new ServiceHost(typeof(Routing.RoutingImpl)))
            {

                host.Open();

                Console.WriteLine("The service is ready at {0}", host.BaseAddresses[0]);
                while (true)
                {

                    // Just hang around until the container destroys the service
                    Thread.Sleep(1000);

                }

                // Close the ServiceHost - not really needed because Docker will destroy the host and us with it
                host.Close();

            }
        }
    }
}
