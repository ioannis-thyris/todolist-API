using AutoMapper;
using Dapper;
using TodoListAPI.AppContext.Sql;
using TodoListAPI.Dtos;
using TodoListAPI.Models;

namespace TodoListAPI.Repositories
{
    public class TodoDapperRepository : ITodoRepository
    {
        public ISqlContext db { get; private set; }


        private readonly IMapper _mapper;

        public TodoDapperRepository(ISqlContext database, IMapper mapper)
        {
            db = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>> GetAll()
        {
            string queryGetAll = "SELECT * FROM Todo";

            using (var connection = db.CreateConnection())
            {
                var todos = await connection.QueryAsync<Todo>(queryGetAll);

                return _mapper.Map<IEnumerable<TodoDto>>(todos);
            }
        }

        public async Task<TodoDto> GetById(string id)
        {
            string queryGetById = "SELECT * FROM Todo WHERE Id = @Id";
            var parameters = new { Id = id };

            using (var connection = db.CreateConnection())
            {
                var todo = await connection.QueryFirstOrDefaultAsync<Todo>(queryGetById, parameters);

                return _mapper.Map<TodoDto>(todo);
            }
        }

        public async Task<TodoDto> Insert(TodoDtoCreate newTodo)
        {
            var todo = _mapper.Map<Todo>(newTodo);

            string queryInsertTodo = @"INSERT INTO Todo(Title, DueTo, Priority)
                                       OUTPUT INSERTED.Id
                                       VALUES (@Title, @DueTo, @Priority)";

            var parameters = new { todo.Title, todo.DueTo, todo.Priority };

            using (var connection = db.CreateConnection())
            {
                string id = await connection.QuerySingleAsync<string>(queryInsertTodo, parameters);

                return await GetById(id);
            }
        }

        public async Task<TodoDto> Update(string id, TodoDtoUpdate updatedTodo)
        {
            var todo = _mapper.Map<Todo>(updatedTodo);

            string queryUpdateTodo = @"UPDATE Todo
                                       SET Title = @Title, DueTo = @DueTo, Priority = @Priority
                                       WHERE Id = @Id";
            var parameters = new { todo.Title, todo.DueTo, todo.Priority, Id = id };

            using (var connection = db.CreateConnection())
            {
                await connection.ExecuteAsync(queryUpdateTodo, parameters);

                return await GetById(id);
            }
        }

        public async Task Delete(string id)
        {
            string queryDeleteTodo = @"DELETE FROM Todo WHERE Id = @Id";
            var parameters = new { Id = id };

            using (var connection = db.CreateConnection())
            {
                await connection.ExecuteAsync(queryDeleteTodo, parameters);
            }
        }
    }
}
