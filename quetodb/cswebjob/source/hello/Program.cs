// See https://aka.ms/new-console-template for more information


using MySql.Data.MySqlClient;

var connectionString = "server=mysql80;database=mydb;user id=user;password=user;";

using (var connection = new MySqlConnection(connectionString))
{
    connection.Open();
    
    using var command = new MySqlCommand("SELECT COUNT(*) FROM student", connection);
    var movieCount = command.ExecuteScalar();

    Console.WriteLine($"There are {movieCount} movies");
}


Console.WriteLine("Hello, World..!");
