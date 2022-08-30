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
            ESBike data = new ESBike();
            var allData = await _elasticClient.SearchAsync<ESBike>(s => s.Query(q => q.MatchAll()));
            data.brand_id = allData.Total + 1;
            data.brand_name = bike.brand_name;

            var result = await _elasticClient.IndexDocumentAsync(data);
            return result.IsValid;
        }

        public async Task<bool> deleteBike(string id)
        {
            var result = await _elasticClient.DeleteAsync<ESBike>(id);
            return result.IsValid;
        }

        public async Task<Response> getAllBike(int pageIndex, int pageSize, string brand_name, string sortOrder, string sortField)
        {
            Response response = new Response();

            //Pagination
            var result = await _elasticClient.SearchAsync<ESBike>(s => s.From((pageIndex - 1) * pageSize).Size(pageSize).Query(q => q.MatchAll()));

            //Searching
            if (!String.IsNullOrEmpty(brand_name)) { 
                result = await _elasticClient.SearchAsync<ESBike>(s => s.Query(q => q.Regexp(re => re.Field(f => f.brand_name).Value(brand_name + "*"))));
            }

            //Sorting
            if (!String.IsNullOrEmpty(sortOrder) && !String.IsNullOrEmpty(sortField)) {
                if (sortOrder == "ASC")
                {
                    if (sortField == "brand_name")
                    {
                        result = await _elasticClient.SearchAsync<ESBike>(s => s.Sort(s => s.Ascending("brand_name.keyword")).Query(q => q.MatchAll()));
                    }
                }

                else {
                    if (sortField == "brand_name") { 
                        result = await _elasticClient.SearchAsync<ESBike>(s => s.Sort(s => s.Descending("brand_name.keyword")).Query(q => q.MatchAll()));
                    }
                }
                
            }    

            var data = result.Hits.ToList().Select(x => new Bike
            {
                _id = x.Id,
                bike_data = x.Source
            }
            );

            response.totalDocuments = result.Total;
            response.data = data;

            return response;
        }

        public async Task<object> getBikeById(int id)
        {
            var result = await _elasticClient.SearchAsync<ESBike>(s => s.Query(q => q.Term(t => t.Field("brand_id").Value(id))));
            
            var data = result.Hits.ToList().Select(x => new Bike { 
                _id = x.Id,
                bike_data = x.Source
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
