using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IMetaExtractor
{
    Task ExtractMetadataAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename);
}