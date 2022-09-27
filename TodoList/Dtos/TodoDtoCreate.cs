namespace TodoListAPI.Dtos
{
    public class TodoDtoCreate : ITodoDto
    {
        public string Title { get; set; }
        public DueTo DueTo { get; set; }
        public Priority Priority { get; set; }
    }
}
