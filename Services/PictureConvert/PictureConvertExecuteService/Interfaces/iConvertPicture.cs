using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace PictureConvertExecuteService.Interfaces;

public interface IConvertPicture
{
    Task ConvertPictureAsync(ConsumeContext<IConvertPictureCmd> consumerContext, string filename);
}