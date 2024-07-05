using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Discovery;
using Steeltoe.Common.LoadBalancer;
using Steeltoe.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.ServiceOptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureIdentityServerAuthenticationOptions : IConfigureNamedOptions<IdentityServerAuthenticationOptions>
    {
        #region Private Members
        private readonly IServiceScopeFactory _ServiceScopeFactory;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory"/> will be injected by DI-Container</param>
        public ConfigureIdentityServerAuthenticationOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _ServiceScopeFactory = serviceScopeFactory;
        }
        #endregion

        /// <summary>
        /// Configures the specified options.
        /// </summary>
        /// <param name="options">The options to configure</param>
        /// <param name="name">The name for the options</param>
        public void Configure(string name, IdentityServerAuthenticationOptions options)
        {
            using (var scope = _ServiceScopeFactory.CreateScope())
            {
                //Ermitteln des Discovery-Client-Services
                var DiscoClient = scope.ServiceProvider.GetRequiredService<IDiscoveryClient>();

                //Erstellen des Load-Balancers für den Service
                var LoadBalancer = new RoundRobinLoadBalancer(DiscoClient);

                //Ermitteln der URI für den Identity-Service vom Load-Balancer
                var URIResult = LoadBalancer.ResolveServiceInstanceAsync(new Uri("http://identityservice")).GetAwaiter().GetResult();

                //Festlegen der Authority
                options.Authority = URIResult.ToString();

                //Setzen der Option für die Notwendigkeit von HTTPS
                options.RequireHttpsMetadata = false;

                ////Setzen der Options für den API-Namen
                options.ApiName = "CatalogAPI";

                ////Tokens sollen zuerst aus dem Request-Header und dann aus dem Query-String ermittelt werden
                ////Das Token wird generell im Request-Header für alle API Aufrufe geliefert
                ////Für die Aufrufe für Bilder und Videos wird das Token aber im Query-String mitgeliefert
                ////Die API muss daher mit beidem gleichzeitig umgehen können.
                options.TokenRetriever = FamilieLaissCoreHelpers.OAuthHelper.TokenRetrieval.FromAuthorizationHeaderAndThenFromQueryString();
            }
        }

        /// <summary>
        /// Configures the specified options.
        /// </summary>
        /// <param name="options">The options to configure</param>
        public void Configure(IdentityServerAuthenticationOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
