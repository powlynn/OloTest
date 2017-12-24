using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OloTest
{
    class Pizza
    {
        [JsonProperty("toppings")]
        public IList<string> Toppings { get; set; }

        public override string ToString()
        {
            //Alphabetize the toppings using Sort()
            ((List<string>)Toppings).Sort();

            //return all appended
            return String.Join(",", Toppings);
        }
    }
}
