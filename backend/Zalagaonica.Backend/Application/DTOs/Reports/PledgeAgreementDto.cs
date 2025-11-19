namespace Application.DTOs.Reports
{
    public class PledgeAgreementDto
    {
        public string PledgeNumber { get; set; } = string.Empty;
        public DateTime PledgeDate { get; set; }
        public ClientDto Client { get; set; } = new();
        public ItemDto Item { get; set; } = new();
        public decimal LoanAmount { get; set; }
        public decimal ReturnAmount { get; set; }
        public int Period { get; set; }
        public DateTime RedeemDeadline { get; set; }
    }

    public class ClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Oib { get; set; }
    }

    public class ItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedValue { get; set; }
    }
}
