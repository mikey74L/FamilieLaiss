using System;
using System.Net.Http;
using System.Threading.Tasks;
using Steeltoe.Common.LoadBalancer;
using Steeltoe.Discovery;

namespace ServiceLayerHelper;

public abstract class MicroserviceBase(IDiscoveryClient steeltoeDiscoClient)
{
    #region Private Members

    private readonly RoundRobinLoadBalancer _loadBalancer = new(steeltoeDiscoClient);

    #endregion

    #region Private Methods

    /// <summary>
    /// Get the URL for the given service from Service Discovery 
    /// </summary>
    /// <param name="address">The address for the given service</param>
    /// <returns>The URL for given service</returns>
    private Task<Uri> GetUrlForServiceAsync(string address)
    {
        return _loadBalancer.ResolveServiceInstanceAsync(new Uri(address));
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Get an HTTP-Client with identity support over bearer token
    /// </summary>
    /// <returns>The requested HTTP-Client</returns>
    protected async Task<HttpClient> GetHttpClient(string adress)
    {
        var clientMicroservice = new HttpClient();

        var uri = await GetUrlForServiceAsync(adress);
        clientMicroservice.BaseAddress = uri;

        return clientMicroservice;
    }

    #endregion
}