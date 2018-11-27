using System;
using System.IO;
using System.Collections.Generic;
using biovia.api.Services;
using biovia.api.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace biovia.tests
{
    public class RepositorySvc
    {
        private readonly biovia.api.Services.Neo4jRepository<EntityBase> _neo4jRepo;

        public RepositorySvc()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            IConfigurationSection graphDbSettings = config.GetSection("GraphDbSettings");

            string boltUrl = graphDbSettings.GetValue<string>("BoltUrl");
            string clientUrl = graphDbSettings.GetValue<string>("ClientUrl");
            string username = graphDbSettings.GetValue<string>("UserName");
            string password = graphDbSettings.GetValue<string>("Password");
            _neo4jRepo = new biovia.api.Services.Neo4jRepository<EntityBase>(boltUrl, clientUrl, username, password);
        }

        public string CreateProject()
        {
            Random random = new Random();
            int randomNumber = random.Next();
            _neo4jRepo.CreateProject(string.Format("Proj{0}", randomNumber), string.Format("Proj Name {0}", randomNumber), string.Format("This is Proj {0}", randomNumber));
            return string.Format("Proj{0}", randomNumber);
        }

        public string CreateStudy()
        {
            string projId = CreateProject();
            return CreateStudy(projId);
        }

        public string CreateStudy(string projectid) 
        {
            Random random = new Random();
            int randomNumber = random.Next();
            _neo4jRepo.CreateStudy(projectid, string.Format("Study{0}", randomNumber), string.Format("Study Name {0}", randomNumber), string.Format("This is Study {0}", randomNumber));
            return string.Format("Study{0}", randomNumber);
        }

        public string CreateExperiment() {
            string studyId = CreateStudy();
            return CreateExperiment(studyId);
        }

        public string CreateExperiment(string studyid)
        {
            Random random = new Random();
            int randomNumber = random.Next();
            _neo4jRepo.CreateExperiment(studyid, string.Format("Exp{0}", randomNumber), string.Format("Experiment Name {0}", randomNumber), 0, DateTime.Now, 0);
            return string.Format("Exp{0}", randomNumber);
        }

        public EntityBase GetProjectById() {
            string projectid = CreateProject();
            return GetProjectById(projectid);
        }


        public EntityBase GetProjectById(string projectid) {
            return _neo4jRepo.GetProjectById(projectid);
        }

        public EntityBase GetStudyById(string studyid)
        {
            return _neo4jRepo.GetStudyById(studyid);
        }

        public EntityBase GetExperimentById(string expid) {
            return _neo4jRepo.GetExperimentById(expid);
        }

        public List<EntityBase> GetStudiesByProjectId() {

            string projectid = CreateProject();
            for (int i = 0; i < 5; i++) {
                CreateStudy(projectid);
            }
            return _neo4jRepo.GetStudiesByProjectId(projectid);
        }

        public List<EntityBase> GetExperimentsByStudyId()
        {
            string projectid = CreateProject();
            string studyid = CreateStudy(projectid);
            for (int i = 0; i < 5; i++) {
                CreateExperiment(studyid);
            }
            return _neo4jRepo.GetExperiementsByStudyId(studyid);
        }

        public void DeleteProject() {
            string projectid = CreateProject();
            _neo4jRepo.DeleteProject(projectid);
        }

        public void DeleteStudy() {
            string studyid = CreateStudy();
            _neo4jRepo.DeleteStudy(studyid);
        }

        public void DeleteExperiment() {
            string expid = CreateExperiment();
            _neo4jRepo.DeleteExperiment(expid);
        }

        public void UpdateProject() {
            string projectid = CreateProject();
            ProjectEntity project = (ProjectEntity)GetProjectById(projectid);
            project.Description = project.Description + " - Updated";
            project.Name = project.Name + " - Updated";
            _neo4jRepo.UpdateProject(project);

        }

        public void UpdateStudy() {
            string studyid = CreateStudy();
            StudyEntity study = (StudyEntity)GetStudyById(studyid);
            study.Description = study.Description + " - Updated";
            study.Name = study.Name + " - Updated";
            _neo4jRepo.UpdateStudy(study);
        }

        public void UpdateExperiment() {
            string expid = CreateExperiment();
            ExperimentEntity experiment = (ExperimentEntity)GetExperimentById(expid);
            experiment.Name = experiment.Name + " - Updated";
            _neo4jRepo.UpdateExperiment(experiment);
        }

        public List<EntityBase> Sort() {

            List<ExperimentEntity> expEntities = new List<ExperimentEntity>();


            ExperimentEntity entity1 = new ExperimentEntity();
            entity1.Id = 1;
            entity1.Name = "Exp 001";
            entity1.ExperimentId = "Exp001";
            entity1.StudyId = "Study001";
            expEntities.Add(entity1);

            ExperimentEntity entity2 = new ExperimentEntity();
            entity2.Id = 3;
            entity2.Name = "Exp 003";
            entity2.ExperimentId = "Exp003";
            entity2.StudyId = "Study003";
            expEntities.Add(entity2);

            ExperimentEntity entity3 = new ExperimentEntity();
            entity3.Id = 2;
            entity3.Name = "Exp 002";
            entity3.ExperimentId = "Exp002";
            entity3.StudyId = "Study002";
            expEntities.Add(entity3);

            ISortByColumnStrategy<ExperimentEntity> sortStrategy = new DynamicSortStrategy<ExperimentEntity>(expEntities, "Name", "asc");
            sortStrategy.Sort();
            return (List<EntityBase>)sortStrategy;
        }
    }
}
