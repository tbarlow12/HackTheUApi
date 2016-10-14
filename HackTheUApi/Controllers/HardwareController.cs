using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HackTheUApi.Models;
using HackTheUApi.Services;

namespace HackTheUApi.Controllers
{
    public class HardwareController : ApiController
    {
        HardwareRepository hardwareRepository;

        public HardwareController()
        {
            hardwareRepository = new HardwareRepository();
        }
        // GET: api/Hardware
        public IEnumerable<Hardware> Get()
        {
            return hardwareRepository.GetAllHardware();
        }

        // GET: api/Hardware/5
        public Hardware Get(int id)
        {
            return hardwareRepository.GetHardware(id);
        }

        // POST: api/Hardware
        public void Post([FromBody]Hardware value)
        {
            hardwareRepository.InsertNewHardware(value);
        }

        // PUT: api/Hardware/5
        public void Put(int id, [FromBody]Hardware value)
        {
            hardwareRepository.UpdateHardware(id, value);
        }

        // DELETE: api/Hardware/5
        public void Delete(int id)
        {
            hardwareRepository.DeleteHardware(id);
        }
    }
}
