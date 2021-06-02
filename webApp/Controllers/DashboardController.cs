using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webApp.Data;
using webApp.Models;
using Microsoft.AspNetCore.Hosting;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using webApp.Dtos;
using SecuringWebApiUsingJwtAuthentication.Helpers;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

namespace webApp.Controllers
{
    public class DashboardController : Controller
    {
        private MyDBContext myDbContext;
        private readonly IMapper _mapper;
        IWebHostEnvironment _webHostEnvironment;
        public DashboardController(MyDBContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            myDbContext = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("dashboard/market/add-market")]

        public async Task<ActionResult> addMarket([FromForm] Market markettoCreate)
        {
            await myDbContext.markets.AddAsync(markettoCreate);
            myDbContext.SaveChanges();
            return Ok(markettoCreate);
        }


        [HttpPost("dashboard/category/add-category")]
        public async Task<ActionResult> addCategory([FromForm] Category modle)
        {
            await myDbContext.categories.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }



        [HttpPost("dashboard/slider/add-slider")]
        public async Task<ActionResult> addSlider([FromForm] Slider modle)
        {
            await myDbContext.sliders.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/field/add-field")]
        public async Task<ActionResult> addField([FromForm] Field modle)
        {
            await myDbContext.fields.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/food/add-food")]
        public async Task<ActionResult> addFood([FromForm] Food modle)
        {
            await myDbContext.foods.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/option/add-option")]
        public async Task<ActionResult> addOption([FromForm] Option modle)
        {
            await myDbContext.options.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/option-group/add-option-group")]
        public async Task<ActionResult> addOptionGroup([FromForm] OptionGroup modle)
        {
            await myDbContext.OptionGroups.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }



        [HttpPost("dashboard/user/add-user")]
        public async Task<ActionResult> addUser([FromForm] ApplicationUser modle)
        {
            await myDbContext.users.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("cities/add-city")]
        public async Task<ActionResult> addCity([FromQuery] City city)
        {
            await myDbContext.cites.AddAsync(city);
            myDbContext.SaveChanges();
            return Ok(city);
        }

        [HttpGet("dashboard/home/get-home")]
        public async Task<ActionResult> getDashboardHome()
        {
            int userCount = myDbContext.users.Where(x => x.role == "user").Count();
            int orderCount = myDbContext.orders.Count();
            int marketCount = myDbContext.markets.Count();
            int earnCount = 2334;
            List<Market> markets = new List<Market> { };
            var orders = await myDbContext.orders.OrderByDescending(x => x.Id).Take(7).AsNoTracking().ToListAsync();
            List<int> ids = orders.ConvertAll(x => x.market_id);

            markets = await myDbContext.markets.Where(x => ids.Contains(x.Id)).AsNoTracking().ToListAsync();
            return Ok(new { orders, markets, userCount, orderCount, marketCount, earnCount });
        }

        [HttpGet("dashboard/user/get-users")]
        public async Task<ActionResult> getusers()
        {
            var data = await myDbContext.users.Where(x => x.role == "user").AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/slider/get-sliders")]
        public async Task<ActionResult> getsliders()
        {
            var data = await myDbContext.sliders.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/user/get-admins")]
        public async Task<ActionResult> getAdmins()
        {
            var data = await myDbContext.users.Where(x => x.role == "admin").AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/driver/get-drivers")]
        public async Task<ActionResult> getdrivers()
        {
            var data = await myDbContext.drivers.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/market/get-markets")]
        public async Task<ActionResult> getmarkets()
        {
            var data = await myDbContext.markets.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/food/get-foods")]
        public async Task<ActionResult> getfoods()
        {
            List<Food> foods = await myDbContext.foods.AsNoTracking().ToListAsync();

            List<int> marketids = foods.ConvertAll(x => x.market_id);
            List<int> catids = foods.ConvertAll(x => x.category_id);

            List<Market> markets = await myDbContext.markets.Where(x => marketids.Contains(x.Id)).AsNoTracking().ToListAsync();
            List<Category> categories = await myDbContext.categories.Where(x => catids.Contains(x.Id)).AsNoTracking().ToListAsync();


            return Ok(new { foods, markets, categories });
        }

        [HttpGet("dashboard/category/get-categories")]
        public async Task<ActionResult> getcategories()
        {
            var data = await myDbContext.categories.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/field/get-fields")]
        public async Task<ActionResult> getfields()
        {
            var data = await myDbContext.fields.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/orders/get-orders")]
        public async Task<ActionResult> getorders()
        {
            var data = await myDbContext.orders.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/option/get-options")]
        public async Task<ActionResult> getoptions()
        {
            var data = await myDbContext.options.AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("dashboard/option-group/get-option-groups")]
        public async Task<ActionResult> getOptionGroups()
        {
            var groups = await myDbContext.OptionGroups.AsNoTracking().ToListAsync();
            List<int> ids = groups.ConvertAll(x => x.food_id);
            List<Food> foods = await myDbContext.foods.Where(x => ids.Contains(x.Id)).AsNoTracking().ToListAsync();


            return Ok(new { groups, foods });
        }

        [HttpPost("dashboard/user/delete-user")]
        public async Task<ActionResult> deleteUser([FromForm] int id)
        {
            var item = await myDbContext.users.Where(x => x.Id == id).FirstAsync();
            myDbContext.users.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/category/delete-category")]
        public async Task<ActionResult> deleteCategory([FromForm] int id)
        {
            var item = await myDbContext.categories.Where(x => x.Id == id).FirstAsync();
            myDbContext.categories.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/slider/delete-slider")]
        public async Task<ActionResult> deleteSlider([FromForm] int id)
        {
            var item = await myDbContext.sliders.Where(x => x.Id == id).FirstAsync();
            myDbContext.sliders.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/market/delete-market")]
        public async Task<ActionResult> deleteMarket([FromForm] int id)
        {
            var item = await myDbContext.markets.Where(x => x.Id == id).FirstAsync();
            myDbContext.markets.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/food/delete-food")]
        public async Task<ActionResult> deleteFood([FromForm] int id)
        {
            var item = await myDbContext.foods.Where(x => x.Id == id).FirstAsync();
            myDbContext.foods.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/city/delete-city")]
        public async Task<ActionResult> deleteCity([FromForm] int id)
        {
            var item = await myDbContext.cites.Where(x => x.Id == id).FirstAsync();
            myDbContext.cites.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/field/delete-field")]
        public async Task<ActionResult> deleteField([FromForm] int id)
        {
            var item = await myDbContext.fields.Where(x => x.Id == id).FirstAsync();
            myDbContext.fields.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/order/delete-order")]
        public async Task<ActionResult> deleteOrder([FromForm] int id)
        {
            var item = await myDbContext.orders.Where(x => x.Id == id).FirstAsync();
            myDbContext.orders.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/option/delete-option")]
        public async Task<ActionResult> deleteOption([FromForm] int id)
        {
            var item = await myDbContext.options.Where(x => x.Id == id).FirstAsync();
            myDbContext.options.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }

        [HttpPost("dashboard/option-group/delete-option-group")]
        public async Task<ActionResult> deleteOptionGroup([FromForm] int id)
        {
            var item = await myDbContext.OptionGroups.Where(x => x.Id == id).FirstAsync();
            myDbContext.OptionGroups.Remove(item);
            myDbContext.SaveChanges();
            return Ok(id);
        }



        // ----------------------------------------------------------------------------------------

        [HttpPost("dashboard/category/update-category")]
        public async Task<ActionResult> updateCategory([FromForm] Category modle)
        {
            myDbContext.Entry(await myDbContext.categories.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/user/update-user")]
        public async Task<ActionResult> updateUser([FromForm] ApplicationUser modle)
        {
            myDbContext.Entry(await myDbContext.users.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/field/update-field")]
        public async Task<ActionResult> updateField([FromForm] Field modle)
        {
            myDbContext.Entry(await myDbContext.fields.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/food/update-food")]
        public async Task<ActionResult> updateFood([FromForm] Food modle)
        {
            myDbContext.Entry(await myDbContext.foods.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/order/update-order")]
        public async Task<ActionResult> updateOrder([FromForm] Order modle)
        {
            myDbContext.Entry(await myDbContext.orders.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/city/update-city")]
        public async Task<ActionResult> updateCity([FromForm] City modle)
        {
            myDbContext.Entry(await myDbContext.cites.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }
        [HttpPost("dashboard/option/update-option")]
        public async Task<ActionResult> updateOption([FromForm] Option modle)
        {
            myDbContext.Entry(await myDbContext.options.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/option-group/update-option-group")]
        public async Task<ActionResult> updateOptionGroup([FromForm] OptionGroup modle)
        {
            myDbContext.Entry(await myDbContext.OptionGroups.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }

        [HttpPost("dashboard/market/update-market")]
        public async Task<ActionResult> updateMarket([FromForm] Market modle)
        {
            myDbContext.Entry(await myDbContext.markets.FirstOrDefaultAsync(x => x.Id == modle.Id)).CurrentValues.SetValues(modle);
            myDbContext.SaveChanges();
            return Ok(modle);
        }


        [HttpPost("dashboard/image/upload")]
        public ActionResult uploadImage([FromForm] IFormFile file)
        {
            string path = _webHostEnvironment.WebRootPath + "uploads/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = System.IO.File.Create(path + file.FileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                //Image image = new Image()
                //{
                //    url = file.FileName
                //};
                return Ok(file.FileName);
            }

        }

    }
}
