namespace TodoListAPI.Dtos
{
    public class TodoDto : ITodoDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DueTo DueTo { get; set; }
        public Priority Priority { get; set; }
    }
}
