using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DomainModels;

namespace TrialByFire.Tresearch.Main
{
    public class Program
    {
        static string GlobalSqlConnectionString;
        static string GlobalFilePath;
        static string GlobalDestination;
        public static void Main(string[] args)
        {
            string sqlConnectionString = "";
            string filePath = "";
            string destination = "";
            string username = "";
            string passphrase = "";
            int operation = -1;
            Account userAccount = new Account();
            bool finished = false;


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
            GlobalSqlConnectionString = sqlConnectionString;

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
            GlobalFilePath = filePath;

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
            GlobalDestination = destination;

            //Authenticate and store account
            while(username.Equals("")  && passphrase.Equals(""))
            {
                // need to validate these two
                Console.WriteLine("Please enter in your username.");
                username = Console.ReadLine();
                Console.WriteLine("Please enter in your passphrase.");
                passphrase = Console.ReadLine();
                
            }

            while(!finished)
            {
                switch(userAccount.AuthorizationLevel)
                {
                    case "User":
                        Console.WriteLine("1. Quit");
                        break;
                    case "System Admin":
                        Console.WriteLine("1. UM View \n2. Quit");
                        break;
                }
            }


        }
    }
}
