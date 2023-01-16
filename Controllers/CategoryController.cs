
using tourism.api.Data;
using tourism.api.Entities;
using tourism.api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace tourism.api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [ResponseCache(Duration = 604800, Location = ResponseCacheLocation.Any)]
public class CategoryController : ControllerBase
{

    private readonly ILogger<CategoryController> _logger;
    private readonly DataContext _context;
    private readonly DataContextProcedures _contextProcedures;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CategoryController(ILogger<CategoryController> logger,
                                DataContext context,
                                DataContextProcedures contextProcedures,
                                IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _context = context;
        _contextProcedures = contextProcedures;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: api/Category/items
    [HttpGet("items")]
    public async Task<ActionResult<IEnumerable<ItemsListModel>>> GetItems(int categoryId, int tagId, int sponsor,
    string phone, int favorite, double locationlat, double locationlong)
    {
        //var query;
        IEnumerable<ItemsListModel> query = null;
        if (favorite == 1)
        {
            _logger.LogError("GetFavoriteItems(phone,favorite)...");
            query = _contextProcedures.ItemsList
                .FromSqlRaw("call prcGetFavorites({0},{1})", phone, favorite)
                .ToList().AsEnumerable();
        }
        else
        {
            _logger.LogError("GetItems(categoryId,tagId)...");
            query = _contextProcedures.ItemsList
                .FromSqlRaw("call prcGetItems({0},{1},{2})", categoryId, tagId, sponsor)
                .ToList().AsEnumerable();
        }

        var fetchedItems = new List<ItemsListModel>();

        foreach (var item in query)
        {
            var _item = new ItemsListModel();
            _item.Id = item.Id;
            _item.Name = item.Name;
            _item.Ratings = item.Ratings;
            _item.Reviews = item.Reviews;
            _item.Type = item.Type;
            _item.Image = item.Image;
            _item.Description = item.Description;
            _item.CategoryId = item.CategoryId;
            _item.CategoryName = item.CategoryName;
            _item.CityId = item.CityId;
            _item.City = item.City;
            _item.Offer = item.Offer;
            _item.Locationlat = item.Locationlat;
            _item.Locationlong = item.Locationlong;
            _item.Website = item.Website;
            _item.Phone = item.Phone;
            // _item.Sponsored = item.Sponsored;
            // _item.OfferId=item.OfferId;
            // _item.OfferName=item.OfferName;
            // _item.OfferDescription=item.OfferDescription;
            // _item.StartDate=item.StartDate;
            // _item.EndDate=item.EndDate;

            var currentLat = Math.PI * locationlat / 180;
            var currentLong = locationlong;

            var fetchedLat = item.Locationlat;
            var fetchedLong = item.Locationlong;


            static double GetDistanceFromLocation(double lat1, double long1, double? lat2, double? long2)
            {
                var lat_2 = Convert.ToDouble(Math.PI * lat2 / 180);
                var long_2 = Convert.ToDouble(long2);

                var distance =
                Math.Sin(lat1) * Math.Sin(lat_2) + Math.Cos(lat1) *
                Math.Cos(lat_2) * Math.Cos(Math.PI * (long1 - long_2) / 180);
                distance = Math.Acos(distance);
                distance = distance * 180 / Math.PI;
                distance = (distance * 60 * 1.1515) * 1.609344;   // miles * 1.609344 = kms

                distance = Math.Round(distance, 1);

                return distance;
            }

            var distance = GetDistanceFromLocation(currentLat, currentLong, fetchedLat, fetchedLong);
            _item.Distance = distance;

            fetchedItems.Add(_item);
        }
        var distinctList = fetchedItems.OrderBy(x => x.Distance)
                        .DistinctBy(x => x.Id);

        return Ok(distinctList);
    }

    // GET: api/Category/itemsweb
    [HttpGet("web-items")]
    public async Task<ActionResult<IEnumerable<WebItemsListModel>>> GetWebItems(int categoryId, int tagId, int sponsor,
    string phone, int favorite, double locationlat, double locationlong)
    {
        //var query;
        IEnumerable<WebItemsListModel> query = null;
        if (favorite == 1)
        {
            _logger.LogError("GetFavoriteItems(phone,favorite)...");
            query = _contextProcedures.WebItemsList
                .FromSqlRaw("call prcGetFavorites({0},{1})", phone, favorite)
                .ToList().AsEnumerable();
        }
        else
        {
            _logger.LogError("GetItems(categoryId,tagId)...");
            query = _contextProcedures.WebItemsList
                .FromSqlRaw("call prcGetWebItems({0},{1},{2})", categoryId, tagId, sponsor)
                .ToList().AsEnumerable();
        }

        var fetchedItems = new List<WebItemsListModel>();

        foreach (var item in query)
        {
            var _item = new WebItemsListModel();
            _item.Id = item.Id;
            _item.Name = item.Name;
            _item.Ratings = item.Ratings;
            _item.Reviews = item.Reviews;
            _item.Type = item.Type;
            _item.Image = item.Image;
            _item.Description = item.Description;
            _item.CategoryId = item.CategoryId;
            _item.CategoryName = item.CategoryName;
            _item.CityId = item.CityId;
            _item.City = item.City;
            _item.Offer = item.Offer;
            _item.Locationlat = item.Locationlat;
            _item.Locationlong = item.Locationlong;
            _item.Website = item.Website;
            _item.Phone = item.Phone;
            _item.Sponsored = item.Sponsored;
            _item.OfferId=item.OfferId;
            _item.OfferName=item.OfferName;
            _item.OfferDescription=item.OfferDescription;
            _item.StartDate=item.StartDate;
            _item.EndDate=item.EndDate;
             _item.Tags=item.Tags;

            var currentLat = Math.PI * locationlat / 180;
            var currentLong = locationlong;

            var fetchedLat = item.Locationlat;
            var fetchedLong = item.Locationlong;


            static double GetDistanceFromLocation(double lat1, double long1, double? lat2, double? long2)
            {
                var lat_2 = Convert.ToDouble(Math.PI * lat2 / 180);
                var long_2 = Convert.ToDouble(long2);

                var distance =
                Math.Sin(lat1) * Math.Sin(lat_2) + Math.Cos(lat1) *
                Math.Cos(lat_2) * Math.Cos(Math.PI * (long1 - long_2) / 180);
                distance = Math.Acos(distance);
                distance = distance * 180 / Math.PI;
                distance = (distance * 60 * 1.1515) * 1.609344;   // miles * 1.609344 = kms

                distance = Math.Round(distance, 1);

                return distance;
            }

            var distance = GetDistanceFromLocation(currentLat, currentLong, fetchedLat, fetchedLong);
            _item.Distance = distance;

            fetchedItems.Add(_item);
        }
        var distinctList = fetchedItems.OrderBy(x => x.Distance)
                        .DistinctBy(x => x.Id);

        return Ok(distinctList);
    }

    // GET: api/Category/item
    [HttpGet("item")]
    public async Task<ActionResult<IEnumerable<ItemModel>>> GetItem(string phone, int itemId)
    {
        _logger.LogError("GetItem(itemId)...");

        var query = _contextProcedures.ItemList
            .FromSqlRaw("call prcGetItem({0},{1})", phone, itemId)
            .ToList().AsEnumerable();

        var result = query.First();

        return Ok(result);
    }

    // GET: api/Category/tags
    [HttpGet("tags")]
    public async Task<ActionResult<IEnumerable<TagModel>>> GetTags(int categoryId)
    {
        _logger.LogError("GetTags(categoryId)...");

        var query = _contextProcedures.TagsList
            .FromSqlRaw("call prcGetTags({0})", categoryId)
            .ToList().AsEnumerable();
        return Ok(query);
    }

    // Get branches
    [HttpGet("branches")]
    public async Task<ActionResult<IEnumerable<BranchModel>>> GetBranches(int itemId, int cityId)
    {
        _logger.LogError("GetBranches()...");
        var query = _contextProcedures.BranchesList
        .FromSqlRaw("call prcGetBranches({0},{1})", itemId, cityId)
                .ToList().AsEnumerable();
        return Ok(query);
    }

    // GET: api/Item/offerdetails
    [HttpGet("offers")]
    public async Task<ActionResult<IEnumerable<OfferModel>>> GetOffers(int ItemId, int CityId)
    {
        _logger.LogError("GetOffers()...");
        var query = _contextProcedures.OffersList
             .FromSqlRaw("call prcGetOffers({0},{1})", ItemId, CityId)
             .ToList().AsEnumerable();

        return Ok(query);
    }



    // Get popular destinations
    [HttpGet("popular-destinations")]
    public async Task<ActionResult<IEnumerable<CityModel>>> GetDestinations(int categoryId)
    {
        _logger.LogError("GetDestinations()...");
        var query = _contextProcedures.CitiesList
                .FromSqlRaw("call prcGetCities({0})", categoryId)
                .ToList().AsEnumerable();
        return Ok(query);
    }

    [HttpPost("insert-favorite")]
    public IActionResult InsertFavorite(string phone, int itemId, int favorite)
    {
        _logger.LogError("GetCategories()...");
        // return await _context.Tblcategories.ToListAsync();   //taking all data from table

        _contextProcedures.Database
            .ExecuteSqlRaw("call prcInsertFavorite({0},{1},{2})", phone, itemId, favorite);

        return Ok("success");

    }

    [HttpPost("insert-tag")]
    public IActionResult InsertTag(int tagId,string tagName, int categoryId)
    {
        try
        
        {
            _contextProcedures.Database
         .ExecuteSqlRaw("call prcInsertTag({0},{1},{2})",tagId,tagName,categoryId);
        return Ok("success");

        }
        

         catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong inside Update User action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }

    }

