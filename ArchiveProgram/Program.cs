using ArchiveProgram;
using Microsoft.Extensions.Configuration;
using SevenZip;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using System.Data;
using System.Text;
using ArchiveProgram.Models.Implementations;
using ArchiveProgram.Models.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
/// <summary>
///     Program:
///         Archive program for grabbing logs from a source, writing them to a file, compressing the file with 7zip,
///         offloading the compressed file to a destination, and then deleting the logs from the source
/// </summary>

// Configuration Builder to utilize appsettings/configuration for the application
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Grab target and destinations as well as connection string
Settings settings = config.GetRequiredSection("Settings").Get<Settings>();
CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));
string targetFolder = settings.Destination;
string sourceCodeFolder = settings.Source;
if (System.IO.Directory.Exists(targetFolder))
{
    try
    {
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // Setting up 7zip, specify where 7z.dll DLL is located
        var path = Path.Combine(directory, Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
        SevenZip.SevenZipBase.SetLibraryPath(path);
        SevenZipCompressor sevenZipCompressor = new SevenZipCompressor();
        sevenZipCompressor.CompressionLevel = SevenZip.CompressionLevel.Ultra;
        sevenZipCompressor.CompressionMethod = CompressionMethod.Lzma;

        string dateTime = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
        var filePath = Path.Combine(sourceCodeFolder, $"{dateTime}_Logs.txt");
        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        using (var connection = new SqlConnection(settings.SqlConnectionString))
        {
            // Grab logs
            var procedure = "[GetArchiveableLogs]";
            var values = new { Timestamp = dateTime };
            var results = connection.Query<Log>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
            List<string> stringList = results.Select(i => i.ToString()).ToList();
            // Write to file
            File.WriteAllLines(filePath, stringList);
            StringBuilder stringBuilder = new StringBuilder();
            // Compress and offload file
            sevenZipCompressor.CompressFiles(Path.Combine(targetFolder, stringBuilder.AppendFormat("{0}{1}", dateTime, "_Archive.7z").ToString()), filePath);
            // Delete logs
            procedure = "DeleteArchiveableLogs";
            var rowsAffected = connection.Execute(procedure, values, commandType: CommandType.StoredProcedure);
        }
    }catch(Exception ex)
    {
        DateTime timestamp = DateTime.UtcNow;
        string level = "Server";
        string userHash = "System";
        string category = "Error";
        string description = ex.Message;
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} {1} {2} {3} {4}", timestamp.ToString(), level, userHash, category,
            description);
        string payload = builder.ToString();
        byte[] salt = new byte[0];
        byte[] key = KeyDerivation.Pbkdf2(payload, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
        string hash = Convert.ToHexString(key);
        ILog log = new Log(timestamp, level, userHash, category, description, hash);
        using (var connection = new SqlConnection(settings.SqlConnectionString))
        {
            var procedure = "[StoreLog]";
            var parameters = new DynamicParameters();
            parameters.Add("Timestamp", log.Timestamp);
            parameters.Add("Level", log.Level);
            parameters.Add("Userhash", log.UserHash);
            parameters.Add("Category", log.Category);
            parameters.Add("Description", log.Description);
            parameters.Add("Hash", log.Hash);
            parameters.Add("Destination", "ArchiveLogs");
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                commandType: CommandType.StoredProcedure, cancellationToken: _cancellationTokenSource.Token))
                .ConfigureAwait(false);
            switch (parameters.Get<int>("Result"))
            {
                case 0:
                    Console.WriteLine("503: Server: Log failed.");
                    break;
                case 1:
                    Console.WriteLine("200: Server: Log success.");
                    break;
                case 2:
                    Console.WriteLine("400: Database: Log rollback occurred.");
                    break;
                default:
                    Console.WriteLine("600: Server: A fatal error occurred.");
                    break;
            }
        }
    }


}