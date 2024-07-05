using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace PictureConvertExecuteService.Interfaces;

public interface IJobExecutor
{
    Task ExecuteJobAsync(ConsumeContext<IConvertPictureCmd> consumerContext);
}