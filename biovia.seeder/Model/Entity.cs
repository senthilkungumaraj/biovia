using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace biovia.seeder.Model
{
    public class Project
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("Study")]
        public List<Study> Study { get; set; }
    }

    public class Experiments
    {
        [JsonProperty("OrderBy")]
        public string OrderBy { get; set; }
        [JsonProperty("Experiment")]
        public List<Experiment> Experiment { get; set; }
    }

    public class Study
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("Identity")]
        public Identity Identity { get; set; }
        [JsonProperty("Experiments")]
        public Experiments Experiments { get; set; }
    }

    public class Identity
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }

    }

    public class Experiment
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("CreationDate")]
        public string CreationDate { get; set; }
        [JsonProperty("Index")]
        public string Index { get; set; }
        [JsonProperty("ID")]
        public string ID { get; set; }
    }
}
