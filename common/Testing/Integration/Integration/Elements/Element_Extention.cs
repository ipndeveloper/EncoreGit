using System;
using WatiN.Core;
using WatiN.Core.Constraints;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public static class Element_Extentions
    {

        public static string CustomGetText(this Element element, int? timeout = null, bool visible = true)
        {
            timeout = element.CustomWaitForVisibility(visible, timeout);
            string str = element.Text;
            if (str != null)
                str = str.Trim();
            return str;
        }

        public static bool CustomCompare(this Element element, CompareID method, Element compareElement, string msg)
        {
            bool result = false;

            switch (method)
            {
                case CompareID.Equal:
                    msg = string.Format("{0}: '{1}' = '{2}'", msg, element.GetKey(), compareElement.GetKey());
                    result = element.Equals(compareElement);
                    break;
                case CompareID.NotEqual:
                    msg = string.Format("{0}: '{1}' != '{2}'", msg, element.GetKey(), compareElement.GetKey());
                    result = !element.Equals(compareElement);
                    break;
                case CompareID.Contains:
                    msg = string.Format("{0}: '{1}' contains '{2}'", msg, element.GetKey(), compareElement.GetKey());
                    result = element.ToString().Contains(compareElement.ToString());
                    break;
            }
            if (result)
                Util.LogPass(msg);
            else
                Util.LogFail(msg);
            return result;
        }

        public static ElementCollection<TElement> GetElements<TElement>(this IElementContainer container) where TElement : Element
        {
            ElementCollection<TElement> elements;
            elements = container.ElementsOfType<TElement>();
            return elements;
        }

        public static ElementCollection<TElement> GetElements<TElement>(this IElementContainer container, Param param) where TElement : Element
        {
            ElementCollection<TElement> elements;
            elements = container.ElementsOfType<TElement>().Filter(param.Constraint);
            return elements;
        }

        [Obsolete("Use '(Param)'", true)]
        public static ElementCollection<TElement> GetElements<TElement>(this IElementContainer container, string attributeValue, AttributeName.ID attributeName = AttributeName.ID.Id) where TElement : Element
        {            
            ElementCollection<TElement> elements;
            elements = container.ElementsOfType<TElement>().Filter(new AttributeConstraint(attributeName.ToString(), attributeValue));
            return elements;
        }

        [Obsolete("Use '(Param)'", true)]
        public static ElementCollection<TElement> GetElements<TElement>(this IElementContainer container, string attributeValue, AttributeName.ID attributeName, RegexOptions options) where TElement : Element
        {
            ElementCollection<TElement> elements;
            elements = container.ElementsOfType<TElement>().Filter(new AttributeConstraint(attributeName.ToString(), new Regex(attributeValue, options)));
            return elements;
        }

        public static TElement GetElement<TElement>(this IElementContainer container) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(AnyConstraint.Instance);
            return element;
        }

        public static TElement GetElement<TElement>(this IElementContainer container, Param param) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(param.Constraint);
            return element;
        }

        [Obsolete("Use '(Param)'", true)]
        public static TElement GetElement<TElement>(this IElementContainer container, int index) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(new IndexConstraint(index));
            return element;
        }

        [Obsolete("Use '(Param)'", true)]
        public static TElement GetElement<TElement>(this IElementContainer container, string attributeValue, AttributeName.ID attributeName = AttributeName.ID.Id) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(new AttributeConstraint(attributeName.ToString(), attributeValue));
            return element;
        }

        [Obsolete("Use '(Param)'", true)]
        public static TElement GetElement<TElement>(this IElementContainer container, string attributeValue, AttributeName.ID attributeName, RegexOptions options) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(new AttributeConstraint(attributeName.ToString(), new Regex(attributeValue, options)));
            return element;
        }

        [Obsolete("Use '(Param)'", true)]
        public static TElement GetElement<TElement>(this IElementContainer container, string attributeValue, string attributeName) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(new AttributeConstraint(attributeName, attributeValue));
            return element;
        }

        [Obsolete("Use '(Param)'", true)]
        public static TElement GetElement<TElement>(this IElementContainer container, string attributeValue, string attributeName, RegexOptions options) where TElement : Element
        {
            TElement element;
            element = container.ElementOfType<TElement>(new AttributeConstraint(attributeName, new Regex(attributeValue, options)));
            return element;
        }

        public static string GetKey(this Element element)
        {
            string key = string.Empty;

            if (!string.IsNullOrEmpty(element.Text))
            {
                key = element.Text.Trim();
            }
            else if (!string.IsNullOrEmpty(element.Name))
            {
                key = element.Name;
            }
            else if (!string.IsNullOrEmpty(element.Id))
            {
                key = element.Id;
            }
            else if (!string.IsNullOrEmpty(element.ClassName))
            {
                key = element.ClassName;
            }

            return key;
        }

        private static ElementCollection<Image> GetSpinners(this IElementContainer container)
        {
            return container.GetElements<Image>(
                new Param("loading", AttributeName.ID.Alt, RegexOptions.IgnoreCase)
                .Or(new Param("Resource/Content/Images/.*load", AttributeName.ID.Src, RegexOptions.None))
                .Or(new Param("processing", AttributeName.ID.Alt, RegexOptions.IgnoreCase))
                );
        }

        public static int? CustomWaitForSpinners(this IElementContainer container, int? timeout = null, int? delay = 2)
        {
            ElementCollection<Image> spinners;
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            try
            {
                bool visible = false;
                if (delay.HasValue)
                {
                    while (delay > 0 && !visible)
                    {
                        spinners = container.GetSpinners();
                        foreach (Image spinner in spinners)
                        {
                            visible = spinner.IsVisible();
                            if (visible)
                                break;
                        }
                        if (!visible)
                        {
                            Thread.Sleep(1000);
                            delay--;
                        }
                    }
                }
                while (visible)
                {
                    spinners = container.GetSpinners();
                    if (spinners.Count > 0)
                    {
                        foreach (Image spinner in spinners)
                        {
                            visible = spinner.IsVisible();
                            if (visible)
                                break;
                        }
                        if (visible)
                        {
                            if (--timeout < 0)
                                throw new TimeoutException();
                            Thread.Sleep(1000);
                        }
                    }
                    else
                        visible = false;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Util.LogDoneMessage(string.Format("CustomWaitForSpinners: System.UnauthorizedAccessException {0}", ex.Message));
            }
            return timeout;
        }

        public static int? CustomWaitForSpinner(this IElementContainer container, int? timeout = null, int? delay = 2)
        {
            Image spinner;
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            spinner = container.GetElement<Image>(new Param("loading", "alt", RegexOptions.IgnoreCase).Or(new Param("Resource/Content/Images/.*load", AttributeName.ID.Src, RegexOptions.None)));
            //if (delay.HasValue)
            spinner.CustomWaitForVisibility(true, delay, false);
            return spinner.CustomWaitForVisibility(false, timeout);
        }

        public static int? CustomWaitForVisibility(this Element element, bool visible = true, int? timeout = null, bool exception = true)
        {
            int wait = 0;
            if (!timeout.HasValue)
                timeout = Settings.WaitUntilExistsTimeOut;
            while (element.IsVisible() != visible)
            {
                if (--timeout < 0)
                {
                    if (exception)
                        throw new TimeoutException(string.Format("Element {0} visible", visible == true ? "is not" : "is"));
                    break;
                }
                Thread.Sleep(1000);
                wait++;
            }
            if (Util.LogLevel() > 0)
            {
                Util.LogDoneMessage(string.Format("CustomWaitForVisibility {0} seconds", wait));
            }
            return timeout;
        }

        public static bool IsVisible(this Element element)
        {
            /*
             * This method is surrounded by try/catch blocks so it may be used on elements
             * that may be removed from the page by either a page change or by javascript.
             */
            bool visible;
            try
            {
                if (!element.Exists || element.GetAttributeValue("type") == "hidden" || element.Style.Display == "none" || element.Style.GetAttributeValue("visibility") == "hidden")
                {
                    visible = false;
                }
                else
                {
                    Element parent = element.Exists ? element.Parent : null;
                    if (parent != null && parent.Exists)
                        visible = parent.IsVisible();
                    else
                        visible = true;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                visible = false;
                Util.LogDoneMessage(string.Format("IsVisible: System.UnauthorizedAccessException {0}", ex.Message));
            }
            return visible;
        }

        public static int? CustomRunScript(this Element element, string action, string parameter = null, int index = 0, int? timeout = null)
        {
            string jquery;

            if (element.Id != null)
            {
                jquery = string.Format("$('#{0}:eq({1})').{2}({3})", element.Id, index, action, parameter);
            }
            else
            {
                Element parent = element.Parent;
                while (parent.Id == null)
                {
                    parent = parent.Parent;
                }

                if (!string.IsNullOrEmpty(element.ClassName))
                {
                    jquery = string.Format("$('#{0} .{1}:eq({2})').{3}({4})", parent.Id, element.ClassName, index, action, parameter);
                }
                else
                {
                    jquery = string.Format("$('#{0} {1}:eq({2})').{3}({4})", parent.Id, element.TagName, index, action, parameter);
                }
            }
            return Util.CustomRunScript(jquery, timeout);
        }

        public static int? CustomClick(this Element element, int? timeout = null, bool visible = true)
        {
            timeout = element.CustomClickNoWait(timeout, visible);
            timeout = Util.Browser.CustomWaitForComplete(timeout);
            return timeout;
        }

        public static int? CustomWaitForEnabled(this Element element, bool enabled = true, int? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            try
            {
                while (element.Enabled != enabled)
                {
                    if (--timeout < 0)
                        throw new TimeoutException("CustomWaitForEnabled");
                    Thread.Sleep(1000);
                    bool exist = element.Exists;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                // Catch the excewption of and element once existing but no longer exists
                // This can be normal opereration for wait for an element to not exist.
                Util.LogDoneMessage(string.Format("CustomWaitForEnabled: System.UnauthorizedAccessException {0}", ex.Message));
            }
            return timeout;
        }

        public static int? CustomWaitForExist(this Element element, int? timeout = null, bool exist = true, bool exception = true)
        {
            int wait = 0;
            if (!timeout.HasValue)
                timeout = Settings.WaitUntilExistsTimeOut;
            try
            {
                while (element.Exists != exist)
                {
                    if (--timeout < 0)
                    {
                        if (exception)
                            throw new TimeoutException(String.Format("Element '{0}' does not exist.", element.GetKey()));
                        break;
                    }
                    Thread.Sleep(1000);
                    wait++;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                if (exist)
                {
                    if (timeout > 0)
                    {
                        // Try again as the element may have been associated with a previous page
                        Thread.Sleep(1000);
                        timeout = element.CustomWaitForExist(--timeout, exist, exception);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            if (Util.LogLevel() > 0)
            {
                Util.LogDoneMessage(string.Format("CustomWaitForExist {0} seconds", wait));
            }
            return timeout;
        }

        public static int? CustomClickNoWait(this Element element, int? timeout = null, bool visible = true)
        {
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            if (visible)
                timeout = element.CustomWaitForVisibility(visible, timeout);
            timeout = element.CustomWaitForEnabled(true, timeout);
            string key = element.GetKey();
            element.ClickNoWait();
            Util.LogDoneMessage(String.Format("Click '{0}'", key));
            return timeout;
        }

        [Obsolete("Use CustomWaitForAttributeValue()", true)]
        public static int? CustomWaitAttributeValue(this Element element, AttributeName.ID attributeID, string value, int? timeout = null)
        {
            string id = attributeID.ToString();
            if (!timeout.HasValue)
                timeout = Settings.WaitUntilExistsTimeOut;
            string attributeName = attributeID.ToString();
            if (element.GetAttributeValue(attributeName) != value && --timeout < 0)
            {
                Thread.Sleep(1000);
            }
            if (timeout < 0)
                throw new TimeoutException();
            return timeout;
        }

        public static int? CustomWaitForAttributeValue(this Element element, AttributeName.ID attributeID, string value, RegexOptions options, int? timeout = null)
        {
            string id = attributeID.ToString();
            if (!timeout.HasValue)
                timeout = Settings.WaitUntilExistsTimeOut;
            string attributeName = attributeID.ToString();
            Regex regex = new Regex(value, options);
            if (!regex.IsMatch(element.GetAttributeValue(attributeName)) && timeout-- > 0)
            {
                Thread.Sleep(1000);
            }
            if (timeout < 0)
                throw new TimeoutException();
            return timeout;
        }

        public static string CustomGetAttributeValue(this Element element, AttributeName.ID attributeID)
        {
            return element.GetAttributeValue(attributeID.ToString());
        }

        public static void CustomSetAttributeValue(this Element element, AttributeName.ID attributeID, string setValue)
        {
            string attributeName = attributeID.ToString();
            element.SetAttributeValue(attributeName, setValue);
            Util.LogDoneMessage(String.Format("Set '{0}'  to '{1}' for element '{2}'", attributeName, setValue, element.Name));
        }
    }
}
