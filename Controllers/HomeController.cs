using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        public IActionResult Index()
        {

            // List<Dishes> AllUsers = _context.Dishes.ToList();
    
			// // Get Users with the LastName "Jefferson"
			// List<Dishes> Jeffersons = _context.Dishes.Where(d => d.Name == "Jefferson").ToList();

    		// Get the 5 most recently added Users
            List<Dishes> dishes = _context.Dishes
                .OrderByDescending(d => d.CreatedAt)
                // .Take(5)
                .ToList();

            return View("Index", dishes);
        }

        [HttpGet("{cmd}/{dishID}")]
        public IActionResult Form(string cmd,int dishID)
        {
            Dishes dish = new Dishes();
            switch(cmd) {
                case "view":
                    dish = (Dishes)_context.Dishes.Where(d => d.DishId == dishID).FirstOrDefault();
                    return View("Detail",dish);

                case "delete":
                    dish = (Dishes)_context.Dishes.Where(d => d.DishId == dishID).FirstOrDefault();
                    _context.Dishes.Remove(dish);
                    _context.SaveChanges();
                    return RedirectToAction("Index");

                default :
                    if(_context.Dishes.Where(d => d.DishId == dishID).Count()>0){
                        dish = (Dishes)_context.Dishes.Where(d => d.DishId == dishID).FirstOrDefault();
                    }
                return View("Form", dish);
            }
        }

        
        [HttpPost("save/{dishID}")]
        public IActionResult Save(int dishID,Dishes Dish)
        {

            if(ModelState.IsValid)
            {
                if(dishID==0){
                     _context.Add(Dish);
                } else {
                    
                    Dishes dish = _context.Dishes.FirstOrDefault(d => d.DishId == dishID);
                    
                    dish.Chef = Dish.Chef;
                    dish.Name = Dish.Name;
                    dish.Calories = Dish.Calories;
                    dish.Tastiness = Dish.Tastiness;
                    dish.Description = Dish.Description;
                    dish.UpdatedAt = DateTime.Now;

                }
               
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                // Oh no!  We need to return a ViewResponse to preserve the ModelState, and the errors it now contains!
                return View("Form", Dish);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
