using HW08.Library.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW08.Library.DataAccess
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // 创建表的 SQL 语句
                var createTableCommand = connection.CreateCommand();
                createTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Words (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        English TEXT NOT NULL,
                        Chinese TEXT
                    );
                ";
                createTableCommand.ExecuteNonQuery();
            }
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public void AddWord(string english, string chinese)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = @"
        INSERT INTO Words (English, Chinese)
        VALUES ($English, $Chinese);
    ";
            insertCommand.Parameters.AddWithValue("$English", english); // 参数占位符需要加 $
            insertCommand.Parameters.AddWithValue("$Chinese", chinese);

            insertCommand.ExecuteNonQuery();
        }

        public List<Word> GetAllWords()
        {
            var words = new List<Word>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT ID, English, Chinese FROM Words";

                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        words.Add(new Word
                        {
                            ID = reader.GetInt32(0),
                            English = reader.GetString(1),
                            Chinese = reader.GetString(2)
                        });
                    }
                }
            }

            return words;
        }
    }
}
