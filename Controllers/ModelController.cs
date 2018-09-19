using Microsoft.AspNetCore.Mvc;
using small_assignment_2.Models;
using System;
using System.Linq;
using template.Data;
using template.Extensions;
using template.Models;

namespace small_assignment_2.Controllers
{
    public class ModelController : Controller
    {
        [HttpGet]
        [Route("getallmodels")]
        public IActionResult GetAllModels([FromQuery]int pageNumber = 0, [FromQuery]int pageSize = 10)
        {
            Envelope<ModelDTO> envelope = new Envelope<ModelDTO>();

            var mDto = DataContext.Models.ToLightWeight(GetAcceptLanguage()).ToList();

            foreach (var item in mDto)
            {
                item.Links.AddReference("self", "/getallmodels");
            }

            envelope.Items = mDto;
            envelope.PageNumber = pageNumber;
            envelope.PageSize = pageSize;
            return Ok(envelope);
        }

        [HttpGet]
        [Route("getmodelbyid/{modelId}")]
        public IActionResult GetModelById(int modelId)
        {
            var mdDto = DataContext.Models.ToDetails(GetAcceptLanguage()).FirstOrDefault(m => m.Id == modelId);
            mdDto.Links.AddReference("self", "/getmodelbyid/" + modelId);
            return Ok(mdDto);
        }

        private string GetAcceptLanguage()
        {
            if (HttpContext.Request.Headers["Accept-Language"].ToString() == "")
                return "en-US";
            else
                return HttpContext.Request.Headers["Accept-Language"].ToString();
        }
    }
}