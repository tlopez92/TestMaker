using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;
using System.Collections.Generic;
using TestMakerFreeWeb.Data;
using System.Linq;
using Mapster;
using TestMakerFreeWeb.Controllers;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : BaseApiController
    {
        #region private fields
        private ApplicationDbContext dbContext;
        #endregion

        #region Constructors
        public ResultController(ApplicationDbContext context) : base(context)
        {
        }
        #endregion
        #region RESTful conventions methods
        /// <summary>
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Result</param>
        /// <returns>the Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = this.dbContext.Results.Where(r => r.Id == id).FirstOrDefault();

            if(result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }

            return new JsonResult(result.Adapt<ResultViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Adds a new Result to the Database
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var result = model.Adapt<Result>();

            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;

            this.dbContext.Results.Add(result);

            this.dbContext.SaveChanges();

            return new JsonResult(result.Adapt<ResultViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Edit the Result with the given {id}
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var result = this.dbContext.Results.Where(r => r.Id == model.Id).FirstOrDefault();

            if(result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", model.Id)
                });
            }

            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;

            result.LastModifiedDate = DateTime.Now;

            this.dbContext.SaveChanges();

            return new JsonResult(result.Adapt<ResultViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Deletes the Result with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Result</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = this.dbContext.Results.Where(r => r.Id == id).FirstOrDefault();

            if(result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }

            this.dbContext.Results.Remove(result);

            this.dbContext.SaveChanges();

            return new OkResult();
        }
        #endregion

        // GET api/question/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var results = this.dbContext.Results.Where(r => r.QuizId == quizId).ToArray();

            return new JsonResult(
            results.Adapt<ResultViewModel[]>(), jsonSettings);
        }
    }
}

