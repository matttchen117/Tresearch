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
CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));
string targetFolder = settings.Destination;
string sourceCodeFolder = settings.Source;
if (System.IO.Directory.Exists(targetFolder))
{
    try
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
        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        using (var connection = new SqlConnection(settings.SqlConnectionString))
        {
            var procedure = "[GetLogs]";
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
        }
        // Compress the directory and save the file in a yyyyMMdd_project-files.7z format (eg. 20141024_project-files.7z

        //sevenZipCompressor.CompressDirectory(sourceCodeFolder, Path.Combine(targetFolder, string.Concat(DateTime.Now.ToString("yyyyMMdd"), "_project-files.7z")));
    }catch(Exception ex)
    {
        DateTime timestamp = DateTime.UtcNow;
        string level = "Server";
        string username = "System";
        string category = "Error";
        string description = ex.Message;
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} {1} {2} {3} {4}", timestamp.ToString(), level, username, category,
            description);
        string payload = builder.ToString();
        byte[] salt = new byte[0];
        byte[] key = KeyDerivation.Pbkdf2(payload, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
        string hash = Convert.ToHexString(key);
        ILog log = new Log(timestamp, level, username, category, description, hash);
        using (var connection = new SqlConnection(settings.SqlConnectionString))
        {
            var procedure = "[StoreLog]";
            var parameters = new DynamicParameters();
            parameters.Add("Timestamp", log.Timestamp);
            parameters.Add("Level", log.Level);
            parameters.Add("Username", log.Username);
            parameters.Add("Category", log.Category);
            parameters.Add("Description", log.Description);
            parameters.Add("Hash", log.Hash);
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