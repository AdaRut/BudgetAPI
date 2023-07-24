namespace BudgetAPI.Models
{
    public class GroupItemDto
    {
        public string Name { get; set; }
        public decimal PlannedAmount { get; set; }
        public decimal SpendAmount { get; set; }
        public string Notes { get; set; }
        public bool IsPaid { get; set; }
    }
}
