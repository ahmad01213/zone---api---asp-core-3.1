
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

        public class ApiController : Controller
        {
            private MyDBContext myDbContext;
            private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        IWebHostEnvironment _webHostEnvironment;
            public ApiController( MyDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)

            {
                myDbContext = context;
                _mapper = mapper;
                _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            }

     


            [HttpPost("image/upload")]
            public ActionResult uploadImage([FromForm]IFormFile file)
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

        // ----------------------------------------------------------------------------------------



       

            // ----------------------------------------------------------------------------------------

            



            // ----------------------------------------------------------------------------------------


            [HttpGet("mobile/get-home")]
            public async Task<ActionResult> getHome()
            {
                var fields =await myDbContext.fields.AsNoTracking().ToListAsync();
                var sliders = await myDbContext.sliders.AsNoTracking().ToListAsync();
                var mostrated = await myDbContext.markets.OrderByDescending(x => x.rate).Take(10).AsNoTracking().ToListAsync();
                var readyfordeliver = await myDbContext.markets.OrderByDescending(x => x.order_count).Take(10).AsNoTracking().ToListAsync();
                var toppicks = await myDbContext.markets.OrderByDescending(x => x.picks).Take(10).AsNoTracking().ToListAsync();
                return Ok( new { fields,mostrated,sliders,readyfordeliver,toppicks});
            }

        [HttpPost("mobile/get-top-foods")]
        public async Task<ActionResult> getTopfoodsOnResturant([FromForm]int id)
        {
         
            var toppicks = await myDbContext.foods.Where(x=>x.market_id == id).OrderByDescending(x => x.picks).Take(10).AsNoTracking().ToListAsync();
            return Ok(toppicks );
        }

   

        [HttpPost("mobile/get-field-markets")]
        public async Task<ActionResult> getFieldmarkets([FromForm]FieldMarketsRequest modle)
            {
            var myLat = modle.lat;
            var myLon = modle.lng;
            var radiusInMile = 1000000;
            var markets = myDbContext.markets
                   .AsEnumerable()
                   .Where(x=>x.field_id == modle.FieldId)
                   .Select(market => new { market, Dist = distanceInMiles(myLon, myLat, market.lng, market.lat) }).OrderBy(market => market.Dist)
                   .Where(p => p.Dist <= radiusInMile);

            return Ok(markets);
            }


            [HttpPost("mobile/get-market-foods")]
            public async Task<ActionResult> getMarketfoods([FromForm] int id)
            {
            var foods = await myDbContext.foods.Where(x => x.market_id == id).AsNoTracking().ToListAsync();
                return Ok(foods);
            }

        [HttpPost("mobile/add-cart")]
        public async Task<ActionResult> addCart([FromForm] CartAddRequest modle)
        {

            Cart cart = _mapper.Map<Cart>(modle);
          await myDbContext.carts.AddAsync(cart);
            myDbContext.SaveChanges();

            foreach (var item in modle.options)
            {
                CartGroupOption cartGroupOption = new CartGroupOption
                {
                    cart_id = cart.Id,
                    group_id = int.Parse(item.Key),
                    option_id = int.Parse(item.Value),
                   
                };
                await myDbContext.CartGroupoptions.AddAsync(cartGroupOption);

            }

            myDbContext.SaveChanges();
            return Ok(cart);
        }

        [HttpPost("mobile/get-food-detail")]
        public async Task<ActionResult> getCategoryfoods([FromForm] int id)
        {
            List<FoodDetailResponse> foodDetails = new List<FoodDetailResponse> { };
           var groups = await myDbContext.OptionGroups.Where(x => x.food_id == id).AsNoTracking().ToListAsync();
            foreach (var group in groups)
            {
                foodDetails.Add(new FoodDetailResponse
                {
                    optionGroup = group,
                    options = await myDbContext.options.Where(x => x.group_id == group.Id).AsNoTracking().ToListAsync()
            });
            }
                return Ok(foodDetails);
        }

        [HttpPost("mobile/get-category-foods")]
        public async Task<ActionResult> getCategoryfoods([FromForm] CategoryFoodRequest model)
        {
            var data = await myDbContext.foods.Where(x => x.market_id == model.marketId).Where(x => x.category_id == model.categoryId).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpPost("mobile/get-carts")]
        public async Task<ActionResult> getcarts([FromForm] int id)
        {
            var data = await myDbContext.carts.Where(x => x.user_id == id).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpPost("mobile/get-user-addresses")]
        public async Task<ActionResult> getAdresses([FromForm] int id)
        {
            var data = await myDbContext.addresses.Where(x => x.user_id == id).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpPost("mobile/add-address")]
        public async Task<ActionResult> addAdress([FromForm] Address modle)
        {

            await myDbContext.addresses.AddAsync(modle);
            myDbContext.SaveChanges();
            return Ok(modle);

        }

        [HttpPost("mobile/add-order")]
        public async Task<ActionResult> addOrder([FromForm] Order modle)
        { 
            int totalPrice = 0;
            List<Cart> carts = await myDbContext.carts.Where(x => x.user_id == modle.user_id).Where(x => x.order_id == 0).AsNoTracking().ToListAsync();
            carts.ForEach(c => { totalPrice += c.price; });
            modle.price = totalPrice;
            await myDbContext.orders.AddAsync(modle);
            myDbContext.SaveChanges();
            carts.ForEach(c => { c.order_id = modle.Id; });
            myDbContext.SaveChanges();
            return Ok(modle);

        }



        [HttpPost("mobile/user/login")]
            public async Task<ActionResult> Login([FromForm]LoginRequest loginRequest)
            {
                var customer =await myDbContext.users.Where(x => x.email == loginRequest.email).FirstAsync();

                if (customer == null)
                {
                    return Unauthorized("You Entered Invalid Email");
                }
                var passwordHash = HashingHelper.HashUsingPbkdf2(loginRequest.password, loginRequest.password);

                if (customer.password != passwordHash)
                {
                return Unauthorized("You Entered invalid password");
            }

            var token = await Task.Run(() => TokenHelper.GenerateToken(customer));

                return Ok(new
                {
                    token,
                    customer
                });
            }

        [HttpPost("mobile/user/register")]
        public async Task<ActionResult> registerUser([FromForm] ApplicationUser modle)
        {
            modle.password = HashingHelper.HashUsingPbkdf2(modle.password, modle.password);
            await myDbContext.users.AddAsync(modle);
            myDbContext.SaveChanges();
            var token = await Task.Run(() => TokenHelper.GenerateToken(modle));
            var customer = modle;

            return Ok(new
            {
                token,
                customer
            });
        }

        [HttpPost("mobile/driver/get-market-detail")]
        public async Task<ActionResult> getMarketDetails([FromForm] MarketDetailRequest modle)
        {
            var orders = await myDbContext.orders.Where(x=>x.market_id == modle.marketId&&x.status == 1).AsNoTracking().ToListAsync();

            Market market =await myDbContext.markets.Where(x=>x.Id == modle.marketId).FirstAsync();

            double distance = distanceInMiles(modle.lng, modle.lat, market.lng, market.lat);
            MarketDetailResponse data = new MarketDetailResponse()
            {
                market = market,
                distance = distance,
                orders = orders

            };
            return Ok(data);
        }




        [HttpPost("mobile/driver/get-order-detail")]
        public async Task<ActionResult> getOrderItems([FromForm] int id)
        {
            var items = myDbContext.carts.Where(x => x.order_id == id).Select(o => new { food =  myDbContext.foods.Where(x => x.Id == o.food_id).First(),quantity = o.quantity});
            var order =await myDbContext.orders.Where(x => x.Id == id).FirstAsync();
            return Ok(new { order, items });
        }


        [HttpPost("mobile/driver/get-orders")]
        public async Task<ActionResult> getCurrentorders([FromForm] int id)
        {
            var orders = await myDbContext.orders.Where(x => x.status < 4).AsNoTracking().ToListAsync();
            return Ok(orders);
        }

        [HttpPost("mobile/user/get-orders")]
        public async Task<ActionResult> getUserOrders([FromForm] int id)
        {
            var orders = await myDbContext.orders.Where(x => x.user_id ==id).AsNoTracking().ToListAsync();
            return Ok(orders);
        }


        [Authorize]
        [HttpPost("mobile/user/get-info")]

        public async Task<ActionResult> getCurrentUser([FromForm] int id)
        {
            var user = await myDbContext.users.FindAsync(id);
            return Ok(user);
        }



        [HttpPost("mobile/user/get-notifications")]
        public async Task<ActionResult> getUserNotifications([FromForm] int id)
        {
            var notifications = await myDbContext.UserNotifications.Where(x => x.user_id == id).AsNoTracking().ToListAsync();
            return Ok(notifications);
        }

        [HttpPost("mobile/driver/get-profile")]
        public async Task<ActionResult> getDriverProfile([FromForm] int id)
        {
            var driver = await myDbContext.drivers.Where(x => x.user_id == id).FirstAsync();
            return Ok(driver);
        }

        [HttpPost("mobile/driver/update-order-status")]
        public async Task<ActionResult> updateorderstatus([FromForm] OrderStatusRequest modle)
        {
            Order order = await myDbContext.orders.Where(x => x.Id == modle.orderId).FirstAsync();
            if (order.delivery_id == 0||order.delivery_id == modle.deliveryId) {
                order.delivery_id = modle.deliveryId;
                order.status = modle.status;
               await myDbContext.SaveChangesAsync();
                return Ok("تم بنجاح");

            }
            return Ok("نأسف تم إسناد الطلب لمندوب اخر");
        }


        // ------------------------------------ Driver

        [HttpPost("mobile/driver/nearby-markets")]
        public async Task<ActionResult> getDriverNearbymarkets([FromForm] NearbyMarketsRequest modle)
        {
            var myLat = modle.lat;
            var myLon = modle.lng;
            var radiusInMile = 1000000;

            //var minMilePerLat = 68.703;
            //var milePerLon = Math.Cos(myLat) * 69.172;
            //var minLat = myLat - radiusInMile / minMilePerLat;
            //var maxLat = myLat + radiusInMile / minMilePerLat;
            //var minLon = myLon - radiusInMile / milePerLon;
            //var maxLon = myLon + radiusInMile / milePerLon;

            var markets = myDbContext.markets
                   //.Where(market => (minLat <= market.lat && market.lat <= maxLat) && (minLon <= market.lng && market.lng <= maxLon))
                   .AsEnumerable()
                   .Select(market => new { market, Dist = distanceInMiles(myLon, myLat, market.lng, market.lat) }).OrderBy(market => market.Dist)
                   .Where(p => p.Dist <= radiusInMile);

            return Ok(markets);
        }


        [HttpPost("mobile/driver/search-markets")]
        public async Task<ActionResult> getSearchmarkets([FromForm] SearchMarketRequest searchMarket )
        {
            var myLat = searchMarket.lat;
            var myLon = searchMarket.lng;
            //var radiusInMile = 700000;

            //var minMilePerLat = 68.703;
            //var milePerLon = Math.Cos(myLat) * 69.172;
            //var minLat = myLat - radiusInMile / minMilePerLat;
            //var maxLat = myLat + radiusInMile / minMilePerLat;
            //var minLon = myLon - radiusInMile / milePerLon;
            //var maxLon = myLon + radiusInMile / milePerLon;

            var markets = myDbContext.markets
               .Where(p => p.title.Contains(searchMarket.searchText))
                   //.Where(market => (minLat <= market.lat && market.lat <= maxLat) && (minLon <= market.lng && market.lng <= maxLon))
                   .AsEnumerable()
                   .Select(market => new { market, Dist = distanceInMiles(myLon, myLat, market.lng, market.lat) }).OrderBy(market => market.Dist)
                   ;

            return Ok(markets);
        }



        public double ToRadians(double degrees) => degrees * Math.PI / 180.0;
        public double distanceInMiles(double lon1d, double lat1d, double lon2d, double lat2d)
        {
            var lon1 = ToRadians(lon1d);
            var lat1 = ToRadians(lat1d);
            var lon2 = ToRadians(lon2d);
            var lat2 = ToRadians(lat2d);

            var deltaLon = lon2 - lon1;
            var c = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon));
            var earthRadius = 3958.76;
            var distInMiles = earthRadius * c;

            return Math.Round(distInMiles, 2); 
        }
    }



}

