using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace PictureConvertExecuteService.Interfaces;

public interface IPictureInfoExtractor
{
    Task ExtractInfoAsync(ConsumeContext<IConvertPictureCmd> consumerContext, string filename);
}