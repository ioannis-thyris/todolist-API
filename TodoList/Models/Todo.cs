namespace TodoListAPI.Models
{
    public class Todo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DueTo { get; set; }
        public Priority Priority { get; set; }
    }
}
