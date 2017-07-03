using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;

namespace NetSteps.Web.Mvc.Helpers
{
    [Serializable]
    public class MessageHandler : CloneableBase<MessageHandler>
    {
        #region Properties
        public List<string> Errors = new List<string>();
        public List<string> SuccessfulResults = new List<string>();
        public List<string> NoticeResults = new List<string>();
        public bool HasAnyMessages
        {
            get { return HasErrors || HasSuccessfulResults || HasNoticeResults; }
        }
        public bool HasErrors
        {
            get { return Errors.Any(); }
        }
        public bool HasSuccessfulResults
        {
            get { return SuccessfulResults.Any(); }
        }
        public bool HasNoticeResults
        {
            get { return NoticeResults.Any(); }
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

        public void AddNoticeResult(string noticeResult)
        {
            if (!string.IsNullOrEmpty(noticeResult) && !NoticeResults.Contains(noticeResult))
                NoticeResults.Add(noticeResult);
        }

        public void ClearErrors()
        {
            Errors = new List<string>();
        }

        public void ClearSuccessfulResult()
        {
            SuccessfulResults = new List<string>();
        }

        public void ClearNoticeResults()
        {
            NoticeResults = new List<string>();
        }

        public void Clear()
        {
            ClearErrors();
            ClearSuccessfulResult();
            ClearNoticeResults();
        }
        #endregion
    }
}
