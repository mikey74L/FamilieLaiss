using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands
{
    public interface iResetPasswordRequestCmd
    {
        /// <summary>
        /// User name
        /// </summary>
        string UserName { get; }
    }
}
