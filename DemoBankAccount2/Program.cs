// Chapter 13 Handling Events
// You Do It Exercise
// Modified to display negative balances
// Modified to allow 'D' or 'd' for deposit
//   and 'W' or 'w' for withdrawal
// Modified to include LogEntries class
// Modified to log the balance changes to a text file
//
using System;
using static System.Console;
using System.IO;

namespace DemoBankAccount2
{
    class BankAccount
    {
        private int acctNum;
        private double balance;
        /*
        // Create an event handler using built-in handler
        // Notice that EventHandler is a delegate type in the 
        //    .NET Framework
        */
        public event EventHandler BalanceAdjusted; // Delegate declared
        public BankAccount(int acct)
        {
            acctNum = acct;
            balance = 0;
        }
        public int AcctNum
        {
            get
            {
                return acctNum;
            }
        }
        public double Balance
        {
            get
            {
                return balance;
            }
        }
        public void MakeDeposit(double amt)
        {
            balance += amt;

            // Invoke the OnBalanceAdjusted event handler
            OnBalanceAdjusted(EventArgs.Empty);
        }
        public void MakeWithdrawal(double amt)
        {
            balance -= amt;

            // Invoke the OnBalanceAdjusted event handler
            OnBalanceAdjusted(EventArgs.Empty);
        }

        // OnBalanceAdjusted is an event handler with only one argument
        public void OnBalanceAdjusted(EventArgs e)
        {
            BalanceAdjusted(this, e);
        }
    }
    class EventListener
    //Listens for BankAccount events
    {
        private BankAccount acct; //Contains a BankAccount object
        public LogEntries logbook; //Contains LogEntries object

        public EventListener(BankAccount account, LogEntries logBook) //Constructor
        {
            acct = account;
            logbook = logBook;

            //Add the BankAccountBalanceAdjusted method to the event delegate
            acct.BalanceAdjusted += new EventHandler
              (BankAccountBalanceAdjusted);
        }
        public void BankAccountBalanceAdjusted(object sender, EventArgs e)
        {
            string DELIM = ",";
            string logEntry;

            WriteLine("The account balance has been adjusted.");
            WriteLine("   Account# {0}  balance {1}",
              acct.AcctNum, acct.Balance.ToString("N2")); //Modified from C2
           
            // Write to array of log transactions
            logEntry = acct.AcctNum + DELIM + acct.Balance.ToString("N2");
            logbook.EnterLog(logEntry);
        }
    }

    class DemoBankAccountEvent
    // This program tests the BankAccount and EventListener classes
    {
        static void Main()
        {
            const int TRANSACTIONS = 5;
            char code;
            double amt;
            LogEntries logEntries1 = new LogEntries(); //Create with defaults
            BankAccount acct = new BankAccount(334455); 

            EventListener listener = new EventListener(acct, logEntries1);

            for (int x = 0; x < TRANSACTIONS; ++x)
            {
                Write("Enter D for deposit or W for withdrawal ");
                try
                {
                    code = Convert.ToChar(ReadLine());
                }
                catch
                {
                    code = 'd';
                }
                Write("Enter dollar amount ");
                try 
                { 
                    amt = Convert.ToDouble(ReadLine());
                }
                catch
                {
                    amt = 0.0;
                }

                if ((code == 'D') || (code == 'd'))
                    acct.MakeDeposit(amt);
                else if ((code == 'W') || (code == 'w'))
                    acct.MakeWithdrawal(amt);
                else acct.MakeDeposit(0);
            }
            // Write the log transactions to a file
            WriteToFile(logEntries1);

            static void WriteToFile(LogEntries logEnt)
            {
                // Map passed object to LogEntries object
                LogEntries logEntries1 = new LogEntries(); //Create with defaults
                logEntries1 = logEnt;

                // Added file parameters
                // Use Copy Path in Windows to find the file path
                //"C:\Users\kmgei\source\repos\DemoBankAccount2\"
                const string DIR = @"C:\Users\kmgei\source\repos\DemoBankAccount2\";
                const string FILENAME = DIR + "AccountLog.txt";

                // Check Directory exists
                if (!Directory.Exists(DIR))
                    Directory.CreateDirectory(DIR);

                // Get rid of old files
                if (File.Exists(FILENAME))
                    File.Delete(FILENAME);

                // Open or Create the file
                FileStream outFile = new FileStream(FILENAME,
                    FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(outFile);

                for (int idx = 0; idx < TRANSACTIONS; ++idx)
                {
                    // Write log file from an array 
                    //
                    string logEntry = logEntries1.logArray[idx];
                    writer.WriteLine(logEntry);

                    // Debugging
                    Console.WriteLine("Writing to file");
                    Console.WriteLine(logEntries1.logArray[idx]);

                }
                // The order of close must be writer first!!
                writer.Close();
                outFile.Close();
            }

        }
    }
}
