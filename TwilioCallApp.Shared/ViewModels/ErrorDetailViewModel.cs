namespace TwilioCallApp.Shared.ViewModels
{
    public class ErrorDetailViewModel
    {
        public int Code { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public ErrorDetailViewModel(int Code, int Status, string Message)
        {
            this.Code = Code;
            this.Status = Status;
            this.Message = Message;
        }
    }
}
