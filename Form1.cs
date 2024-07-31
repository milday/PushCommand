using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Windows.Forms;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;

namespace PushCommand
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            commandTextBox.Text = "";
            this.StartPosition = FormStartPosition.CenterScreen;
            logTextBox.ReadOnly = true;
            lapsCheckBox.CheckedChanged += LapsCheckBox_CheckedChanged;
            LapsCheckBox_CheckedChanged(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            string hostname = hostnameTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string command = commandTextBox.Text;

            // Resolve IP to hostname if needed
            if (IsIPAddress(hostname))
            {
                hostname = GetHostNameFromIPAddress(hostname);
                if (string.IsNullOrEmpty(hostname))
                {
                    MessageBox.Show("Failed to resolve IP address to hostname.");
                    return;
                }
            }

            if (lapsCheckBox.Checked)
            {
                if (IsIPAddress(hostname))
                {
                    MessageBox.Show("Please use hostname instead of IP address when LAPS is enabled.");
                    return;
                }

                password = GetLapsPassword(GetShortHostName(hostname));
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Failed to retrieve password from LAPS.");
                    return;
                }

                username = "admdesktop";
            }
            else
            {
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Password cannot be empty when LAPS is not enabled.");
                    return;
                }
            }

            if (IsLocalMachine(hostname))
            {
                string result = ExecuteLocalCommand(command);
                logTextBox.AppendText(result + Environment.NewLine);
            }
            else
            {
                string result = ExecuteRemoteCommandWithWinRMCheck(hostname, username, password, command);
                logTextBox.AppendText(result + Environment.NewLine);
            }
        }

        private bool IsLocalMachine(string hostname)
        {
            string localHostname = Environment.MachineName;
            return string.Equals(hostname, localHostname, StringComparison.OrdinalIgnoreCase);
        }

        private string ExecuteLocalCommand(string command)
        {
            string result = string.Empty;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    result = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(error))
                    {
                        result += "\nError: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"Error executing local command: {ex.Message}\n{ex.StackTrace}";
            }

            return result;
        }

        private bool IsIPAddress(string hostname)
        {
            return IPAddress.TryParse(hostname, out _);
        }

        private string GetHostNameFromIPAddress(string ipAddress)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error resolving IP address: " + ex.Message);
                return null;
            }
        }

        private string GetShortHostName(string fullHostName)
        {
            if (fullHostName.Contains("."))
            {
                return fullHostName.Split('.')[0];
            }

            return fullHostName;
        }

        private string GetLapsPassword(string hostname)
        {
            string password = string.Empty;

            try
            {
                using (PowerShell ps = PowerShell.Create())
                {
                    // Set ExecutionPolicy to allow script to run
                    ps.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("ExecutionPolicy", "Bypass")
                      .AddParameter("Scope", "Process")
                      .AddParameter("Force", true);
                    ps.Invoke();

                    // Define paths for module
                    string modulePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WindowsPowerShell", "Modules", "AdmPwd.PS");
                    string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "AdmPwd.PS");

                    // Check if the module is installed and copy if necessary
                    ps.AddScript($@"
                if (-not (Test-Path -Path '{modulePath}')) {{
                    if (Test-Path -Path '{sourcePath}') {{
                        Copy-Item -Recurse -Path '{sourcePath}' -Destination '{modulePath}'
                    }} else {{
                        throw 'Source path does not exist.'
                    }}
                }}
            ");
                    ps.Invoke();

                    if (ps.HadErrors)
                    {
                        foreach (var error in ps.Streams.Error)
                        {
                            MessageBox.Show("Error during module setup: " + error.ToString());
                        }
                        return string.Empty;
                    }

                    // Import the module
                    ps.AddScript("Import-Module -Name 'AdmPwd.PS' -ErrorAction Stop");
                    ps.Invoke();

                    if (ps.HadErrors)
                    {
                        foreach (var error in ps.Streams.Error)
                        {
                            MessageBox.Show("Error importing module: " + error.ToString());
                        }
                        return string.Empty;
                    }

                    // Retrieve the password
                    ps.AddScript($"Get-AdmPwdPassword -ComputerName {hostname}");
                    var results = ps.Invoke();

                    if (ps.HadErrors)
                    {
                        foreach (var error in ps.Streams.Error)
                        {
                            MessageBox.Show("Error retrieving password: " + error.ToString());
                        }
                        return string.Empty;
                    }

                    if (results.Count > 0)
                    {
                        password = results[0].Properties["Password"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving password: " + ex.Message);
            }

            return password;
        }



        private string ExecuteRemoteCommandWithWinRMCheck(string hostname, string username, string password, string command)
        {
            string result = string.Empty;

            try
            {
                ConnectionOptions options = new ConnectionOptions
                {
                    Username = $"{hostname}\\{username}",
                    Password = password,
                    EnablePrivileges = true,
                    Impersonation = ImpersonationLevel.Impersonate
                };

                ManagementScope scope = new ManagementScope($"\\\\{hostname}\\root\\cimv2", options);
                scope.Connect();

                if (!IsWinRMEnabled(scope))
                {
                    result = EnableWinRM(scope);
                    result += ConfigureTrustedHosts(scope);
                }

                result += ExecuteRemoteCommand(hostname, username, password, command);
            }
            catch (Exception ex)
            {
                result = $"Error: {ex.Message}\n{ex.StackTrace}";
                Debug.WriteLine($"Exception occurred: {ex}");
            }

            return result;
        }

        private bool IsWinRMEnabled(ManagementScope scope)
        {
            try
            {
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Service WHERE Name = 'WinRM'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection queryCollection = searcher.Get();

                foreach (ManagementObject service in queryCollection)
                {
                    if (service["State"].ToString().ToLower() == "running")
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking WinRM status: " + ex.Message);
            }

            return false;
        }

        private string EnableWinRM(ManagementScope scope)
        {
            string result = string.Empty;

            try
            {
                ManagementClass processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                inParams["CommandLine"] = "cmd /c winrm quickconfig -force";

                ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

                result = $"WinRM enabled. Process ID: {outParams["processId"]}\nReturn Value: {outParams["returnValue"]}\n";
            }
            catch (Exception ex)
            {
                result = $"Error enabling WinRM: {ex.Message}\n{ex.StackTrace}";
            }

            return result;
        }

        private string ConfigureTrustedHosts(ManagementScope scope)
        {
            string result = string.Empty;

            try
            {
                ManagementClass processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                inParams["CommandLine"] = "cmd /c winrm set winrm/config/client '@{TrustedHosts=\"*\"}'";

                ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

                result = $"TrustedHosts configured. Process ID: {outParams["processId"]}\nReturn Value: {outParams["returnValue"]}\n";
            }
            catch (Exception ex)
            {
                result = $"Error configuring TrustedHosts: {ex.Message}\n{ex.StackTrace}";
            }

            return result;
        }

        private string ExecuteRemoteCommand(string hostname, string username, string password, string command)
        {
            string result = string.Empty;

            try
            {
                using (PowerShell ps = PowerShell.Create())
                {
                    // Set ExecutionPolicy to allow script to run
                    ps.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("ExecutionPolicy", "Bypass")
                      .AddParameter("Scope", "Process")
                      .AddParameter("Force", true);
                    ps.Invoke();

                    string script = $"Invoke-Command -ComputerName {hostname} -Credential (New-Object System.Management.Automation.PSCredential('{hostname}\\{username}', (ConvertTo-SecureString '{password}' -AsPlainText -Force))) -ScriptBlock {{ cmd /c {command} }}";

                    ps.AddScript(script);

                    var results = ps.Invoke();

                    foreach (var output in results)
                    {
                        result += output.ToString() + Environment.NewLine;
                    }

                    if (ps.HadErrors)
                    {
                        foreach (var error in ps.Streams.Error)
                        {
                            result += "Error: " + error.ToString() + Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"Error executing remote command: {ex.Message}\n{ex.StackTrace}";
            }

            return result;
        }

        private void clearLogButton_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
        }

        private void LapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (lapsCheckBox.Checked)
            {
                usernameTextBox.Text = "admdesktop";
                usernameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
                passwordTextBox.Text = string.Empty;
            }
            else
            {
                usernameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
            }
        }
    }
}
