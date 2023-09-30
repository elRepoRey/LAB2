using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Lab2.Services
{
    internal static class DatabaseService<T>
    {
       
        public static void SaveDataToFiles(List<T> data, string FilePath)
        {
            try
            {
                lock (FilePath)
                {
                    File.WriteAllText(FilePath, JsonSerializer.Serialize(data));
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"An error occurred while saving data to {FilePath}.", ex);
            }
        }

        public static List<T> LoadDataFromFiles(string FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string fileContent;
                    lock (FilePath)
                    {
                        fileContent = File.ReadAllText(FilePath);
                    }
                    if (!string.IsNullOrWhiteSpace(fileContent))
                    {
                        return JsonSerializer.Deserialize<List<T>>(fileContent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"An error occurred while loading data from {FilePath}.", ex);
            }
            return new List<T>();
        }

        public class DataAccessException : Exception
        {
            public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
