using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using HackTheUApi.Models;
using HackTheUApi.Services;
using System.Web;

namespace HackTheUApi.Controllers
{
    [RoutePrefix("api/hardware")]
    public class HardwareController : ApiController
    {
        HardwareRepository hardwareRepository;

        public HardwareController()
        {
            hardwareRepository = new HardwareRepository();
        }
        // GET: api/Hardware
        [Route("")]
        public IEnumerable<Hardware> Get()
        {
            var request = HttpContext.Current.Request;
            string name = request.QueryString["name"];
            string category = request.QueryString["category"];
            string owner = request.QueryString["owner"];
            return hardwareRepository.GetHardware(name,category,owner);
        }
        // GET: api/Hardware/5
        [Route("{id:int}")]
        public Hardware Get(int id)
        {
            return hardwareRepository.GetHardware(id);
        }

        [Route("available")]
        public IEnumerable<Hardware> GetAvailable()
        {
            return hardwareRepository.GetByAvailability(true);
        }

        [Route("checkedout")]
        public IEnumerable<Hardware> GetCheckedOut()
        {
            return hardwareRepository.GetByAvailability(false);
        }

        // POST: api/Hardware
        [Route("")]
        public bool Post([FromBody]Hardware value)
        {
            return hardwareRepository.InsertNewHardware(value);
        }

        // PUT: api/Hardware/5
        [Route("{id:int}")]
        public bool Put(int id, [FromBody]Hardware value)
        {
            return hardwareRepository.UpdateHardware(id, value);
        }

        // DELETE: api/Hardware/5
        [Route("{id:int}")]
        public bool Delete(int id)
        {
            return hardwareRepository.DeleteHardware(id);
        }
    }
}
