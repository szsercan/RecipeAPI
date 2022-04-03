using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Entity;
using System.Linq;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private DatabaseContext dbContext;
        public RecipeController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost("Add")]
        public IActionResult Add(Recipe recipe)
        {
            dbContext.Set<Recipe>().Add(recipe);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("Update")]
        public IActionResult Update(Recipe recipe)
        {
            dbContext.Set<Recipe>().Update(recipe);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("Delete")]
        public IActionResult Delete([FromBody]int Id)
        {
            var recipe = dbContext.Set<Recipe>().AsNoTracking().FirstOrDefault(e => e.Id == Id);
            dbContext.Set<Recipe>().Remove(recipe);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var data=dbContext.Set<Recipe>().AsNoTracking();
            return Ok(data);
        }

        [HttpGet("Get")]
        public IActionResult Get(int Id)
        {
            //var result = (from r in dbContext.Recipe
            //              join ri in dbContext.RecipeIngredient on r.Id equals ri.RecipeId into riGroup
            //             where r.Id == Id
            //             select new
            //             {
            //                 Id=r.Id,
            //                 Name=r.Name,
            //                 Description=r.Description,
            //                 ImagePath=r.ImagePath,
            //                 Ingredients=riGroup.ToArray()
            //             }).SingleOrDefault();

            //var result = (from r in dbContext.Recipe
            //              join ri in dbContext.RecipeIngredient on r.Id equals ri.RecipeId into riGroup
            //              where r.Id == Id
            //              select new
            //              {
            //                  Id = r.Id,

            //              }).SingleOrDefault();

            var result=dbContext.Set<Recipe>().AsNoTracking().Where(r => r.Id == Id).SingleOrDefault();

            return Ok(result);
        }
    }
}
