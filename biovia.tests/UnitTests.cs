using Microsoft.VisualStudio.TestTools.UnitTesting;
using biovia.api;
using System;
using System.Collections.Generic;

namespace biovia.tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void CreateProject()
        {
            RepositorySvc svc = new RepositorySvc();
            svc.CreateProject();
        }

        [TestMethod]
        public void CreateStudy()
        {
            RepositorySvc svc = new RepositorySvc();
            svc.CreateStudy();
        }

        [TestMethod]
        public void CreateExperiment()
        {
            RepositorySvc svc = new RepositorySvc();
            svc.CreateExperiment();
        }

        [TestMethod]
        public void GetProjectsById() {
            RepositorySvc svc = new RepositorySvc();
            var entity = svc.GetProjectById();
            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void GetStudiesByProjectId() {
            RepositorySvc svc = new RepositorySvc();
            var entity = svc.GetStudiesByProjectId();
            Assert.IsTrue(entity.Count > 0);
        }

        [TestMethod]
        public void GetExperiementsByStudyId() {
            RepositorySvc svc = new RepositorySvc();
            var entity = svc.GetExperimentsByStudyId();
            Assert.IsTrue(entity.Count > 0);
        }

        [TestMethod]
        public void DeleteProject() {
            RepositorySvc svc = new RepositorySvc();
            svc.DeleteProject();
        }

        [TestMethod]
        public void DeleteStudy() {
            RepositorySvc svc = new RepositorySvc();
            svc.DeleteStudy();
        }

        [TestMethod]
        public void DeleteExperiment() {
            RepositorySvc svc = new RepositorySvc();
            svc.DeleteExperiment();
        }

        [TestMethod]
        public void UpdateProject() {
            RepositorySvc svc = new RepositorySvc();
            svc.UpdateProject();
        }

        [TestMethod]
        public void UpdateExperiment()
        {
            RepositorySvc svc = new RepositorySvc();
            svc.UpdateExperiment();
        }

        [TestMethod]
        public void UpdateStudy() {
            RepositorySvc svc = new RepositorySvc();
            svc.UpdateStudy();
        }

        [TestMethod]
        public void SortExperiments() {
            RepositorySvc svc = new RepositorySvc();
            var result = svc.Sort();
            int cnt = result.Count;
        }

    }
}
