using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Managers;

namespace TrialByFire.Tresearch.Main
{

    public class Program
    {
        public static async Task Main(string[] args)
        {
            string sqlConnectionString = "";
            string filePath = "";
            string destination = "";
            string username = "";
            string passphrase = "";
            string view = "";
            int operation = -1;
            bool finished = false;
            bool isAuthorized = false;
            bool isValidOperation = false;
            bool isValidFilePath;
            Account UserAccount = new Account();

            while (sqlConnectionString.Equals(""))
            {
                Console.WriteLine("Please input your sql connection string");
                sqlConnectionString = Console.ReadLine();
                try
                {
                    using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                    {
                        connection.Open();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid connection string. Please try again.\n");
                    sqlConnectionString = "";
                }
            }
            ConfigurationManager.AppSettings.Set("SqlConnectionString", sqlConnectionString);

            while (filePath.Equals(""))
            {
                Console.WriteLine("Please input a path to a destination to create the file for the logs.");
                filePath = Console.ReadLine();
                if(!Directory.Exists(filePath))
                {
                    Console.WriteLine("Invalid path. Please try again. \n");
                    filePath = "";
                }
            }
            ConfigurationManager.AppSettings.Set("FilePath", filePath);

            while (destination.Equals(""))
            {
                Console.WriteLine("Please input a path to a desination to create the zip file for archiving. " +
                    "(This should be different from the path for the file.)");
                destination = Console.ReadLine();
                if (!Directory.Exists(filePath) || destination == filePath)
                {
                    Console.WriteLine("Invalid path. Please try again. \n");
                    destination = "";
                }
            }
            ConfigurationManager.AppSettings.Set("Destination", destination);

            // Archiving Service
            /*MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            ArchivingManager archivingManager = new ArchivingManager(mssqlDAO, logService);
            Task<bool> archiveTask = archivingManager.ArchiveLogs("2021-12-14 22:12:00");
            bool isArchived = await archiveTask;
            Console.WriteLine(isArchived);*/

            //Authenticate and store account
            while (username.Equals("")  && passphrase.Equals(""))
            {
                // need to validate these two
                Console.WriteLine("Please enter in your username.");
                username = Console.ReadLine();
                Console.WriteLine("Please enter in your passphrase.");
                passphrase = Console.ReadLine();

                UserAccount = new Account();
            }

            switch (UserAccount.AuthorizationLevel)
            {
                case "User":
                    view = "User";
                    break;
                case "System Admin":
                    view = "SysAdmin";
                    break;
                default:
                    Console.Write("Error, user has unknown authorization level. System shutting down.");
                    Environment.Exit(1);
                    break;
            }

            while (!finished)
            {
                switch (view)
                {
                    case "User":
                        Console.WriteLine("1. Quit");
                        try
                        {
                            operation = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            InvalidNumberInput();
                            return;
                        }
                        switch (operation)
                        {
                            case 1:
                                Console.WriteLine("System shutting down.");
                                Environment.Exit(0);
                                break;
                            default:
                                InvalidNumberInput();
                                break;
                        }
                        break;
                    case "SysAdmin":
                        Console.WriteLine("1. UM View \n 2. Quit");
                        try
                        {
                            operation = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            InvalidNumberInput();
                            return;
                        }
                        switch (operation)
                        {
                            case 1:
                                switch (UserAccount.AuthorizationLevel)
                                {
                                    case "User":
                                        InvalidAuthorizationLevel();
                                        break;
                                    case "System Admin":
                                        view = "UMView";
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 2:
                                Console.WriteLine("System shutting down.");
                                Environment.Exit(0);
                                break;
                            default:
                                InvalidNumberInput();
                                break;

                        }
                        break;
                    case "UM":
                        Console.WriteLine("1. Create Account \n2. Update Account \n3. Delete Account \n4. Disable Account " +
                            "\n5. Enable Account \n6. Bulk Operation \n7. Go Back");
                        try
                        {
                            operation = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            InvalidNumberInput();
                            return;
                        }
                        switch (operation)
                        {
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                break;
                            case 6:
                                break;
                            case 7:
                                view = "SysAdmin";
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        Console.Write("Error, no view has been set. System shutting down.");
                        Environment.Exit(1);
                        break;
                }
            }
        }

        public static void InvalidNumberInput()
        {
            Console.WriteLine("Input is not a valid number. Please try again.");
        }

        public static void InvalidAuthorizationLevel()
        {
            Console.WriteLine("You are not authorized to perform this action.");
        }

        public static bool ValidateOperation(string operation)
        {
            throw new NotImplementedException();
        }
    }
}
