using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IPictureInfoExtractor
{
    Task ExtractInfoAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename);
}