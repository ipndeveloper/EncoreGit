using System;
using System.Collections.Generic;

namespace NetSteps.WebControls
{
    /// <summary>
    /// Same concept has PageError, but for MVC, we're assuming we have an object of this type on the model, and that rendering will be called manually. Also, no ViewState :)
    /// </summary>
    [Serializable]
    public class MVCPageError
    {
        #region Properties
        private List<string> Errors = new List<string>();
        private List<string> SuccessfulResults = new List<string>();
        public bool HasErrors
        {
            get { return (Errors.Count != 0); }
        }
        public bool HasSuccessfulResults
        {
            get { return (SuccessfulResults.Count != 0); }
        }
        #endregion

        #region Methods
        public void AddError(string error)
        {
            if (!string.IsNullOrEmpty(error) && !Errors.Contains(error))
                Errors.Add(error);
        }

        public void AddSuccessfulResult(string successfulResult)
        {
            if (!string.IsNullOrEmpty(successfulResult) && !SuccessfulResults.Contains(successfulResult))
                SuccessfulResults.Add(successfulResult);
        }

        public void ClearErrors()
        {
            Errors = new List<string>();
        }

        public void ClearSuccessfulResult()
        {
            SuccessfulResults = new List<string>();
        }

        public void Clear()
        {
            ClearErrors();
            ClearSuccessfulResult();
        }
        #endregion

        #region Rendering
        public string RenderHTML()
        {
            string output = string.Empty;
            if (HasErrors)
            {
                foreach (string error in Errors)
                    output += string.Format("<div class=\"error_message\">{0}</div>", error);
                output = string.Format("<div class=\"error_message_block\">{0}</div>", output);
            }
            else if (HasSuccessfulResults)
            {
                foreach (string message in SuccessfulResults)
                    output += string.Format("<div class=\"success_message\">{0}</div>", message);
                output = string.Format("<div class=\"success_message_block\">{0}</div>", output);
            }

            //clear everything before we ouput
            this.Clear();
            return output;
        }
        #endregion
    }
}
