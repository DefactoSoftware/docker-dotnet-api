using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using apicore.Config;

namespace apicore.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        public AppSettings AppSettings { get; }
        public TestController(IOptions<AppSettings> appSettings)
        {
            // Reference AppSettings
            AppSettings = appSettings.Value;
        }

        [HttpGet]
        public string[] Get()
        {
            // Return string array with test values
            return new string[] { "testData1", "testData2" };
        }


        [HttpGet("appsettings")]
        public string Get(string name)
        {
            // Return value of an appsetting (Value of "AppName" in this case)
            return AppSettings.AppName;
        }
    }
}
