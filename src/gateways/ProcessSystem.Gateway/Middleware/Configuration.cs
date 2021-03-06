using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ProcessSystem.Middleware
{
    public static class Configuration
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {

        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static void ValidateConfigurationOptions(this IApplicationBuilder app)
        {
            // Just get them all early. All validatoin is in DataAttributes.
            try
            {

            }
            catch (OptionsValidationException ex)
            {
                throw new InvalidOperationException("Ошибка в app.config, cм. внутреннее исключение", ex); 
            }
        }
    }
}
