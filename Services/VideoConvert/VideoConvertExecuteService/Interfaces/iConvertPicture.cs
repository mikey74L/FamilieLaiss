using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace VideoConvertExecuteService.Interfaces;

public interface IConvertPicture
{
    Task ConvertPicture(long id, string filename, ConsumeContext<IConvertVideoCmd> consumerContext);
}