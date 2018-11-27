using System;
namespace biovia.api.Model
{
    public class Neo4jEntities
    {
        public Neo4jEntities()
        {
        }
    }

    public class ProjectEntity : EntityBase
    {
        private string _projectid;
        private string _description;

        public string ProjectId {
            get {
                return _projectid;
            }
            set {
                _projectid = value;
            }
        }

        public string Description {
            get {
                return _description;
            }
            set {
                _description = value;
            }
        }
    }

    public class StudyEntity : EntityBase {
        private string _projectid;
        private string _studyid;
        private string _description;

        public string ProjectId
        {
            get
            {
                return _projectid;
            }
            set
            {
                _projectid = value;
            }
        }

        public string StudyId {
            get {
                return _studyid;
            }
            set {
                _studyid = value;
            }

        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
    }

    public class ExperimentEntity: EntityBase {

        private string _studyid;
        private string _experimentid;
        private int _index;
        private DateTime _creationdate;
        private int _sortIndex;

        public string StudyId
        {
            get
            {
                return _studyid;
            }
            set
            {
                _studyid = value;
            }

        }

        public string ExperimentId
        {
            get
            {
                return _experimentid;
            }
            set
            {
                _experimentid = value;
            }
        }

        public int Index {
            get {
                return _index;
            }
            set {
                _index = value;
            }
        }

        public DateTime CreationDate {
            get
            {
                return _creationdate;
            }
            set {
                _creationdate = value;
            }
        } 

        public int SortIndex{
            get {
                return _sortIndex;
            }
            set {
                _sortIndex = value;
            }
        }
    }
}
