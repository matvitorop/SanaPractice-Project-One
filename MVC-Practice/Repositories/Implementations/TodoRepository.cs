using Microsoft.Data.SqlClient;
using MVC_Practice.Models;
using MVC_Practice.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MVC_Practice.Repositories.Implementations
{
    public class TodoRepository : ITodoRepository
    {
        private readonly string _connectionString;
        public TodoRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public async Task AddCategoryAsync(Categories category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("INSERT INTO Categories (Name) VALUES (@Name)", connection);
                command.Parameters.AddWithValue("@Name", category.Name);

                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<Categories>> GetCategoriesAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                SqlCommand command = new SqlCommand("SELECT * FROM Categories", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                List<Categories> categories = new List<Categories>();
                
                while (reader.Read())
                {
                    Categories category = new Categories
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    categories.Add(category);
                }
                
                return await Task.FromResult(categories);
            }
        }
        public async Task AddTaskAsync(Tasks task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"INSERT INTO Tasks (Title, DueDate, IsCompleted, CompletedDate, CategoryId)
                      VALUES (@Title, @DueDate, @IsCompleted, @CompletedDate, @CategoryId)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

                    if (task.DueDate.HasValue)
                        command.Parameters.AddWithValue("@DueDate", task.DueDate.Value);
                    else
                        command.Parameters.AddWithValue("@DueDate", DBNull.Value);

                    if (task.CompletedDate.HasValue)
                        command.Parameters.AddWithValue("@CompletedDate", task.CompletedDate.Value);
                    else
                        command.Parameters.AddWithValue("@CompletedDate", DBNull.Value);

                    if (task.CategoryId.HasValue)
                        command.Parameters.AddWithValue("@CategoryId", task.CategoryId.Value);
                    else
                        command.Parameters.AddWithValue("@CategoryId", DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<List<Tasks>> GetActiveTasksAsync()
        {
            var list = new List<Tasks>();
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                SqlCommand command = new SqlCommand(@"SELECT t.*, c.Name AS CategoryName"
                                                    + " FROM Tasks t"
                                                    + " LEFT JOIN Categories c ON t.CategoryId = c.Id"
                                                    + " WHERE t.IsCompleted = 0" 
                                                    + " ORDER BY t.Id", connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync()) 
                {
                    list.Add(new Tasks
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        
                        DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate"))
                        ? (DateTime?)null
                        : (DateTime?)reader["DueDate"],
                        
                        IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                        
                        CompletedDate = reader.IsDBNull(reader.GetOrdinal("CompletedDate"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("CompletedDate")),
                        
                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId"))
                        ? (int?)null
                        : reader.GetInt32(reader.GetOrdinal("CategoryId"))
                    });
                }
            }
            return list;
        }
        public async Task<List<Tasks>> GetCompletedTasksAsync()
        {
            var list = new List<Tasks>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(@"SELECT t.*, c.Name AS CategoryName"
                                                    + " FROM Tasks t"
                                                    + " LEFT JOIN Categories c ON t.CategoryId = c.Id"
                                                    + " WHERE t.IsCompleted = 1"
                                                    + " ORDER BY t.CompletedDate DESC", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new Tasks
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        
                        DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                        
                        IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                        
                        CompletedDate = reader.IsDBNull(reader.GetOrdinal("CompletedDate"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("CompletedDate")),
                        
                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId"))
                        ? (int?)null
                        : reader.GetInt32(reader.GetOrdinal("CategoryId"))
                    });
                }
            }
            return list;
        }
        public async Task CompleteTask(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(@"UPDATE Tasks SET IsCompleted = 1, CompletedDate = GETDATE() WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", taskId);
                
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
