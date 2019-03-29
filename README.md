# EventLogParser

```
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
```

## Examples

```
.\EventLogParser.exe eventid=4104
[*] Parsing PowerShell 4104 event logs...

[+] Regex Match: net user $NewOsUser $NewOsPass /add & net localgroup administrators /add $NewOsUser'';"
[+] Regex Context:
        # Create query
        }else{
        Break
        Write-Verbose "$Instance : The service account does not have local administrator privileges so no OS admin can be created.  Aborted."
        net user $NewOsUser $NewOsPass /add & net localgroup administrators /add $NewOsUser'';"
        # Status user
        Write-Verbose "$Instance : Payload generated."
        }
        }else{

[+] Regex Match: New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($Username, $secpass)
[+] Regex Context:
        $secpass = ConvertTo-SecureString $Password -AsPlainText -Force
        {
        if($Username -and $Password)
        # Create PS Credential object
        New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($Username, $secpass)
        }
        # Create Create the connection to LDAP
        if ($DomainController)
        {

[+] Regex Match: ConvertTo-SecureString $Password -AsPlainText -Force
[+] Regex Context:
        {
        if($Username -and $Password)
        # Create PS Credential object
        {
        ConvertTo-SecureString $Password -AsPlainText -Force
        $Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($Username, $secpass)
        }
        # Create Create the connection to LDAP
        if ($DomainController)
```
