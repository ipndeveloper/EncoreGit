
namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public abstract class SectionModel
    {
        #region Resources
        public virtual bool Active { get; set; }
        public virtual string Action { get; set; }
        public virtual string Title { get; set; }
        public virtual string PartialViewName { get; set; }
        public virtual bool Completed { get; set; }
        #endregion

        #region Infrastructure
        public SectionModel LoadBaseResources(
            bool active,
            string action,
            string title,
            string partialViewName,
            bool completed)
        {
            this.Active = active;
            this.Action = action;
            this.Title = title;
            this.PartialViewName = partialViewName;
            this.Completed = completed;

            return this;
        }
        #endregion
    }
}