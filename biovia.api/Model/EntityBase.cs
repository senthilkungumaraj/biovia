using System;
namespace biovia.api.Model
{
    public abstract class EntityBase
    {
        public int _id { get; set; }
        private string _name;

        public int Id {
            get {
                return _id;
            }
            set {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}
