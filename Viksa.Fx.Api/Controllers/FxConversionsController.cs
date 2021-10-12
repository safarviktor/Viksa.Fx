using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Viksa.Fx.Business;

namespace Viksa.Fx.Api.Controllers
{
    public class FxConversionsController : AbstractController
    {
        private readonly IFxBusiness _fxBusiness;

        public FxConversionsController(IFxBusiness fxBusiness)
        {
            _fxBusiness = fxBusiness;
        }

        [Route("{fromCurrency}/{fromAmount}/{toCurrency}")]
        [HttpGet]
        public async Task<IActionResult> Convert([FromRoute] string fromCurrency, [FromRoute] decimal fromAmount, [FromRoute] string toCurrency, [FromQuery] DateTime? date = null)
        {
            var result = await _fxBusiness.GetToAmount(fromAmount, fromCurrency, toCurrency, date);
            return Ok(result);
        }
    }
}
