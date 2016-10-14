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
    [RoutePrefix("api/teams")]
    public class TeamController : ApiController
    {
        TeamRepository teamRepository;

        public TeamController()
        {
            teamRepository = new TeamRepository();
        }
        // GET: api/Teams
        [Route("")]
        public IEnumerable<Team> Get()
        {
            return teamRepository.GetAllTeams();
        }

        // GET: api/Teams/5
        [Route("{id:int}")]
        public Team Get(int id)
        {
            return teamRepository.GetTeam(id);
        }

        // POST: api/Teams
        [Route("")]
        public Team Post([FromBody]Team value)
        {
            return teamRepository.InsertNewTeam(value);
        }

        // PUT: api/Teams/5
        [Route("{id:int}")]
        public bool Put(int id, [FromBody]Team value)
        {
            return teamRepository.UpdateTeam(id, value);
        }

        // DELETE: api/Teams/5
        [Route("{id:int}")]
        public bool Delete(int id)
        {
            return teamRepository.DeleteTeam(id);
        }
    }
}
