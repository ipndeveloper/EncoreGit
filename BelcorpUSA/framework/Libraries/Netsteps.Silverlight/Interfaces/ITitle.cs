
namespace NetSteps.Silverlight
{
    public interface ITitle
    {
        event TitleChangeEventHandler TitleChanged;
        string Title { get; set; }
        string Tag { get; set; }
    }
}
