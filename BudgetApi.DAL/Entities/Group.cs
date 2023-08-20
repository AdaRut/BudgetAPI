namespace BudgetAPI.DAL.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual IEnumerable<GroupItem> GroupItems { get; set; } = new List<GroupItem>();
    }
}
