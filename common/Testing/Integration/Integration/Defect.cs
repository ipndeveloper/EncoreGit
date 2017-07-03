using System;

namespace NetSteps.Testing.Integration
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Defect : Attribute
    {
        int _id;
        DefectStatus _status;

        public Defect(int id)
        {
            _id = id;
            _status = DefectStatus.Open;
        }

        public Defect(int id, DefectStatus status)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }

        public DefectStatus Status
        {
            get { return _status; }
        }
    }

    public enum DefectStatus
    {
        Open,
        Closed
    }
}
