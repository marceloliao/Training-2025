using System.Data.SQLite;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntelContentImportScript
{
    public class Util
    {
        public static string GetRelativePath(string path, string to)
        {
            var rgFrom = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            var rgTo = to.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            var cSame = rgFrom.TakeWhile((p, i) => i < rgTo.Length && string.Equals(p, rgTo[i])).Count();

            return Path.Combine(
                Enumerable.Range(0, rgFrom.Length - cSame)
                .Select(_ => "..")
                .Concat(rgTo.Skip(cSame))
                .ToArray()
            ).Substring(1);
        }

        public static string CreateDbConnectionString(string dbFilename)
        {
            return $"Uri=File:{Path.GetFullPath(dbFilename)}";
        }

        public static void InitializeDatabase(string dbFilename)
        {
            string connectionString = CreateDbConnectionString(dbFilename);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Import_Data 
                        (TridionServer TEXT NOT NULL,
                         ObjectGuid TEXT NOT NULL,
                         ObjectFilename TEXT NOT NULL,
                         UNIQUE(TridionServer, ObjectFilename))";

                command.ExecuteNonQuery();
            }
        }

        public static string RetrieveOldGuid(string dbFilename, string tridionServer, string filename)
        {
            string connectionString = CreateDbConnectionString(dbFilename);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"SELECT ObjectGuid FROM Import_Data 
                      WHERE TridionServer = '$TridionServer' AND ObjectFilename = '$ObjectFilename'";

                command.Parameters.AddWithValue("$TridionServer", tridionServer);
                command.Parameters.AddWithValue("$ObjectFilename", filename);
                
                using(var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetString(0);
                    }
                }
            }

            return null;
        }

        public static void AddNewGuid(string dbFilename, string tridionServer, string guid, string filename)
        {
            string connectionString = CreateDbConnectionString(dbFilename);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"INSERT OR IGNORE INTO Import_Data VALUES ($TridionServer, $Guid, $ObjectFilename)";

                command.Parameters.AddWithValue("$TridionServer", tridionServer);
                command.Parameters.AddWithValue("$Guid", guid);
                command.Parameters.AddWithValue("$ObjectFilename", filename);

                command.ExecuteNonQuery();
            }
        }

        public static int UpdateFileContent(string filePath, string searchPattern, string replacePattern)
        {
            string content = string.Empty;
            int count = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                content = reader.ReadToEnd();
                reader.Close();
            }

            content = Regex.Replace(content, searchPattern,
                m =>
                {
                    count++;
                    return replacePattern;
                });

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(content);
                writer.Close();
            }

            return count;
        }

        /// <summary>
        /// Return a list of files whose filenames match the regex search pattern
        /// </summary>
        /// <param name="folder">source folder</param>
        /// <param name="searchPattern">regex search pattern</param>
        /// <param name="searchOption">SearchOption to indicate to search all sub directories or top level-directory only</param>
        /// <returns></returns>
        public static string[] GetFiles(string folder, string searchPattern, SearchOption searchOption)
        {
            Regex regex = new Regex(searchPattern);
            var result = Directory.GetFiles(folder, "*.*", searchOption).Where(file => regex.IsMatch(file)).ToArray();
            return result;
        }
    }
}
