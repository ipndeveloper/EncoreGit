using WatiN.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_Downline_Control : Control<TableRow>
    {
        protected List<string> _data = new List<string>();

        protected override void InitializeContents()
        {
            base.InitializeContents();

            for (int index = 2; index < Element.TableCells.Count; index++)
            {
                _data.Add(Element.TableCells[index].CustomGetText());
            }
        }

        public bool IsValid()
        {
            bool valid = true;
            foreach (string value in _data)
            {
                if (string.IsNullOrEmpty(value))
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }
    }
}
