
namespace NetSteps.Silverlight
{
    public interface ISubState
    {
        string CurrentSubState { get; set; }
        string LastSubState { get; set; }

        void SetSubState(object sender, StateChangedEventArgs e);
    }
}
