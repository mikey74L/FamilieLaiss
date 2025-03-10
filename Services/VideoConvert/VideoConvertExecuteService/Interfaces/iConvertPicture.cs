using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadVideo;
using MassTransit;
using System.Threading.Tasks;

namespace VideoConvertExecuteService.Interfaces;

public interface IConvertPicture
{
    Task ConvertPicture(long id, string filename, ConsumeContext<IMassConvertVideoCmd> consumerContext);
}