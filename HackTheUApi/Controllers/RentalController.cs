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
    [RoutePrefix("api/rentals")]
    public class RentalController : ApiController
    {
        RentalRepository rentalRepository;

        public RentalController()
        {
            rentalRepository = new RentalRepository();
        }
        // GET: api/Rental
        [Route("")]
        public IEnumerable<Rental> Get()
        {
            return rentalRepository.GetAllRentals();
        }

        // GET: api/Rental/hardware/5
        [Route("hardware/{id:int}")]
        public Rental GetHardwareRental(int id)
        {
            return rentalRepository.GetHardwareRental(id);
        }

        // GET: api/Rental/hacker/5
        [Route("hacker/{id:int}")]
        public Rental GetHackerRental(int id)
        {
            return rentalRepository.GetHackerRental(id);
        }

        [Route("current")]
        public Rental GetHackerRental()
        {
            return rentalRepository.GetCurrentRentals();
        }

        // POST: api/Rental
        [Route("new/{hardware:int}/{hacker:int}")]
        public bool CheckOut(int hardware, int hacker)
        {
            return rentalRepository.CheckOut(hardware, hacker);
        }

        [Route("{hardware:int}/{hacker:int}")]
        public bool CheckIn(int hardware, int hacker)
        {
            return rentalRepository.CheckIn(hardware, hacker);
        }
    }
}
