namespace Application.DTOs.Reports
{
    public class ReservationReceiptDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public ClientDto Client { get; set; } = new();
        public List<ReservationItemDto> Items { get; set; } = new();
        public decimal ReservationDeposit { get; set; }
        public DateTime ReservationUntil { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
    }

    public class ReservationItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
