using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IConvertPicture
{
    Task ConvertPictureAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename);
}