using Elastic_Search_CRUD.Models;
using Elastic_Search_CRUD.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Elastic_Search_CRUD.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BikeController : ControllerBase
    {
        private readonly IBike _bike; 
        public BikeController(IBike bike) {
            _bike = bike;
        }

        [HttpGet]
        public async Task<IActionResult> getAllBikes([FromQuery, BindRequired] int PageIndex, [FromQuery, BindRequired]int PageSize) {
            var result = await _bike.getAllBike(PageIndex, PageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getBikeById([FromRoute]int id) { 
            var result = await _bike.getBikeById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> addBike([FromBody]Addbike bike) {
            var result = await _bike.addBike(bike);
            if (result)
            {
                return Ok(result);
            }
            else {
                return BadRequest(result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateBike([FromRoute]string id, [FromBody]Addbike bike){
            var result = await _bike.updateBike(id, bike);
            if (result)
            {
                return Ok(result);
            }
            else {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteBook([FromRoute]string id) {
            var result = await _bike.deleteBike(id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

    }

}

