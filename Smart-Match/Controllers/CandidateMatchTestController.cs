using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Smart_Match.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateMatchTestController : ControllerBase
    {
        private readonly ICandidateMatcherService _matcherService;

        public CandidateMatchTestController(ICandidateMatcherService matcherService)
        {
            _matcherService = matcherService;
        }

        [HttpGet("run")]
        public async Task<IActionResult> RunMatching()
        {
            var result = await _matcherService.MatchCandidatesToJobsAsync();
            return Ok(result); // מחזיר את תוצאות ההתאמה כ-JSON
        }
    }
}
