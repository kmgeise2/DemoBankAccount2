using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBankAccount2
{
        class LogEntries
        {
            public string[] logArray = new string[5];
            public int idx = 0;
            public LogEntries()
            {
                //This Constructor creates object with default values
            }
            public LogEntries(string[] sArray, int index)
            {
                //This Constructor creates object with passed values
                this.logArray = sArray;
                this.idx = index;
            }
            public bool EnterLog(string entry)
            {
                bool wasSuccessful = false;    
                string logEntry = entry;
                int lidx = 0;
                
                int numEntries = logArray.Length;
                bool lidxFound = false;

                // Find first empty array location
                for (int idx = 0; idx < numEntries; ++idx)
                {
                if ((logArray[idx] == null) && (!lidxFound))
                    {
                        lidx = idx;
                        lidxFound = true;
                    }
                }
                // Write the log entry to the array
                try
                {
                    logArray[lidx] = logEntry;
                    wasSuccessful = true;
                }
                catch
                {
                    // Index out of range; do nothing
                }
                return wasSuccessful;
            }

        }

    }
