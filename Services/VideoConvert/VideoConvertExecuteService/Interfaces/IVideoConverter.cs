using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Interfaces;

public interface IVideoConverter
{
    Task ConvertVideo(ConsumeContext<IConvertVideoCmd> consumeContext, string filenameSourceVideo, MediaInfoData metadata);
}