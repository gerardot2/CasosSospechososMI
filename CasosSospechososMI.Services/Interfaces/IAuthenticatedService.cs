using System;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services
{
    public interface IAuthenticatedService
    {
        T GetRestService<T>();
        Task UpdateAuthToken();
    }
}
