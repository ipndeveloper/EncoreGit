using System;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Like Triplet but with a fourth object :-)
    /// Created: 09-21-2009
    /// </summary>
    [Serializable]
    public sealed class Quadruplet
    {
        public object First;
        public object Second;
        public object Third;
        public object Fourth;

        public Quadruplet()
        {

        }

        public Quadruplet(object x, object y)
        {
            First = x;
            Second = y;
        }

        public Quadruplet(object x, object y, object z)
        {
            First = x;
            Second = y;
            Third = z;

        }

        public Quadruplet(object x, object y, object z, object t)
        {
            First = x;
            Second = y;
            Third = z;
            Fourth = t;

        }
    }
}
