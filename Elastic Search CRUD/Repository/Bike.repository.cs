using Elastic_Search_CRUD.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elastic_Search_CRUD.Repository
{
    public class BikeRepository : IBike
    {
        private readonly IElasticClient _elasticClient;

        public BikeRepository(IElasticClient elasticClient) { 
            _elasticClient = elasticClient;
        }

        
        public async Task<bool> addBike(Addbike bike)
        {
            Bike data = new Bike();
            var allData = await _elasticClient.SearchAsync<Bike>(s => s.Query(q => q.MatchAll()));
            data.brand_id = allData.Total + 1;
            data.brand_name = bike.brand_name;

            var result = await _elasticClient.IndexDocumentAsync(data);
            return result.IsValid;
        }

        public async Task<bool> deleteBike(string id)
        {
            var result = await _elasticClient.DeleteAsync<Bike>(id);
            return result.IsValid;
        }

        public async Task<object> getAllBike()
        {
            var result = await _elasticClient.SearchAsync<Bike>(s => s.Query(q => q.MatchAll()));
            var data = result.Hits.ToList().Select(x => new Response
            {
                _id = x.Id,
                data = x.Source
            }
            ); 

            return data;
        }

        public async Task<object> getBikeById(int id)
        {
            var result = await _elasticClient.SearchAsync<Bike>(s => s.Query(q => q.Term(t => t.Field("brand_id").Value(id))));
            var data = result.Hits.ToList().Select(x => new Response { 
                _id = x.Id,
                data = x.Source
            });
            
            return data;
        }

        public async Task<bool> updateBike(string id, Addbike bike)
        {
            var result = await _elasticClient.UpdateAsync<Addbike>(id, u => u.Doc(bike));
            return result.IsValid;
        }
    }
}
