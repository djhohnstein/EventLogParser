using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Eventing.Reader;
using EventLogParser;

namespace EventLogParser
{
    class Program
    {

        static void Usage()
        {
            string usageString = @"
Usage
=====

EventLogParser.exe eventid=EVENTID [outfile=C:\Windows\Temp\loggedfiles.txt]

Description:
    
    EventLogParser will parse event IDs 4103, 4104 and 4688 to search for sensitive
    information, including:
        - RDP Credentials
        - net user commands
        - Plaintext secure-strings
        - PSCredential objects
        - SSH commands using keys
        - Imported powershell modules.

Arguments:
    
    Required:
        
        eventid - Must be one of:
                    4103 - Script Block Logging
                    4104 - PowerShell module logging
                    4688 - Process Creation logging.
                           Note: Must be high integrity and have
                                 command line logging enabled.

    Optional:
   
        context - Number of lines surrounding the ""interesting"" regex matches.
                  Only applies to 4104 events. Default is 3.

        outfile - Path to the file you wish to write all matching script block logs
                  to. This only applies to event ID 4104.

Example:

    .\EventLogParser.exe eventid=4104 outfile=C:\Windows\Temp\scripts.txt context=5

        Writes all 4104 events with ""sensitive"" information to C:\Windows\Temp\scripts.txt
        and prints 5 lines before and after the matching line.

    .\EventLogParser.exe eventid=4103
        
        List all modules path on disk that have been loaded by each user.
";
            Console.WriteLine(usageString);
            Environment.Exit(1);
        }

        static Dictionary<string, string> ArgumentParser(string[] args)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            results["context"] = "3";
            results["outfile"] = "";
            char[] sep = { '=' };
            foreach (string arg in args)
            {
                if (arg.Contains("="))
                {
                    string[] parts = arg.Split(sep, 2);
                    if (parts.Length == 2)
                    {
                        results[parts[0].Trim().ToLower()] = parts[1].Trim();
                    }
                    else
                    {
                        Console.WriteLine("[-] Invalid argument passed. Skipping {0}.", arg);
                    }
                }
                else
                {
                    Console.WriteLine("[-] Invalid argument passed. Skipping {0}.", arg);
                }
            }
            if (!results.ContainsKey("eventid"))
            {
                Console.WriteLine("[X] No eventid passed.");
                Usage();
            }
            if (!EventLogHelpers.supportedEventIds.ContainsKey(results["eventid"]))
            {
                Console.WriteLine("[X] Invalid eventid passed. You gave: {0}", results["eventid"]);
                Usage();
            }
            return results;
        }

        static void Main(string[] args)
        {
            Dictionary<string, string> arguments = ArgumentParser(args);
            string eventid = arguments["eventid"];
            if (eventid == "4104")
            {
                EventLogHelpers.supportedEventIds[eventid].DynamicInvoke(arguments["outfile"], arguments["context"]);
            }
            else
            {
                EventLogHelpers.supportedEventIds[eventid].DynamicInvoke();
            }
            Console.WriteLine("[*] Finished parsing {0} logs.", eventid);
        }
    }
}
