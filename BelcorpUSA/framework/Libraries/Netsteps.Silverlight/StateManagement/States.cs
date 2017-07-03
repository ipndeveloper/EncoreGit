using System;
using System.Collections.Generic;

namespace NetSteps.Silverlight
{
    public class States
    {
        public class MainWindow
        {
            public enum MainWindowStates
            {
                LeadsMain,
                PopupDialog
            }

            public static StateChangedEventArgs CurrentState
            {
                get
                {
                    if (_pastStates.Count == 0)
                        return null;

                    return _pastStates.Peek();
                }
                set
                {
                    _pastStates.Push(value);
                }
            }

            public static event EventHandler PopupOpened;
            public static event EventHandler PopupClosed;

            private static bool _IsPopupOpen = false;
            public static bool IsPopupOpen
            {
                get
                {
                    return _IsPopupOpen;
                }
                set
                {
                    if (_IsPopupOpen != value)
                    {
                        if (value && PopupOpened != null)
                            PopupOpened(null, new EventArgs());
                        else if (!value && PopupClosed != null)
                            PopupClosed(null, new EventArgs());
                    }
                    _IsPopupOpen = value;
                }
            }

            private static Stack<StateChangedEventArgs> _pastStates = new Stack<StateChangedEventArgs>();
            public static StateChangedEventArgs Back()
            {
                return Back(1);
            }

            public static StateChangedEventArgs Back(int count)
            {
                StateChangedEventArgs stateArgs = null;
                while (_pastStates.Count > 0 && count > 0)
                {
                    stateArgs = _pastStates.Pop();
                    count--;
                }

                stateArgs.HasBeenHandled = false; // Reset has been handled - JHE

                return stateArgs;
            }

            public static StateChangedEventArgs Last(string stateName)
            {
                StateChangedEventArgs result = null;
                Stack<StateChangedEventArgs> temp = new Stack<StateChangedEventArgs>();
                while (_pastStates.Count > 0)
                {
                    StateChangedEventArgs stateArgs = _pastStates.Pop();
                    temp.Push(stateArgs);
                    if (stateArgs.StateName == stateName)
                    {
                        result = stateArgs;
                        break;
                    }
                }

                // Put them back
                //while (temp.Count > 0)
                //{
                //    _pastStates.Push(temp.Pop());
                //}

                result.HasBeenHandled = false; // Reset has been handled - JHE

                return result;
            }

            public static StateChangedEventArgs Last(string stateName, string substateName)
            {
                StateChangedEventArgs stateArgs = null;
                Stack<StateChangedEventArgs> temp = new Stack<StateChangedEventArgs>();
                while (_pastStates.Count > 0)
                {
                    stateArgs = _pastStates.Pop();
                    temp.Push(stateArgs);
                    if (stateArgs.StateName == stateName && stateArgs.SubStateName == substateName)
                        break;
                }

                // Put them back
                //while (temp.Count > 0)
                //{
                //    _pastStates.Push(temp.Pop());
                //}

                stateArgs.HasBeenHandled = false; // Reset has been handled - JHE

                return stateArgs;
            }

            public static StateChangedEventArgs LastDifferent()
            {
                if (CurrentState == null)
                    return null;

                string stateName = CurrentState.StateName;
                string substateName = CurrentState.SubStateName;
                StateChangedEventArgs stateArgs = null;
                while (_pastStates.Count > 0)
                {
                    stateArgs = _pastStates.Pop();
                    if (stateArgs.StateName != stateName || stateArgs.SubStateName != substateName)
                        break;
                }

                stateArgs.HasBeenHandled = false; // Reset has been handled - JHE

                return stateArgs;
            }
        }

    }
}
