using ArchiveProgram;
using Microsoft.Extensions.Configuration;
using SevenZip;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using System.Data;
using System.Text;
using ArchiveProgram.Models.Implementations;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Service collection registers services
// Build the service provider
// Use the service provider to grab the instances

// Doesnt matter when executed, zero time if doing separate check
// Can look into CookieAuthentication in .NET, but Token is more universally used and avoids privacy laws

Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

string targetFolder = settings.Destination;
string sourceCodeFolder = settings.Source;
if (System.IO.Directory.Exists(targetFolder))
{

    string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    // Specify where 7z.dll DLL is located
    var path = Path.Combine(directory, Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
    SevenZip.SevenZipBase.SetLibraryPath(path);
    //SevenZipCompressor.SetLibraryPath(path);
    SevenZipCompressor sevenZipCompressor = new SevenZipCompressor();
    sevenZipCompressor.CompressionLevel = SevenZip.CompressionLevel.Ultra;
    sevenZipCompressor.CompressionMethod = CompressionMethod.Lzma;

    string dateTime = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
    var filePath = Path.Combine(sourceCodeFolder, $"{dateTime}_Logs.txt");
    using (var connection = new SqlConnection(settings.SqlConnectionString))
    {
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                var procedure = "GetLogs";
                var values = new { Timestamp = dateTime };
                var results = connection.Query<Log>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                List<string> stringList = results.Select(i => i.ToString()).ToList();
                foreach (string stringValue in stringList)
                {
                    Console.WriteLine(stringValue);
                }
                //results.ForEach(l => Console.WriteLine($"{l.Timestamp} {l.Level} {l.Username} {l.Category} {l.Description}"));
                File.WriteAllLines(filePath, stringList);
                StringBuilder stringBuilder = new StringBuilder();
                sevenZipCompressor.CompressFiles(Path.Combine(targetFolder, stringBuilder.AppendFormat("{0}{1}", dateTime, "_Archive.7z").ToString()), filePath);
                procedure = "DeleteLogs";
                var rowsAffected = connection.Execute(procedure, values, commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }
    }
    // Compress the directory and save the file in a yyyyMMdd_project-files.7z format (eg. 20141024_project-files.7z

    //sevenZipCompressor.CompressDirectory(sourceCodeFolder, Path.Combine(targetFolder, string.Concat(DateTime.Now.ToString("yyyyMMdd"), "_project-files.7z")));
}