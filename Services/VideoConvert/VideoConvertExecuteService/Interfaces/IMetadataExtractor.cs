using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using System.Threading.Tasks;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Interfaces;

public interface IMetadataExtractor
{
    Task<MediaInfoData> ExtractMetadata(ConsumeContext<IConvertVideoCmd> context, string filenameSourceVideo);
}