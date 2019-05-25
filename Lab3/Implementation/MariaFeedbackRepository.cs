using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Lab3.Implementation
{
    class MariaFeedbackRepository : IMariaFeedbackRepository
    {
        private MySqlConnection connection;

        public MariaFeedbackRepository(IOptions<MariaSetting> settings)
        {
            connection = new MySqlConnection(settings.Value.ConnectionString);
        }

        public Task<IEnumerable<Feedback>> ReadAll()
        {
            var sql = "SELECT * FROM Feedback"; // Строка запроса

            var dataRows = ExecuteQuery(sql);

            return Task.FromResult(dataRows.Select(Convert));
        }

        public Task Create(Feedback item)
        {
            
            var sql =
                $"insert INTO Feedback (id, Name, Text, CreatedAt, UpdatedAt) VALUES ('{item.Id.ToString()}','{item.Name}', '{item.Text}', '{item.CreatedAt:yyyy-MM-ddТhh:mm:ss}', '{item.UpdatedAt:yyyy-MM-ddТhh:mm:ss}') ON DUPLICATE KEY UPDATE Name = '{item.Name}', Text = '{item.Text}';";

            ExecuteQuery(sql);
            
            return Task.FromResult(true);
        }

        public Task<Feedback> Read(Guid id)
        {
            var sql = $"select * from Feedback where Id = '{id.ToString()}'";
                
            var dataRows = ExecuteQuery(sql);

            return Task.FromResult(dataRows.Select(Convert).SingleOrDefault());        
        }

        public Task<bool> Update(Feedback item)
        {
            var sql = $"update Feedback set Name = '{item.Name}', Text = '{item.Text}', UpdatedAt = '{item.UpdatedAt:yyyy-MM-ddТhh:mm:ss}'  where Id = '{item.Id.ToString()}'";
                
            var dataRows = ExecuteQuery(sql);

            return Task.FromResult(true);
        }

        public Task<bool> Delete(Guid id)
        {
            var sql = $"delete from Feedback where Id = '{id.ToString()}'";
                
            var dataRows = ExecuteQuery(sql);

            return Task.FromResult(true);  
        }

        public Task<bool> DeleteAll()
        {
            var sql = $"delete from Feedback";
                
            var dataRows = ExecuteQuery(sql);

            return Task.FromResult(true);
        }

        private Feedback Convert(DataRow dataRow)
        {
            return new Feedback
            {
                Id = Guid.Parse(dataRow[0].ToString()),
                Name = dataRow[1].ToString(),
                Text = dataRow[2].ToString(),
                CreatedAt = DateTime.Parse(dataRow[3].ToString()),
                UpdatedAt = DateTime.Parse(dataRow[4].ToString())
            };
        }

        private DataRow[] ExecuteQuery(string sql)
        {
            var sqlCom = new MySqlCommand(sql, connection);
            connection.Open();
            sqlCom.ExecuteNonQuery();
            var dataAdapter = new MySqlDataAdapter(sqlCom);
            var dt = new DataTable();
            dataAdapter.Fill(dt);
            connection.Close();

            return dt.Select();
        }
    }
}