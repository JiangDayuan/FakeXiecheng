using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Controllers
{
    [Route("api/manualapi")]

    public class ManualAPIController
    {
        // GET: api/<TestAPIController>
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "A1", "A2" };
        }
    }
}
