using TodoListAPI.AppContext.Sql;
using TodoListAPI.Dtos;
using TodoListAPI.Models;

namespace TodoListAPI.Repositories
{
    public interface ITodoRepository
    {
        public Task<TodoDto> Insert(TodoDtoCreate newTodo);
        public Task<TodoDto> GetById(string id);
        public Task<IEnumerable<TodoDto>> GetAll();
        public Task<TodoDto> Update(string id, TodoDtoUpdate updatedTodo);
        public Task Delete(string id);
    }
}
