using Nekono.App.Pos.Models;
using System.Threading.Tasks;

namespace Nekono.App.Pos.Services
{
    public interface ILoginServices
    {
        Task<LoginResponse> Authenticate(LoginRequest loginRequest);
    }
}