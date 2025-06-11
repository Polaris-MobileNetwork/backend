namespace Application.Common.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }   = false;
        public int Code { get; set; }
        public string? Message { get; set; } 

    }
}
