using System;
using System.Collections.Generic;
using Neo4j.Driver.V1;
using System.Linq;
using Neo4jClient;
using biovia.api.Model;
using Microsoft.Extensions.Options;
namespace biovia.api.Services
{
    public class Neo4jRepository<T> : IRepository<T> where T : EntityBase
    {
        public readonly IDriver _driver;
        public readonly GraphClient _client;


        public Neo4jRepository(string boltUrl, string clientUrl, string username, string password) {

            _driver = GraphDatabase.Driver(boltUrl, AuthTokens.Basic(username, password));
            _client = new GraphClient(new Uri(clientUrl), username, password);
            _client.Connect();
        }

        public void CreateExperiment(string studyid, string expid, string name, int index, DateTime creationdate, int sortindex)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (e:Experiment { studyid: $studyid, expid: $expid, name: $name, index: $index, creationdate: $creationdate, sortindex: $sortindex});", new { studyid, expid, name, index, creationdate, sortindex });
                    var relation = tx.Run("MATCH (s:Study),(e:Experiment) " +
                                          "WHERE e.expid = $expid AND s.studyid = $studyid " +
                                          "CREATE(s) -[h:HAS]->(e) " +
                                          "RETURN h", new { expid, studyid });
                });
            }
        }

        public void CreateProject(string projectid, string name, string description)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (p:Project { projectid: $projectid, name: $name, description: $description});", new { projectid, name, description });

                });
            }
        }

        public void CreateStudy(string projectid, string studyid, string name, string description)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (s:Study { projectid: $projectid, studyid: $studyid, name: $name, description: $description});", new { projectid, studyid, name, description });
                    var relation = tx.Run("MATCH (p:Project),(s:Study) " +
                                          "WHERE p.projectid = $projectid AND s.studyid = $studyid " +
                                          "CREATE(p) -[c:CONTAINS]->(s) " +
                                          "RETURN c", new { projectid, studyid });
                });
            }
        }

        public void DeleteExperiment(string expid)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (e:Experiment) " +
                                   "WHERE e.expid = $expid " +
                                   "DETACH DELETE e", new { expid });

                });
            }
        }

        public void DeleteProject(string projectid)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (p:Project) " +
                                   "WHERE p.projectid = $projectid " +
                                   "DETACH DELETE p", new { projectid });

                });
            }
        }

        public void DeleteStudy(string studyid)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (s:Study) " +
                                   "WHERE s.studyid = $studyid " +
                                   "DETACH DELETE s", new { studyid });

                });
            }
        }

        public List<EntityBase> GetExperiementsByStudyId(string studyid)
        {
            List<EntityBase> experiments = new List<EntityBase>();
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {

                    var result = tx.Run("MATCH (s:Study {studyid: {studyid}})-[h:HAS]-(e:Experiment)" +
                                        "RETURN e", new { studyid });

                    foreach (var record in result)
                    {
                        //Get as an INode instance to access properties.
                        var node = record["e"].As<Neo4j.Driver.V1.INode>();
                        ExperimentEntity experiment = new ExperimentEntity
                        {
                            ExperimentId = node["expid"].As<string>(),
                            Name = node.Properties.ContainsKey("name") ? node["name"].As<string>(): "Untitled",
                            Index = node.Properties.ContainsKey("index") ? node["index"].As<int>() : 0,
                            CreationDate = node.Properties.ContainsKey("creationdate") ? node["creationdate"].As<DateTime>(): DateTime.MinValue,
                            SortIndex = node.Properties.ContainsKey("sortindex") ? node["sortindex"].As<int>() : 0,
                            Id = node.Id.As<int>()

                        };
                        experiments.Add(experiment);
                    }

                });
            }

            return experiments;
        }

        public EntityBase GetExperimentById(string expid)
        {

            ExperimentEntity experiment = null;
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (e:Experiment) " +
                                   "WHERE e.expid = $expid " +
                                   "RETURN e", new { expid });

                    foreach (var record in result)
                    {
                        //Get as an INode instance to access properties.
                        var node = record["e"].As<Neo4j.Driver.V1.INode>();

                        experiment = new ExperimentEntity
                        {
                            StudyId = node["studyid"].As<string>(),
                            ExperimentId = node["expid"].As<string>(),
                            Name = node.Properties.ContainsKey("name") ? node["name"].As<string>() : "Untitled",
                            Index = node.Properties.ContainsKey("index") ? node["index"].As<int>() : 0,
                            CreationDate = node.Properties.ContainsKey("creationdate") ? node["creationdate"].As<DateTime>() : DateTime.MinValue,
                            SortIndex = node.Properties.ContainsKey("index") ? node["sortindex"].As<int>() : 0,
                            Id = node.Id.As<int>()

                        };

                    }
                });
            }

            return experiment;
        }

        public EntityBase GetProjectById(string projectid)
        {
            ProjectEntity project = null;
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (p:Project) " +
                                   "WHERE p.projectid = $projectid " +
                                   "RETURN p", new { projectid });

                    foreach(var record in result) {
                        //Get as an INode instance to access properties.
                        var node = record["p"].As<Neo4j.Driver.V1.INode>();

                        project = new ProjectEntity
                        {
                            ProjectId = node["projectid"].As<string>(),
                            Name = node["name"].As<string>(),
                            Description = node["description"].As<string>(),
                            Id = node.Id.As<int>()

                        };

                    }
                });
            }

            return project;
        }

        public List<EntityBase> GetProjects()
        {
            List<EntityBase> projects = new List<EntityBase>();
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (p:Project) " +
                                   "RETURN p", new { });

                    foreach (var record in result)
                    {
                        //Get as an INode instance to access properties.
                        var node = record["p"].As<Neo4j.Driver.V1.INode>();
                        ProjectEntity project = new ProjectEntity
                        {
                            ProjectId = node["projectid"].As<string>(),
                            Name = node["name"].As<string>(),
                            Description = node["description"].As<string>(),
                            Id = node.Id.As<int>()

                        };
                        projects.Add(project);
                    }

                });
            }

            return projects;
        }

        public List<EntityBase> GetStudiesByProjectId(string projectid)
        {
            List<EntityBase> studies = new List<EntityBase>();
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (p:Project {projectid: {projectid}})-[c:CONTAINS]-(s:Study)" +
                                        "RETURN s", new { projectid });


                    foreach (var record in result)
                    {
                        //Get as an INode instance to access properties.
                        var node = record["s"].As<Neo4j.Driver.V1.INode>();
                        StudyEntity study = new StudyEntity
                        {
                            StudyId = node["studyid"].As<string>(),
                            Name = node["name"].As<string>(),
                            Description = node["description"].As<string>(),
                            Id = node.Id.As<int>()

                        };
                        studies.Add(study);
                    }

                });
            }

            return studies;
        }

        public EntityBase GetStudyById(string studyid)
        {
            StudyEntity study = null;
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (s:Study) " +
                                   "WHERE s.studyid = $studyid " +
                                   "RETURN s", new { studyid });

                    foreach (var record in result)
                    {
                        //Get as an INode instance to access properties.
                        var node = record["s"].As<Neo4j.Driver.V1.INode>();

                        study = new StudyEntity
                        {
                            StudyId = node["studyid"].As<string>(),
                            Name = node["name"].As<string>(),
                            Description = node["description"].As<string>(),
                            Id = node.Id.As<int>()

                        };

                    }
                });
            }

            return study;
        }

        public void UpdateExperiment(T entity)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (e:Experiment) " +
                                        "WHERE e.expid = $expid " +
                                        "SET e.name = $name " +
                                        "RETURN e", new { expid = entity.As<ExperimentEntity>().ExperimentId, name = entity.As<ExperimentEntity>().Name });
                });
            }
        }

        public void UpdateProject(T entity)
        {
            using(var session = _driver.Session()) {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (p:Project) " +
                                        "WHERE p.projectid = $projectid " +
                                        "SET p.name = $name, p.description = $description " +
                                        "RETURN p", new { projectid = entity.As<ProjectEntity>().ProjectId, name = entity.As<ProjectEntity>().Name, description = entity.As<ProjectEntity>().Description });
                });
            }
        }

        public void UpdateSortIndexForExperiments(string studyId, string sortColumn, string sortOrder)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    List<ExperimentEntity> entities = GetExperiementsByStudyId(studyId).As<List<ExperimentEntity>>();
                    ISortByColumnStrategy<ExperimentEntity> sortStrategy = new DynamicSortStrategy<ExperimentEntity>(entities, sortColumn, sortOrder);
                    sortStrategy.Sort();

                    _client.Cypher.Unwind(entities, "row")
                           .Match("(e:Experiment { expid: row.ExperimentId })")
                           .Set("e.sortindex = row.SortIndex")
                           .ExecuteWithoutResults();

                });
            }
        }

        public void UpdateSortOrderForStudy(string studyId, string sortColumn, string sortOrder)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                var result = tx.Run("MATCH (s:Study) " +
                                    "WHERE s.studyid = $studyid " +
                                    "SET s.orderby = $orderby " +
                                    "RETURN s", new { studyid = studyId, orderby = string.Format("{0};{1}", sortColumn,sortOrder) });

                });
            }
        }

        public void UpdateStudy(T entity)
        {
            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var result = tx.Run("MATCH (s:Study) " +
                                        "WHERE s.studyid = $studyid " +
                                        "SET s.name = $name, s.description = $description " +
                                        "RETURN s", new { studyid = entity.As<StudyEntity>().StudyId, name = entity.As<StudyEntity>().Name, description = entity.As<StudyEntity>().Description });
                });
            }
        }
    }
}
