using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadVideo;
using MassTransit;
using System.Threading.Tasks;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Interfaces;

public interface IVideoConverter
{
    Task ConvertVideo(ConsumeContext<IMassConvertVideoCmd> consumeContext, string filenameSourceVideo,
        MediaInfoData metadata);
}