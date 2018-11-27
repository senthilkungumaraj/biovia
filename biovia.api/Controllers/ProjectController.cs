using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver.V1;
using biovia.api.Model;
using Microsoft.Extensions.Options;
namespace biovia.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private biovia.api.Services.IRepository<EntityBase> _service;
        private readonly IOptions<GraphDbSettings> _graphDbSettings;

        public ProjectsController(IOptions<GraphDbSettings> graphDbSettings) {
            _graphDbSettings = graphDbSettings;
            _service = new biovia.api.Services.Neo4jRepository<EntityBase>(_graphDbSettings.Value.BoltUrl, _graphDbSettings.Value.ClientUrl, _graphDbSettings.Value.UserName, _graphDbSettings.Value.Password);
        }

        // GET api/values
        [HttpGet]
        public ActionResult<List<EntityBase>> GetProjects()
        {

            return _service.GetProjects();
        }

        // GET api/values/5
        [HttpGet("{projectid}")]
        public ActionResult<EntityBase> GetProjectById(string projectid)
        {
            return _service.GetProjectById(projectid);
        }


        [HttpPost("{projectid}/studies/{studyid}/experiments")]
        public bool UpdateSortIndexForExperiments([FromBody] biovia.api.Services.SortRequest sortRequest) {
            _service.UpdateSortOrderForStudy(sortRequest.StudyId, sortRequest.SortColumn, sortRequest.SortOrder);
            return true;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET api/values
        [HttpGet("{projectid}/studies")]
        public ActionResult<List<EntityBase>> GetStudies(string projectid)
        {
            return _service.GetStudiesByProjectId(projectid);
        }

        // GET api/values
        [HttpGet("{projectid}/studies/{studyid}")]
        public ActionResult<EntityBase> GetStudyById(string studyid)
        {
            return _service.GetStudyById(studyid);
        }

        // GET api/values
        [HttpGet("{projectid}/studies/{studyid}/experiments")]
        public ActionResult<List<EntityBase>> GetExperiments(string studyid)
        {
            return _service.GetExperiementsByStudyId(studyid);
        }

        // GET api/values
        [HttpGet("{projectid}/studies/{studyid}/experiments/{experimentid}")]
        public ActionResult<EntityBase> GetExperimentById(string experimentid)
        {
            return _service.GetExperimentById(experimentid);
        }
    }
}
