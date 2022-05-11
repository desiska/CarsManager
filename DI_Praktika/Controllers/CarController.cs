using DI_Praktika.Data;
using DI_Praktika.Data.Entities;
using DI_Praktika.Models.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext db;

        public CarController()
        {
            db = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrator, Client")]
        public IActionResult Index()
        {
            CarIndexVM model = new CarIndexVM()
            {
                Items = db.Cars.ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Create()
        {
            CarCreateVM model = new CarCreateVM();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarCreateVM model)
        {
            if (ModelState.IsValid)
            {
                Car car = new Car()
                {
                    ID = Guid.NewGuid().ToString(),
                    Brand = model.Brand,
                    Model = model.CarModel,
                    Description = model.Description,
                    PriceForRentForDay = model.PriceForRentForDay,
                    PassangersCount = model.PassangersCount,
                    Year = model.Year,
                    Photo = SaveFile(model.Photo)
                };

                db.Add<Car>(car);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        private static string SaveFile(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var extension = fileName.Split('.').Last();
            var fileNameWithoutExtension = string.Join("", fileName.Split('.').Take(fileName.Length - 1));

            var newfileName = "wwwroot/images/" + String.Format("{0}-{1:ddMMYYYYHHmmss}.{2}",
                fileNameWithoutExtension,
                DateTime.Now,
                extension
            );

            if (!Directory.Exists(Path.GetDirectoryName(newfileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newfileName));
            }

            using (var localFile = System.IO.File.OpenWrite(newfileName))
            {
                using (var uploadedFile = file.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }

            return newfileName;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Car model = db.Cars.Find(id);

            if(model == null)
            {
                return NotFound();
            }

            CarEditVM car = new CarEditVM()
            {
                ID = model.ID,
                Brand = model.Brand,
                CarModel = model.Model,
                Description = model.Description,
                PriceForRentForDay = model.PriceForRentForDay,
                PassangersCount = model.PassangersCount,
                Year = model.Year,
                Photo = model.Photo
            };

            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarEditVM model)
        {
            if (ModelState.IsValid)
            {
                Car car = db.Cars.FindAsync(model.ID).Result;

                car.Brand = model.Brand;
                car.Model = model.CarModel;
                car.Description = model.Description;
                car.PriceForRentForDay = model.PriceForRentForDay;
                car.PassangersCount = model.PassangersCount;
                car.Year = model.Year;
                car.Photo = model.Photo;

                try
                {
                    db.Update(car);
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    bool isExist = db.Cars.Any(f => f.ID == car.ID);

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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string? id)
        {
            Car car = await db.Cars.FindAsync(id);
            db.Cars.Remove(car);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Client")]
        public IActionResult SeeACar(string id)
        {
            SeeACarVM model = new SeeACarVM()
            {
                Src = db.Cars.Find(id).Photo
            };

            return View(model);
        }
    }
}
