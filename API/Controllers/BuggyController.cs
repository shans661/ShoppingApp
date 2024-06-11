using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext Context;
        public BuggyController(StoreContext context)
        {
            this.Context = context;

        }
        [HttpGet("notfound")]
        public ActionResult GetNoFoundRequest()
        {
            Product item = Context.Products.Find(99);
            if(item == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            Product item = Context.Products.Find(99);
            
            item.Description.ToString();
            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
         [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}