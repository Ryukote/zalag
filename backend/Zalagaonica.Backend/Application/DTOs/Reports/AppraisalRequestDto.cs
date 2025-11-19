namespace Application.DTOs.Reports
{
    public class AppraisalRequestDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public ClientDto Client { get; set; } = new();
        public ItemDto Item { get; set; } = new();
        public string Purpose { get; set; } = string.Empty;
    }
}
