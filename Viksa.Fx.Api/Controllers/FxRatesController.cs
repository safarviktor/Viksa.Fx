using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viksa.Fx.Business;

namespace Viksa.Fx.Api.Controllers
{
    public class FxRatesController : AbstractController
    {
        private readonly IFxBusiness _fxBusiness;

        public FxRatesController(IFxBusiness fxBusiness)
        {
            _fxBusiness = fxBusiness;
        }

        [Route("{fromCurrency}/{toCurrency}/{fromDate}/{toDate}")]
        [HttpGet]
        public async Task<IActionResult> GetRateHistory([FromRoute] string fromCurrency, [FromRoute] string toCurrency, [FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            var result = await _fxBusiness.GetRateHistory(fromCurrency, toCurrency, fromDate, toDate);
            return Ok(result);
        }
    }
}
