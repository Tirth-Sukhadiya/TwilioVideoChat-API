namespace TwilioCallApp.Shared.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorDetailViewModel Error { get; set; }
        public ErrorViewModel(int Code, int Status, string Message)
        {
            Error = new ErrorDetailViewModel(Code, Status, Message);
        }
    }
}
