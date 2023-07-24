namespace BudgetAPI.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IEnumerable<Group> Groupes { get; set; } = new List<Group>();


    }
}