    [HttpPost("insert-category")]
    public IActionResult InsertCategory([FromBody] CategoryModel model)
    {
        try
        {
            if (model is null)
            {
                _logger.LogError("model is null.");
                return BadRequest("model object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }


            Console.WriteLine("0");

            if (model.Image != "")
            {
                var binary = model.Image;
                model.Image = Guid.NewGuid().ToString() + ".png";
                Console.WriteLine("filename:" + model.Image);
                System.IO.File.WriteAllBytes("Resources/images/" + model.Image, Convert.FromBase64String(binary));
            }

            Console.WriteLine("filename:" + model.Image);
            _contextProcedures.Database
                .ExecuteSqlRaw("call prcInsertCategory({0},{1},{2})", model.Id, model.Name, model.Image);
            return Ok(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong inside Update User action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }

    }


    [HttpPost("delete-category")]
    public ActionResult<ResponseModel> DeleteCategory(int categoryId)
    {
          try{
             var model = new ResponseModel();
        _logger.LogError("DeleteCategory()...");
        _contextProcedures.Database
         .ExecuteSqlRaw("call prcDeleteCategory({0})", categoryId);
        //return Ok("success");
          model.Status = "success";
          model.Code = "200";            
          return model;
          }
        catch(Exception ex)
        {
            _logger.LogError("delete categories: " + ex.Message);
            return null;
        }

    }
     [HttpPost("delete-tag")]
    public ActionResult<ResponseModel> DeleteTag(int tagid)
    {
         var model = new ResponseModel();
        _logger.LogError("DeleteTag()...");
        _contextProcedures.Database
         .ExecuteSqlRaw("call prcDeleteTag({0})", tagid);
        //return Ok("success");
         model.Status = "success";
         model.Code = "200";            
         return model;

    }

      [HttpPost("delete-item")]
    public ActionResult<ResponseModel> DeleteItem(int itemId)
    {
        var model = new ResponseModel();
        _logger.LogError("DeleteItem()...");
        _contextProcedures.Database
         .ExecuteSqlRaw("call prcDeleteItem({0})", itemId);
        //return Ok("success");
        model.Status = "success";
        model.Code = "200";            
        return model;

    }

     [HttpPost("delete-branch")]
    public ActionResult<ResponseModel> DeleteBranch(int branchId)
    {
         var model = new ResponseModel();
        _logger.LogError("DeleteBranch()...");
        _contextProcedures.Database
         .ExecuteSqlRaw("call prcDeleteBranch({0})", branchId);
        //return Ok("success");
        model.Status = "success";
        model.Code = "200";            
        return model;

    }

    [HttpPost("insert-item")]
    public async Task<ActionResult<IEnumerable<InsertItemModel>>> InsertItem([FromBody] InsertItemModel model)
    // public IActionResult InsertItem([FromBody] InsertItemModel model)
    {
       
        try
        {
            if (model is null)
            {
                _logger.LogError("model is null.");
                return BadRequest("model object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }


            Console.WriteLine("0");
            var binary = model.image;
            model.filename = Guid.NewGuid().ToString() + ".png";
            Console.WriteLine("id:" + model.id);
            Console.WriteLine("filename:" + model.filename);
            Console.WriteLine("tags:" + model.tags);
             System.IO.File.WriteAllBytes("Resources/images/" + model.filename, Convert.FromBase64String(binary));
        //   /  InsertItemsList
            //  List<InsertItemModel> ResultList = new List<InsertItemModel>();
            var query= _contextProcedures.InsertItemsList
            .FromSqlRaw("call prcInsertItems({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",model.id, model.name, model.offerid
           , model.description, model.ratings, model.reviews, model.categoryid, model.type, model.sponsored,
             model.filename,model.tags)
            .ToList().AsEnumerable();
            // Console.WriteLine(ResultList.id);
            // Console.WriteLine(query.Count());
            // foreach(var item in query){
            //     Console.Write(item.itemid);
            // }
         
         

        //    var query=_contextProcedures.Database.ExecuteSqlRaw("call prcInsertItems({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
        //     model.id ,model.name, model.description,
        //     model.ratings, model.reviews, model.categoryid, model.type, model.sponsor, model.filename,model.tags);
        //     Console.WriteLine("1");
      

            return Ok(query);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong  action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPost("insert-branch")]
    public IActionResult Insertbranch([FromBody] InsertBranchModel model)
    {
        try
        {
            if (model is null)
            {
                _logger.LogError("model is null.");
                return BadRequest("model object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }

            Console.WriteLine("0");
           _contextProcedures.Database
            .ExecuteSqlRaw("call prcInsertBranches({0},{1},{2},{3},{4},{5},{6},{7},{8})",model.itemid ,model.branchname, model.locationaddress,
            model.latitude, model.longitude , model.isdefault, model.website, model.phone, model.cityid );
            Console.WriteLine("1");
      

            return Ok(model);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong  action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("insert-discount")]
    public IActionResult Insertdiscount([FromBody] InsertDiscountModel model)
    {
        try
        {
            if (model is null)
            {
                _logger.LogError("model is null.");
                return BadRequest("model object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }

            Console.WriteLine("0");
           _contextProcedures.Database
            .ExecuteSqlRaw("call prcInsertDiscounts({0},{1},{2},{3},{4},{5})",model.itemid ,model.discountname, 
            model.discountdescription,model.discountrate, model.startdate , model.enddate );
            
            Console.WriteLine("1");
      

            return Ok(model);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong  action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


      // GET: api/Category/get-cities
    [HttpGet("get-cities")]
    public async Task<ActionResult<IEnumerable<AllCitiesModel>>> GetCities()
    {
        _logger.LogError("GetCities()...");

        var query = _contextProcedures.AllCitiesList
            .FromSqlRaw("call prcGetAllCities()")
            .ToList().AsEnumerable();
        return Ok(query);
    }

     // Get branches
    [HttpGet("get-branches")]
    public async Task<ActionResult<IEnumerable<GetBranchModel>>> GetAllBranches(int itemId)
    {
        _logger.LogError("GetBranches()...");
        var query = _contextProcedures.GetBranchesList
        .FromSqlRaw("call prcGetItemBranches({0})", itemId)
                .ToList().AsEnumerable();
        return Ok(query);
    }

     [HttpGet("get-discounts")]
    public async Task<ActionResult<IEnumerable<OfferModel>>> GetAlldiscounts(int itemId)
    {
        _logger.LogError("GetBranches()...");
        var query = _contextProcedures.OffersList
        .FromSqlRaw("call prcGetItemDiscounts({0})", itemId)
                .ToList().AsEnumerable();
        return Ok(query);
    }

     [HttpPost("delete-discount")]
    public ActionResult<ResponseModel> DeleteDiscount(int discountId)
    {
         var model = new ResponseModel();
        _logger.LogError("DeleteDiscount()...");
        _contextProcedures.Database
         .ExecuteSqlRaw("call prcDeleteDiscount({0})", discountId);
        //return Ok("success");
          model.Status = "success";
        model.Code = "200";            
        return model;

    }

}