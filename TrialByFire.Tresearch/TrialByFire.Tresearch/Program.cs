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
using TrialByFire.Tresearch.Services;
using System.Net.Mail;

namespace TrialByFire.Tresearch.Main
{
    public class Program
    {
        public static Account UserAccount;
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

            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AuthenticationService authenticationService = new AuthenticationService(mssqlDAO, logService);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);
            UserAccount = GetAccount(mssqlDAO, logService, authenticationService);

            view = GetView();

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
                    case "System Admin":
                        Console.WriteLine("1. UM View \n 2. Quit");
                        try
                        {
                            operation = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            InvalidNumberInput();
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
                    case "UMView":
                        Console.WriteLine("1. Create Account \n2. Update Account \n3. Delete Account \n4. Disable Account " +
                            "\n5. Enable Account \n6. Bulk Operation \n7. Go Back");
                        try
                        {
                            operation = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            InvalidNumberInput();
                        }
                        switch (operation)
                        {
                            case 1:
                                if (VerifyAuthorization(UserAccount, "System Admin", mssqlDAO, logService))
                                {
                                    string creationEmail;
                                    string creationPassphrase;
                                    string creationAuthorizationLevel;
                                    try
                                    {
                                        Console.WriteLine("Please enter an email");
                                        creationEmail = Console.ReadLine();
                                        Console.WriteLine("Please enter a passphrase at least eight characters in length");
                                        creationPassphrase = Console.ReadLine();
                                        Console.WriteLine("Please enter Authorization Level");
                                        creationAuthorizationLevel = Console.ReadLine();
                                        bool isValidEmail = ValidateEmail(creationEmail, logService);
                                        bool isValidPassphrase = ValidatePassphrase(creationPassphrase);
                                        bool isValidAuthorizationLevel = ValidateAuthorizationLevel(creationAuthorizationLevel);
                                        if(isValidEmail || isValidPassphrase == false)
                                        {
                                            Console.WriteLine("Invalid Username or Passphrase");
                                            break;
                                        }

                                        if (isValidAuthorizationLevel == false)
                                        {
                                            Console.WriteLine("Authorization Failed");
                                        }

                                        bool createAccountSuccessful = accountManager.CreateAccount(creationEmail, creationPassphrase, creationAuthorizationLevel);
                                        if (createAccountSuccessful)
                                        {
                                            Console.WriteLine("Create Account was Successful");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                    break;
                                }
                                break;
                            case 2:
                                if (VerifyAuthorization(UserAccount, "System Admin", mssqlDAO, logService))
                                {
                                    
                                    try
                                    {
                                        string existingUsername;
                                        string newEmail = "";
                                        string newPassphrase = "";
                                        string newAuthorizationLevel = "";
                                        Console.WriteLine("Enter existing username");
                                        existingUsername = Console.ReadLine();
                                        bool isValidUsername = ValidateUsername(existingUsername, logService);
                                        bool isValidEmail = false;
                                        bool isVaildPassphrase = false;
                                        bool isValidAuthorizationLevel = false;
                                        if(isValidUsername == false)
                                        {
                                            Console.WriteLine("Username is not valid");
                                            break;
                                        }
                                        string eChoice;
                                        string pChoice;
                                        string aChoice;
                                        Console.WriteLine("Would you like to update email? Enter: y/n");
                                        eChoice = Console.ReadLine();
                                        Console.WriteLine("Would you like to update passphrase? Enter: y/n");
                                        pChoice = Console.ReadLine();
                                        Console.WriteLine("Would you like to update authorizationLevel? Enter: y/n");
                                        aChoice = Console.ReadLine();
                                        if((eChoice == "n") && (pChoice == "n") && (aChoice == "n")){
                                            Console.WriteLine("Update Account Failed");
                                            break;
                                        }
                                        if(eChoice == "y")
                                        {
                                            newEmail = Console.ReadLine();
                                        }
                                        if(passphrase == "y")
                                        {
                                            newPassphrase = Console.ReadLine();
                                        }
                                        if(aChoice == "y")
                                        {
                                            newAuthorizationLevel = Console.ReadLine();
                                        }
                                        isValidEmail = ValidateEmail(newEmail, logService);

                                        if (isValidEmail == false)
                                        {
                                            Console.WriteLine("Email is not valid");
                                            break;
                                        }

                                        isVaildPassphrase = ValidatePassphrase(newPassphrase);

                                        if (isVaildPassphrase == false)
                                        {
                                            Console.WriteLine("Passphrase is not valid");
                                            break;
                                        }

                                        isValidAuthorizationLevel = ValidateAuthorizationLevel(newAuthorizationLevel);

                                        if (isValidAuthorizationLevel == false)
                                        {
                                            Console.WriteLine("AuthorizationLevel is invalid");
                                            break;
                                        }

                                        bool updateAccountSuccessful = accountManager.UpdateAccount(existingUsername, newEmail, newPassphrase, newAuthorizationLevel);
                                        
                                        if (updateAccountSuccessful)
                                        {
                                            Console.WriteLine("Update Account was Successful");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                    break;
                                }
                                break;
                            case 3:
                                if (VerifyAuthorization(UserAccount, "System Admin", mssqlDAO, logService))
                                {
                                    Console.WriteLine("Enter Username for deletion");
                                    string deletionUsername;

                                    try
                                    {
                                        deletionUsername = Console.ReadLine();
                                        bool isValidUsername = ValidateUsername(deletionUsername, logService);

                                        if (isValidUsername == false)
                                        {
                                            Console.WriteLine("Invalid Username");
                                            break;
                                        }

                                        bool isDeleted = accountManager.DeleteAccount(deletionUsername);
                                        if (isDeleted)
                                        {
                                            Console.WriteLine("Delete Account was Successful");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                    break;
                                }
                                break;
                            case 4:
                                if (VerifyAuthorization(UserAccount, "System Admin", mssqlDAO, logService))
                                {
                                    Console.WriteLine("Enter Username");
                                    string usernameToDisable;

                                    try
                                    {
                                        usernameToDisable = Console.ReadLine();
                                        bool isValidUsername = ValidateUsername(usernameToDisable, logService);

                                        if (isValidUsername == false)
                                        {
                                            Console.WriteLine("Invalid Username");
                                            break;
                                        }

                                        bool isEnabled = accountManager.DisableAccount(usernameToDisable);
                                        if (isEnabled)
                                        {
                                            Console.WriteLine("Disable Account Was Successful");
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                                break;
                            case 5:
                                if (VerifyAuthorization(UserAccount, "System Admin", mssqlDAO, logService))
                                    {
                                        Console.WriteLine("Enter Username and Email");
                                        string usernameToEnable;
                                        string emailToEnable;

                                        try
                                        {
                                            usernameToEnable = Console.ReadLine();
                                            emailToEnable = Console.ReadLine();
                                            bool isValidUsername = ValidateUsername(usernameToEnable, logService);
                                            bool isValidEmail = ValidateEmail(emailToEnable, logService);

                                            if(isValidUsername == false || isValidEmail == false)
                                            {
                                                break;
                                            }

                                             bool isEnabled = accountManager.EnableAccount(usernameToEnable, emailToEnable);
                                            if (isEnabled)
                                            {
                                                Console.WriteLine("Enable Account Was Successful");
                                            }
                                          
                                        }
                                        catch (Exception e)
                                        {
                                        Console.WriteLine(e);
                                        }
                                    }
                                break;
                            case 6:
                                //MSSQLDAO mssqlDAO = new MSSQLDAO();
                                //LogService logService = new LogService(mssqlDAO);
                                if(VerifyAuthorization(UserAccount, "System Admin", mssqlDAO, logService))
                                {
                                    Console.WriteLine("Please enter in the file path containing the operation requests.");
                                    string file;
                                    try
                                    {
                                        file = Console.ReadLine();
                                        if(File.Exists(file))
                                        {
                                            try
                                            {
                                                int successes = ReadFile(file, mssqlDAO, logService);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error, the file could not be read. Please try again.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error, the file path is not valid. Please try again.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine();
                                    }
                                }
                                break;
                            case 7:
                                view = "System Admin";
                                break;
                            default:
                                Console.WriteLine("Error, invalid operation. Please try again.");
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
        
        public static Account GetAccount(MSSQLDAO mssqlDAO, LogService logService, AuthenticationService authenticationService)
        {
            string username = "";
            string passphrase = "";

            while (username.Equals("") && passphrase.Equals(""))
            {
                // need to validate these two
                Console.WriteLine("Please enter in your username.");
                username = Console.ReadLine();
                Console.WriteLine("Please enter in your passphrase.");
                passphrase = Console.ReadLine();

                UserAccount = authenticationService.GetAccount(username, passphrase);

                if(UserAccount == null)
                {
                    Console.WriteLine("Invalid Username/password or Disabled account");
                    username = "";
                    passphrase = "";
                }
            }
            return UserAccount;
        }
        
        public static string GetView()
        {
            string view = "";
            switch (UserAccount.AuthorizationLevel)
            {
                case "User":
                    view = "User";
                    break;
                case "System Admin":
                    view = "System Admin";
                    break;
                default:
                    Console.Write("Error, user has unknown authorization level. System shutting down.");
                    Environment.Exit(1);
                    break;
            }
            return view;
        }
        
        public static bool ValidateUsername(string username, LogService logService)
        {
            bool isValidEmail = false;
            try
            {
                MailAddress checkAddress = new MailAddress(username);
                isValidEmail = (checkAddress.Address == username);
                return true;
            }catch(FormatException ex)
            {
                logService.CreateLog(DateTime.Now, "Error", UserAccount.Username, "Business", ex.Message);
                
            }
            return false;
        }

        public static bool ValidateEmail(string username, LogService logService)
        {
            bool isValidEmail = false;
            try
            {
                MailAddress checkAddress = new MailAddress(username);
                isValidEmail = (checkAddress.Address == username);
                return true;
            }
            catch (FormatException ex)
            {
                logService.CreateLog(DateTime.Now, "Error", UserAccount.Username, "Business", ex.Message);
            }
            return false;
        }
        
        public static bool ValidatePassphrase(string passphrase)
        {
            bool isValidPassphrase = false;
            try
            {
                isValidPassphrase = (passphrase.Length >= 8);
                return true;
            }
            catch(FormatException e)
            {
                Console.WriteLine("Invalid passphrase. Try again");
            }
            return false;
        }

        public static bool ValidateAuthorizationLevel(string authorizationLevel)
        {
            bool isValidAuthorizationLevel = false;
            try
            {
                if ((authorizationLevel == "User") || (authorizationLevel == "System Admin"))
                {
                    isValidAuthorizationLevel = true;
                    return isValidAuthorizationLevel;
                }
            }
            catch(FormatException e)
            {
                Console.WriteLine("Invalid AuthorizationLevel. Try again");
            }
            return false;
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

        public static bool VerifyAuthorization(Account account, string requiredAuthorizationLevel, MSSQLDAO mssqlDAO, 
            LogService logService)
        {
            try
            {
                AuthorizationService authorizationService = new AuthorizationService(mssqlDAO, logService);
                return authorizationService.GetAccountAuthLevel(account, requiredAuthorizationLevel);
            }
            catch (Exception ex)
            {
                logService.CreateLog(DateTime.Now, "Error", UserAccount.Username, "Business", ex.Message);
                return false;
            }
        }

        public static int ReadFile(string file, MSSQLDAO mssqlDAO, LogService logService)
        {
            int successes = 0;
            int requests = 0;
            AccountService accountService = new AccountService(mssqlDAO, logService);
            foreach (string line in File.ReadLines("@" + file))
            {
                requests++;
                string[] request = line.Split(';');
                try
                {
                    switch (request[0])
                    {
                        case "Create":
                            if (accountService.CreateAccount(request[1], request[2], request[3]))
                            {
                                successes++;
                            }
                            break;
                        case "Update":
                            if (accountService.UpdateAccount(request[1], request[2], request[3],
                                request[4]))
                            {
                                successes++;
                            }
                            break;
                        case "Delete":
                            if (accountService.DeleteAccount(request[1]))
                            {
                                successes++;
                            }
                            break;
                        case "Disable":
                            if (accountService.DisableAccount(request[1]))
                            {
                                successes++;
                            }
                            break;
                        case "Enable":
                            if (accountService.EnableAccount(request[1], request[2]))
                            {
                                successes++;
                            }
                            break;
                        default:
                            logService.CreateLog(DateTime.Now, "Error", UserAccount.Username,
                                "Business", "Improper request detected in bulk operation.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logService.CreateLog(DateTime.Now, "Error", UserAccount.Username, "Business",
                        ex.Message);
                }
            }
            logService.CreateLog(DateTime.Now, "Info", UserAccount.Username, "Business", 
                $"Bulk Operation Was Successful with {successes} successes in {requests} requests.");
            return successes;
        }
    }
}
