using System.Threading.Tasks;
using ValidateToken.Service;

namespace ValidateToken.Application
{
    public class Worker : IWorker
    {
        private readonly IValidateTokenService _validateTokenService;

        public Worker(IValidateTokenService validateTokenService)
        {
            _validateTokenService = validateTokenService;
        }

        public async Task RunAsync()
        {
            await _validateTokenService.GetAllAsync();
        }
    }
}
