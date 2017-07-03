using System;
using System.Collections.Generic;
using System.Windows;

namespace NetSteps.Silverlight
{
    public delegate void StateEventHandler(object sender, StateChangedEventArgs e);

    public class StateChangedEventArgs : EventArgs
    {
        public string StateName { get; protected set; }
        public string SubStateName { get; protected set; }
        public object Obj { get; set; }
        public bool HasBeenHandled { get; set; }

        public StateChangedEventArgs(string stateName)
        {
            HasBeenHandled = false;
            StateName = stateName;
        }
        public StateChangedEventArgs(string stateName, object obj)
            : this(stateName)
        {
            HasBeenHandled = false;
            Obj = obj;
        }
        public StateChangedEventArgs(string stateName, string subStateName)
            : this(stateName)
        {
            HasBeenHandled = false;
            SubStateName = subStateName;
        }
        public StateChangedEventArgs(string stateName, object obj, string subStateName)
            : this(stateName, obj)
        {
            HasBeenHandled = false;
            SubStateName = subStateName;
        }

        public override bool Equals(object obj)
        {
            if (obj is StateChangedEventArgs)
            {
                StateChangedEventArgs temp = obj as StateChangedEventArgs;
                if (temp.StateName == StateName && temp.SubStateName == SubStateName)
                    return true;
                else
                    return false;
            }
            return base.Equals(obj);
        }
    }

    public class StateChangedPopupEventArgs : StateChangedEventArgs
    {
        public StateChangedPopupEventArgs(IPopupChild obj)
            : base("PopupDialog", obj)
        {
        }
        public StateChangedPopupEventArgs(IPopupChild obj, string subStateName)
            : base("PopupDialog", obj, subStateName)
        {
        }
    }

    public class StateChangedEmailAddEventArgs : StateChangedEventArgs
    {
        public string FollowupState { get; set; }
        public string FollowupSubState { get; set; }

        public string BodyText { get; set; }
        public string TitleText { get; set; }

        private string _ToFieldText = string.Empty;
        public string ToFieldText { get { return _ToFieldText; } set { _ToFieldText = value; } }
        private Visibility _ToFieldVisibility = Visibility.Visible;
        public Visibility ToFieldVisibility { get { return _ToFieldVisibility; } set { _ToFieldVisibility = value; } }
        private bool _ToFieldEnabled = true;
        public bool ToFieldEnabled { get { return _ToFieldEnabled; } set { _ToFieldEnabled = value; } }

        private string _CcFieldText = string.Empty;
        public string CcFieldText { get { return _CcFieldText; } set { _CcFieldText = value; } }
        private Visibility _CcFieldVisibility = Visibility.Visible;
        public Visibility CcFieldVisibility { get { return _CcFieldVisibility; } set { _CcFieldVisibility = value; } }
        private bool _CcFieldEnabled = true;
        public bool CcFieldEnabled { get { return _CcFieldEnabled; } set { _CcFieldEnabled = value; } }

        private string _BccFieldText = string.Empty;
        public string BccFieldText { get { return _BccFieldText; } set { _BccFieldText = value; } }
        private Visibility _BccFieldVisibility = Visibility.Visible;
        public Visibility BccFieldVisibility { get { return _BccFieldVisibility; } set { _BccFieldVisibility = value; } }
        private bool _BccFieldEnabled = true;
        public bool BccFieldEnabled { get { return _BccFieldEnabled; } set { _BccFieldEnabled = value; } }

        public string HiddenEmailAddress { get; set; }
        private List<string> _FilterResults;
        public List<string> FilterResults
        {
            get
            {
                if (_FilterResults == null)
                    _FilterResults = new List<string>();

                return _FilterResults;
            }
            set
            {
                _FilterResults = value;
            }
        }

        public string Subject { get; set; }

        public StateChangedEmailAddEventArgs()
            : base("Dashboard", "EmailAdd") { }

        public StateChangedEmailAddEventArgs(string titleText, string to, string subject)
            : base("Dashboard", "EmailAdd")
        {
            TitleText = titleText;
            ToFieldText = to;
            Subject = subject;
        }

    }
}
