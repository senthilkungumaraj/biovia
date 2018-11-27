using System;
using System.IO;
using System.Collections.Generic;
using Neo4j.Driver.V1;
using Neo4jClient;
using Newtonsoft.Json;
using biovia.seeder.Model;
using Microsoft.Extensions.Configuration;

namespace biovia.seeder
{
    class Program
    {
        public static GraphClient _client;
        public static IDriver _driver;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            IConfigurationSection graphDbSettings = configuration.GetSection("GraphDbSettings");

            string uri = graphDbSettings.GetValue<string>("BoltUrl");
            string username = graphDbSettings.GetValue<string>("UserName");
            string password = graphDbSettings.GetValue<string>("Password");
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
            _client = new GraphClient(new Uri(graphDbSettings.GetValue<string>("ClientUrl")), username, password);
            _client.Connect();

            List<Project> projects = LoadJson();
            foreach (Project project in projects)
            {
                CreateProject(project);

                foreach(Study study in project.Study) {
                    CreateStudyForProject(project.ID, study);

                    foreach(Experiment experiment in study.Experiments.Experiment) {
                        CreateExperimentForStudy(study.ID, experiment);
                    }
                }
            }
        }

        public static void CreateProject(Project project) {

            _client.Cypher
                .Create("(p:Project {Project})")
                .WithParam("Project", new
                {
                    name = project.Name,
                    description = project.Description,
                    projectid = project.ID
                })
                .ExecuteWithoutResults();

        }

        public static void CreateStudyForProject(string projectId, Study study) {

            _client.Cypher
                .Create("(s:Study {Study})")
                .WithParam("Study", new
                {
                    name = study.Identity.Name,
                    description = study.Identity.Description,
                    studyid = study.ID
                })
                .ExecuteWithoutResults();

            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var relation = tx.Run("MATCH (p:Project),(s:Study) " +
                                  "WHERE p.projectid = $projectid AND s.studyid = $studyid " +
                                  "CREATE(p) -[c:CONTAINS]->(s) " +
                                          "RETURN c", new { projectid = projectId, studyid = study.ID });
                });
            }
        }

        public static void CreateExperimentForStudy(string studyId, Experiment experiment) {
            _client.Cypher
                .Create("(e:Experiment {Experiment})")
                .WithParam("Experiment", new
                {
                    name = experiment.Name,
                    expid = experiment.ID,
                    studyid = studyId,
                    creationdate = experiment.CreationDate,
                    index = experiment.Index
                })
                .ExecuteWithoutResults();

            using (var session = _driver.Session())
            {
                session.WriteTransaction(tx =>
                {
                    var relation = tx.Run("MATCH (s:Study),(e:Experiment) " +
                                  "WHERE s.studyid = $studyid AND e.expid = $expid " +
                                  "CREATE(s) -[h:HAS]->(e) " +
                                          "RETURN h", new { studyid = studyId, expid = experiment.ID });
                });
            }
        }

        public static List<Project> LoadJson() {
            using (StreamReader r = new StreamReader("Data/sampledata.json"))
            {
                string json = r.ReadToEnd();
                List<Project> items = JsonConvert.DeserializeObject<List<Project>>(json);
                return items;
            }
        }

        public class ContainsRelationship : Relationship, IRelationshipAllowingSourceNode<object>, IRelationshipAllowingTargetNode<object>
        {
            public static readonly string TypeKey = "CONTAINS";

            public ContainsRelationship(NodeReference targetNode)
            : base(targetNode)
            { }

            public override string RelationshipTypeKey
            {
                get { return TypeKey; }
            }
        }
    }
}
