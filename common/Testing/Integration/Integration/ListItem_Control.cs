using WatiN.Core;
using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class ListItem_Control: Control<Div>
    {
        List<string> _list;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _list =  new List<string>();
            int count = Element.List(Find.Any).ListItems.Count;
            for (int index = 0; index < count; index++)
            {
                _list.Add(Element.GetElement<ListItem>(new Param(index)).Span(Find.ByClass("FR")).CustomGetText());
            }
        }

        public string GetStringValue(int index)
        {
            return _list[index];
        }

        public float GetFloatValue(int index)
        {
            return float.Parse(_list[index], System.Globalization.NumberStyles.Currency);
        }

        public int GetIntergerValue(int index)
        {
            return (int)float.Parse(_list[index], System.Globalization.NumberStyles.Currency);
        }

        /// <summary>
        /// Compares values based on string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool IsValid(ListItem_Control list)
        {
            bool valid = true;
            for (int index = 0; index < _list.Count; index++)
            {
                if (_list[index] != list.GetStringValue(index))
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }
    }
}
