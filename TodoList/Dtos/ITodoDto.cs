namespace TodoListAPI.Dtos
{
    public interface ITodoDto
    {
        public string Title { get; set; }
        public DueTo DueTo { get; set; }
        public Priority Priority { get; set; }
    }
}
