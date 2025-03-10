using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IConvertPicture
{
    Task ConvertPictureAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename);
}