using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using System.Threading.Tasks;

namespace VideoConvertExecuteService.Interfaces;

public interface IJobExecutor
{
    Task ExecuteJob(ConsumeContext<IMassConvertVideoCmd> consumerContext);
}