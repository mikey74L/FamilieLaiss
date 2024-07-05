using System.Collections.Generic;
using System.Threading.Tasks;
using UserInteraction.DTO;

namespace Message.API.Interfaces
{
    public interface IIdentityMicroService
    {
        Task<IEnumerable<FamilieLaissUserDTOModel>> GetUsersForGroup(string groupName);
    }
}
