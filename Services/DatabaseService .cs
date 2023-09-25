using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Lab2.Services
{
    internal class DatabaseService<T>
    {
        private readonly string _filePath;

        public DatabaseService(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void SaveDataToFiles(List<T> data)
        {
            try
            {
                lock (_filePath)
                {
                    File.WriteAllText(_filePath, JsonSerializer.Serialize(data));
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"An error occurred while saving data to {_filePath}.", ex);
            }
        }

        public List<T> LoadDataFromFiles()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string fileContent;
                    lock (_filePath)
                    {
                        fileContent = File.ReadAllText(_filePath);
                    }
                    if (!string.IsNullOrWhiteSpace(fileContent))
                    {
                        return JsonSerializer.Deserialize<List<T>>(fileContent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"An error occurred while loading data from {_filePath}.", ex);
            }
            return new List<T>();
        }

        public class DataAccessException : Exception
        {
            public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
