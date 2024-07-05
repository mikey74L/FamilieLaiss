using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace PictureConvertExecuteService.Interfaces;

public interface IMetaExtractor
{
    Task ExtractMetadataAsync(ConsumeContext<IConvertPictureCmd> consumerContext, string filename);
}