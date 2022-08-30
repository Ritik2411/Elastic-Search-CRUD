using Elastic_Search_CRUD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elastic_Search_CRUD.Repository
{
    public interface IBike
    {
        Task<object> getAllBike();

        Task<object> getBikeById(int id);
        
        Task<bool> addBike(Addbike bike);

        Task<bool> updateBike(string id, Addbike bike);

        Task<bool> deleteBike(string id);
    }
}
