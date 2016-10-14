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
    [RoutePrefix("api/hackers")]
    public class HackerController : ApiController
    {
        HackerRepository hackerRepository;

        public HackerController()
        {
            hackerRepository = new HackerRepository();
        }

        // GET: api/Hacker
        [Route("")]
        public IEnumerable<Hacker> Get()
        {
            var request = HttpContext.Current.Request;
            string team = request.QueryString["team"];
            string checked_in = request.QueryString["checkedin"];
            string present = request.QueryString["present"];
            return hackerRepository.GetHackers(team, checked_in, present);
        }

        // GET: api/Hacker/5
        [Route("{id:int}")]
        public Hacker Get(int id)
        {
            return hackerRepository.GetHacker(id);
        }
        [Route("checkedin")]
        public IEnumerable<Hacker> GetCheckedIn()
        {
            return hackerRepository.GetByCheckedIn(true);
        }
        [Route("notcheckedin")]
        public IEnumerable<Hacker> GetNotCheckedIn()
        {
            return hackerRepository.GetByCheckedIn(false);
        }
        [Route("present")]
        public IEnumerable<Hacker> GetPresent()
        {
            return hackerRepository.GetByPresent(true);
        }
        [Route("absent")]
        public IEnumerable<Hacker> GetNotPresent()
        {
            return hackerRepository.GetByPresent(false);
        }

        // POST: api/Hacker
        [Route("")]
        public bool Post([FromBody]Hacker value)
        {
            return hackerRepository.InsertNewHacker(value);
        }

        // PUT: api/Hacker/5
        [Route("{id:int}")]
        public bool Put(int id, [FromBody]Hacker value)
        {
            return hackerRepository.UpdateHacker(id, value);
        }

        // DELETE: api/Hacker/5
        [Route("{id:int}")]
        public bool Delete(int id)
        {
            return hackerRepository.DeleteHacker(id);
        }
    }
}
