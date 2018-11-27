using System;
using System.Collections.Generic;
using Neo4j.Driver.V1;
using biovia.api.Model;
namespace biovia.api.Services
{
    public interface IRepository<T> where T:EntityBase
    {
        List<EntityBase> GetProjects();
        EntityBase GetProjectById(string projectid);
        void CreateProject(string projectid, string name, string description);
        void DeleteProject(string projectid);
        void UpdateProject(T entity);

        EntityBase GetStudyById(string studyid);
        List<EntityBase> GetStudiesByProjectId(string projectid);
        void CreateStudy(string projectid, string studyid, string name, string description);
        void DeleteStudy(string studyid);
        void UpdateStudy(T entity);

        EntityBase GetExperimentById(string expid);
        void UpdateSortIndexForExperiments(string studyId, string sortColumn, string sortOrder);
        void UpdateSortOrderForStudy(string studyId, string sortColumn, string sortOrder);
        List<EntityBase> GetExperiementsByStudyId(string studyid);
        void CreateExperiment(string studyid, string expid, string name, int index, DateTime creationdate, int sortindex);
        void DeleteExperiment(string expid);
        void UpdateExperiment(T entity);

    }
}
