using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;


namespace ProcessSystem.DB
{
    public class Test2
    {
        
public Test2()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");


            AppConfiguration = builder.Build();
        }
            public IConfiguration AppConfiguration { get; set; }

        public string Configure(IApplicationBuilder app)
        {
            var color = AppConfiguration["color"];
            var text = AppConfiguration["text"];
            return color;
        }
    }

        
    
}
