using CRUDangular_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CRUDangular_1.Controllers
{
    public class ItemWebApiController : ApiController
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET api/<controller>
        public item_total Get()
        {
            item_total IT = new item_total();
            List<items_region> IR = new List<items_region>();
            List<rooms_region> RR = new List<rooms_region>();
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            RR = db.rooms_region.ToList();
            IR = db.items_region.ToList();
            IT.item = IR;
            IT.room = RR;
            return IT;
        }

        // GET api/<controller>/5
        public rooms_region Get(int id)
        {
            rooms_region RR = new rooms_region();
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            if (id == 10101)
            {
                var sql = "select SUM(item_total) from dbo.items_region";
                var total = db.Database.SqlQuery<int>(sql).Single();
                RR.item_amount = total;
            }
            else
            {
                RR = db.rooms_region.Where(a => a.room_ID == id).FirstOrDefault();
            }
            return RR;
        }
        // POST api/<controller>
        [HttpPost]
        public void Post(singleItem item)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            if (ModelState.IsValid)
            {
                int amount = Convert.ToInt32(item.item_total);
                int room_no = Convert.ToInt32(item.room_no);
                items_region IR = new items_region();
                List<rooms_region> RR = new List<rooms_region>();
                rooms_region SingleRR = new rooms_region();
               IR =  db.items_region.Where(a => a.item_name == item.item_name).FirstOrDefault();
               if (IR != null)
               {
                   IR.item_total = IR.item_total + Convert.ToInt32(item.item_total);
                   if (IR.item_remarks == null || IR.item_remarks == "N/A")
                   {
                       IR.item_remarks = item.item_remarks;
                   }
                   db.Entry(IR).State = System.Data.Entity.EntityState.Modified;
                   if (room_no >= 1)
                   {
                       RR = db.rooms_region.Where(a => a.room_no == room_no).ToList();
                       if (RR != null && RR.Count() != 0)
                       {
                           SingleRR = RR.Where(a => a.item_ID == IR.item_ID).FirstOrDefault();
                           if (SingleRR != null)
                           {
                               SingleRR.item_amount = SingleRR.item_amount + Convert.ToInt32(item.item_total);
                               db.Entry(SingleRR).State = System.Data.Entity.EntityState.Modified;
                           }
                           else
                           {
                               rooms_region SingleRR1 = new rooms_region();
                               int ID = IR.item_ID;
                               SingleRR1.item_ID = ID;
                               SingleRR1.room_no = Convert.ToInt32(item.room_no);
                               SingleRR1.item_amount = Convert.ToInt32(item.item_total);
                               SingleRR1.updated_by = User.Identity.Name;
                               SingleRR1.updated_date = DateTime.Now.ToString();
                               db.rooms_region.Add(SingleRR1);
                           }
                       }
                       else
                       {
                           SingleRR.item_ID = IR.item_ID;
                           SingleRR.room_no = Convert.ToInt32(item.room_no);
                           SingleRR.item_amount = Convert.ToInt32(item.item_total);
                           SingleRR.updated_by = User.Identity.Name;
                           SingleRR.updated_date = DateTime.Now.ToString();
                           db.rooms_region.Add(SingleRR);
                       }
                   }
               }
               else if (IR == null)
               {
                   IR.item_name = item.item_name;
                   IR.item_total = amount;
                   IR.item_remarks = item.item_remarks;
                   IR.updated_by = User.Identity.Name;
                   IR.updated_date = DateTime.Now.ToString();
                   db.items_region.Add(IR);
                   db.SaveChanges();
                   IR = db.items_region.Where(a => a.item_name == item.item_name).FirstOrDefault();
                   SingleRR.item_ID = IR.item_ID;
                   SingleRR.room_no =Convert.ToInt32(item.room_no);
                   SingleRR.item_amount =Convert.ToInt32(item.item_total);
                   db.rooms_region.Add(SingleRR);
               }
               try
               {
                   db.SaveChanges();
               }
               catch (Exception e)
               {
                   log.Error(e);
               }
            }
        }

        // PUT api/<controller>/5
        public void Put(rooms_region room)
        {
            if (ModelState.IsValid) 
            {
                enterItem(room.item_ID, room.room_ID, room.item_amount);
                test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
                db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
        }

        public void enterItem(int item_id,int room_id,int amount)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            items_region IR = new items_region();
            rooms_region RR = new rooms_region();
            RR = db.rooms_region.Where(a => a.room_ID == room_id).FirstOrDefault();
            IR = db.items_region.Where(a => a.item_ID == item_id).FirstOrDefault();

            int total = amount;
            if (RR.item_amount > total)
            {
                total = RR.item_amount - total;
                IR.item_total = IR.item_total - total;
                db.Entry(IR).State = System.Data.Entity.EntityState.Modified;
            }
            else if (RR.item_amount < total)
            {
                total = total - RR.item_amount;
                IR.item_total = IR.item_total + total;
                db.Entry(IR).State = System.Data.Entity.EntityState.Modified;
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}