
using tourism.api.Data;
using tourism.api.Entities;
using tourism.api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace tourism.api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [ResponseCache(Duration = 604800, Location = ResponseCacheLocation.Any)]
public class HomeController : ControllerBase
{

    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;
    private readonly DataContextProcedures _contextProcedures;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HomeController(ILogger<HomeController> logger,
                                DataContext context,
                                DataContextProcedures contextProcedures,
                                IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _context = context;
        _contextProcedures = contextProcedures;
        _httpContextAccessor = httpContextAccessor;
    }



    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<Tblcategory>>> GetCategories()
    {
        try{
        _logger.LogError("GetCategories()...");
        var category = _context.Tblcategories.Where(x => x.IsDeleted == 0).Select(x => new CategoryModel
        {
            Id = x.Id,
            Name = x.Name,
            Image = x.Image
        }
         ).ToList().AsEnumerable();

        return Ok(category);
        }
        catch(Exception ex)
        {
            _logger.LogError("get categories: " + ex.Message);
            return null;
        }
    }

    // GET: api/Home/articles
    [HttpGet("articles")]
    public async Task<ActionResult<IEnumerable<ArticleModel>>> GetArticles(int cityId)
    {
        _logger.LogError("GetArticles(cityId)...");

        var query = _contextProcedures.ArticlesList
            .FromSqlRaw("call prcGetArticles({0})", cityId)
            .ToList().AsEnumerable();

        return Ok(query);
    }

    // GET: api/Home/attractions
    [HttpGet("attractions")]
    public async Task<ActionResult<IEnumerable<AttractionModel>>> GetAttractions(int cityId)
    {
        _logger.LogError("GetAttractions(cityId)...");

        var query = _contextProcedures.AttractionsList
            .FromSqlRaw("call prcGetAttractions({0})", cityId)
            .ToList().AsEnumerable();

        return Ok(query);
    }

}