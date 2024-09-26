using System.Collections.Generic;
using System.Threading.Tasks;
using ValidateToken.Model;

namespace ValidateToken.Repository
{
    public interface IValidateTokenRepository
    {
        Task<List<IntegrationvariablesStatus>> GetAllAsync();
    }
}
