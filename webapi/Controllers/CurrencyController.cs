using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using webapi.Data.Models;
using webapi.IRepository;
using webapi.Models.DTO;
using webapi.Repository;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private IConfiguration _config;
        private ICurrencyRepository _oCurrencyRepository;
        public CurrencyController(IConfiguration config, ICurrencyRepository currencyRepository)
        {
            _config = config;
            _oCurrencyRepository = currencyRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetCurrencies")]
        public async Task<IActionResult> Get()
        {
            var result = await _oCurrencyRepository.Gets();
            if (result != null)
                return Ok(result);
            else return StatusCode((int)HttpStatusCode.NotFound, "Invalid currency");
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetCurrencyChanges/{userId}")]
        public async Task<JsonResult> GetCurrencyChanges(Guid userId)
        {

            return new JsonResult(await _oCurrencyRepository.GetByCurrencyChangesByUserId(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("SaveCurrencyChange")]
        public async Task<IActionResult> Save([FromForm] CurrencyChangeDTO currencyChange)
        {

            if (currencyChange != null)
            {
                return Ok(await _oCurrencyRepository.SaveCurrencyChange(currencyChange));
            }
            else return StatusCode((int)HttpStatusCode.InternalServerError, "Some ERROR!");
        }

        [HttpGet]
        [Route("UpdateCurrencyAPI/{key}")]
        public async Task<IActionResult> GetActual(int key) // можно усложнить :)
        {
            if (key == 112) 
            {
                HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync("https://www.nbrb.by/api/exrates/rates?periodicity=0"); // API с курсами валют
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var result = JsonConvert.DeserializeObject<List<Currency>>(await response.Content.ReadAsStringAsync());
                    if (result != null)
                        await _oCurrencyRepository.Save(result);
                    else return NotFound();
                    return Ok(result);
                }
                return Ok(response);
            }
            else return StatusCode((int)HttpStatusCode.InternalServerError, "Some ERROR!");
        }
    }
}
