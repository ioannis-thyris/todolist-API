using System;
using System.Text.Json;
using TodoListAPI.Dtos;

namespace TodoListAPI.Repositories
{
    public class TodoFirebaseRepository : ITodoRepository
    {
        private readonly HttpClient firebaseClient;
        private readonly IConfiguration _configuration;
        private readonly string firebaseDb;

        public TodoFirebaseRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;

            firebaseDb = _configuration.GetSection("Firebase").GetValue<string>("Realtime Database");

            firebaseClient = httpClient;
            firebaseClient.BaseAddress = new Uri(firebaseDb);
        }

        public async Task<IEnumerable<TodoDto>> GetAll()
        {
            var response = await firebaseClient.GetFromJsonAsync<JsonDocument>("todo.json");

            if (response == null)
                return Enumerable.Empty<TodoDto>();

            var todoDtos = new List<TodoDto>();

            try
            {
                foreach (var property in response.RootElement.EnumerateObject())
                {
                    todoDtos.Add(ParseEntry(property));
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not parse todos. Please check Database for illegal entries.");
            }

            return todoDtos;
        }

        public async Task<TodoDto> GetById(string id)
        {
            var dto = await firebaseClient.GetFromJsonAsync<TodoDto>($"todo/{id}.json");

            if (dto != null)
                dto.Id = id;

            return dto;
        }

        public async Task<TodoDto> Insert(TodoDtoCreate newTodo)
        {
            var response = await firebaseClient.PostAsJsonAsync("todo.json", newTodo);

            var content = await response.Content.ReadFromJsonAsync<JsonElement>();

            string id = content.GetProperty("name")
                               .GetString();

            return await GetById(id);
        }

        public async Task<TodoDto> Update(string id, TodoDtoUpdate updatedTodo)
        {
            var dtoJson = JsonSerializer.Serialize(updatedTodo);

            HttpContent request = JsonContent.Create(updatedTodo);

            var response = await firebaseClient.PatchAsync($"todo/{id}.json", request);


            return await GetById(id);
        }

        public async Task Delete(string id)
        {
            await firebaseClient.DeleteAsync($"todo/{id}.json");
        }


        public TodoDto ParseEntry(JsonProperty entry)
        {

            var title = entry.Value.GetProperty("title")
                   .GetString();

            var priority = entry.Value.GetProperty("priority")
                                      .GetInt32();

            var date = entry.Value.GetProperty("dueTo")
                                  .GetProperty("date")
                                  .GetString();

            var time = entry.Value.GetProperty("dueTo")
                                  .GetProperty("time")
                                  .GetString();

            var dto = new TodoDto
            {
                Id = entry.Name,
                Title = title,
                Priority = (Priority)priority,
                DueTo = new DueTo { Date = date, Time = time }
            };

            return dto;


        }
    }
}
