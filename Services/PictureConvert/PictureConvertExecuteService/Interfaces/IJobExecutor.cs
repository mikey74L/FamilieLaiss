using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IJobExecutor
{
    Task ExecuteJobAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext);
}