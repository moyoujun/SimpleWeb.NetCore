using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWeb.NetCore.Dto;

namespace SimpleWeb.NetCore.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class ZhuanLanController : Controller
    {

        [HttpGet]
        public IEnumerable<ZhuanlanDto> GetColumns(int limit, int offset)
        {
            var templist = new List<ZhuanlanDto>();
            //

            var column1 = new ZhuanlanDto()
            {
                ID = "test01",
                Name = "Test01 Name",
                Author = "Test01 Author",
                Tags = new List<string>()
                {
                    "TestTag01", "TestTag02"
                }
            };

            templist.Add(column1);

            return templist;
        }
    }
}
