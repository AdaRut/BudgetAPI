namespace BudgetAPI.DAL.Entities
{
    public class GroupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PlannedAmount { get; set; }
        public decimal SpendAmount { get; set; }
        public string Notes { get; set; }
        public bool IsPaid { get; set; }

        public int GroupId { get; set; }
        public virtual Group group { get; set; }
    }
}
