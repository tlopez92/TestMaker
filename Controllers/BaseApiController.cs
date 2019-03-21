using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWeb.Data;

namespace TestMakerFreeWeb.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        #region shared properties
        protected ApplicationDbContext dbContext { get; private set; }
        protected JsonSerializerSettings jsonSettings { get; private set; }
        #endregion
        #region constructor
        public BaseApiController(ApplicationDbContext context)
        {
            this.dbContext = context;

            this.jsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }
        #endregion
    }
}
