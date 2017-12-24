using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OloTest
{
    class PizzaToppings
    {
        static string url = "http://files.olo.com/pizzas.json";

        static void Main(string[] args)
        {
            //get the topping info from the api
            var pizzas = GetData(url);

            //check if we actually got something back
            if(pizzas == null)
            {
                Console.WriteLine("Something went wrong attempting to connect with URL: " + url);
                return;
            }

            var toppingResults = new List<KeyValuePair<Pizza, int>>();

            foreach(var pizza in pizzas)
            {
                var findTopping = toppingResults.Where(x => x.Key.ToString() == pizza.ToString()).FirstOrDefault();

                //(weird null-check for KeyValuePairs)
                if(findTopping.Equals(new KeyValuePair<Pizza, int>()))
                {
                    //We don't already have this combination; add it to the list
                    toppingResults.Add(new KeyValuePair<Pizza, int>(pizza, 1));
                }
                else
                {
                    //We already have this combination; increment the count by 1
                    //(have to create new KeyValuePair because they're read-only :/)
                    var newPair = new KeyValuePair<Pizza, int>(findTopping.Key, findTopping.Value + 1);
                    toppingResults.Remove(findTopping);

                    toppingResults.Add(newPair);
                }
            }

            var results = toppingResults.OrderByDescending(x => x.Value).ToList().Take(20);

            //Display results
            var count = 1;

            foreach(var result in results)
            {
                Console.WriteLine(string.Format("#{0} appearing {1} times:", count, result.Value));
                result.Key.Toppings.ToList().ForEach((x) => Console.Write(x + " "));
                Console.WriteLine();

                count++;
            }

            Console.Read();
        }

        private static IList<Pizza> GetData(string url)
        {
            using (var w = new WebClient())
            {
                var data = "";

                try
                {
                    data = w.DownloadString(url);
                }
                catch (Exception e)
                {
                    Console.WriteLine("URL unreachable");
                }

                return !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<List<Pizza>>(data) : null;
            }
        }
    }


}
