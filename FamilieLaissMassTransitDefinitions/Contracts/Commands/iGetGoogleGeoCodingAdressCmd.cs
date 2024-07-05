using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands
{
    public interface iGetGoogleGeoCodingAdressCmd
    {
        string ID { get; }

        double Longitude { get; }
        
        double Latitude { get; }
    }
}
