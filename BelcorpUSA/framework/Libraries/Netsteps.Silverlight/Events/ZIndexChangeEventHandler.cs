using System;

namespace NetSteps.Silverlight
{
    #region Enums
    public static class Enums
    {
        public enum Direction
        {
            NotSet,
            Down,
            Up
        }
    }
    #endregion

    public delegate void ZIndexChangeEventHandler(object sender, ZIndexChangeEventArgs e);

    public class ZIndexChangeEventArgs : EventArgs
    {
        public string ControlName { get; protected set; }
        public Enums.Direction Direction { get; set; }

        public ZIndexChangeEventArgs(string controlName, Enums.Direction direction)
        {
            ControlName = controlName;
            Direction = direction;
        }
    }
}
