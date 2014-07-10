namespace ProxySearch.Engine.Error
{
    public interface IErrorFeedbackHolder
    {
        IErrorFeedback ErrorFeedback { get; set; }
    }
}
