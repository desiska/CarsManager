using DI_Praktika.Data;
using DI_Praktika.Data.Entities;
using DI_Praktika.Models.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Controllers
{
    [Authorize(Roles = "Administrator, Client")]
    public class QueryController : Controller
    {
        private readonly ApplicationDbContext db;

        public QueryController()
        {
            db = new ApplicationDbContext();
        }

        public IActionResult Index()
        {
            List<Query> queries = db.Queries.ToList();
            List<QueryVM> list = new List<QueryVM>();

            foreach (var query in queries)
            {
                if(query.End <= DateTime.Now)
                {
                    query.Status = db.Statuses.Where(s => s.Name == "Overdue").FirstOrDefault().ID;
                    db.Update(query);
                    db.SaveChanges();
                }

                list.Add(new QueryVM()
                {
                    ID = query.ID,
                    Start = query.Start,
                    End = query.End,
                    Car = db.Cars.Find(query.carID).ToString(),
                    Price = query.Price,
                    Status = db.Statuses.Find(query.Status).Name,
                    User = db.Users.Where(u => u.Id == query.userID).FirstOrDefault().UserName
                });
            }

            QueryIndexVM model = new QueryIndexVM()
            {
                Items = list
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            QueryCreateVM model = new QueryCreateVM()
            {
                Cars = db.Cars.ToList(),
                StatusObject = db.Statuses.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QueryCreateVM model)
        {
            List<Query> list = db.Queries.Where(q => q.carID == model.CarID).ToList();
            bool isFree = true;

            foreach (var query in list)
            {
                if(query.Start <= model.Start && query.End >= model.Start)
                {
                    isFree = false;
                    break;
                }
            }

            if (isFree)
            {
                if (ModelState.IsValid)
                {
                    Query query = new Query()
                    {
                        ID = Guid.NewGuid().ToString(),
                        carID = model.CarID,
                        userID = db.Users.Where(u => u.UserName == model.User).FirstOrDefault().Id,
                        End = model.End,
                        Start = model.Start,
                        Price = model.Price,
                        Status = model.Status
                    };

                    db.Queries.Add(query);

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            else
            {
                model.IsFirstTime = false;
                model.Cars = db.Cars.ToList();
                model.StatusObject = db.Statuses.ToList();

                model.Message = "The car is not free.";
                return View(model);
            }


            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            Query query = db.Queries.Find(id);

            QueryEditVM model = new QueryEditVM()
            {
                ID = query.ID,
                CarID = query.carID,
                End = query.End,
                Start = query.Start,
                Price = query.Price,
                Status = query.Status,
                Cars = db.Cars.ToList(),
                User = query.userID,
                StatusObject = db.Statuses.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(QueryEditVM model)
        {
            if (ModelState.IsValid)
            {
                Query query = db.Queries.Find(model.ID);

                query.carID = model.CarID;
                query.End = model.End;
                query.Start = model.Start;
                query.Price = model.Price;
                query.Status = model.Status;

                try
                {
                    db.Update(query);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    bool isExist = db.Cars.Any(f => f.ID == query.ID);

                    if (!isExist)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(string id)
        {
            Query query = db.Queries.Find(id);

            db.Remove(query);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Canceled(string id)
        {
            Query query = db.Queries.Find(id);
            query.Status = db.Statuses.Where(s => s.Name == "Canceled").FirstOrDefault().ID;
            db.Update(query);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
