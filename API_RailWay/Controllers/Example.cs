using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EFMT.Concrete;
using MT.Entities;

//namespace API_RailWay.Controllers
//{
//    public class Approaches1Controller : ApiController
//    {
//        private EFDbContext db = new EFDbContext();

//        // GET: api/Approaches
//        public IQueryable<ApproachesCars> GetApproachesCars()
//        {
//            return db.ApproachesCars;
//        }

//        // GET: api/Approaches/5
//        [ResponseType(typeof(ApproachesCars))]
//        public IHttpActionResult GetApproachesCars(int id)
//        {
//            ApproachesCars approachesCars = db.ApproachesCars.Find(id);
//            if (approachesCars == null)
//            {
//                return NotFound();
//            }

//            return Ok(approachesCars);
//        }

//        // PUT: api/Approaches/5
//        [ResponseType(typeof(void))]
//        public IHttpActionResult PutApproachesCars(int id, ApproachesCars approachesCars)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != approachesCars.ID)
//            {
//                return BadRequest();
//            }

//            db.Entry(approachesCars).State = EntityState.Modified;

//            try
//            {
//                db.SaveChanges();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ApproachesCarsExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/Approaches
//        [ResponseType(typeof(ApproachesCars))]
//        public IHttpActionResult PostApproachesCars(ApproachesCars approachesCars)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.ApproachesCars.Add(approachesCars);
//            db.SaveChanges();

//            return CreatedAtRoute("DefaultApi", new { id = approachesCars.ID }, approachesCars);
//        }

//        // DELETE: api/Approaches/5
//        [ResponseType(typeof(ApproachesCars))]
//        public IHttpActionResult DeleteApproachesCars(int id)
//        {
//            ApproachesCars approachesCars = db.ApproachesCars.Find(id);
//            if (approachesCars == null)
//            {
//                return NotFound();
//            }

//            db.ApproachesCars.Remove(approachesCars);
//            db.SaveChanges();

//            return Ok(approachesCars);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool ApproachesCarsExists(int id)
//        {
//            return db.ApproachesCars.Count(e => e.ID == id) > 0;
//        }
//    }
//}