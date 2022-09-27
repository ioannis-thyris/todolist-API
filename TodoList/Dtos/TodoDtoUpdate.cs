namespace TodoListAPI.Dtos
{
    public class TodoDtoUpdate : ITodoDto
    {
        public string Title { get; set; }
        public DueTo DueTo { get; set; }
        public Priority Priority { get; set; }
    }
}
