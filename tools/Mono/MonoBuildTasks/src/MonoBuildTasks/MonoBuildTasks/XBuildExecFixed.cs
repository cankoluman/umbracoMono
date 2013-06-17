using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MonoBuildTasks
{
	public class XBuildExec : Task
	{
		[Required]
		public string OS {get; set;}

		[Required]
		public string Command {get; set;}

		public override bool Execute ()
		{
			ProcessStartInfo pInfo;

			string stdOutput = String.Empty;
			string stdError = String.Empty;

			try 
			{
				var cmdFixSemiColons = (OS.Equals("Windows_NT"))
					? Command
					: Command.Replace(";", @"\;"); 

				var cmd = GetCommandAndArgs(cmdFixSemiColons);

				if (cmd.Length == 2)
				{
					pInfo = new ProcessStartInfo(cmd[0], cmd[1]);
				}
				else
				{
					pInfo = new ProcessStartInfo(cmd[0]);
				}

				pInfo.UseShellExecute = false;	
				pInfo.CreateNoWindow = true;

				pInfo.RedirectStandardOutput = true;
				pInfo.RedirectStandardError = true;

				Log.LogMessageFromText(String.Format("Executing {0}", Command), MessageImportance.Normal);

				Process proc = Process.Start(pInfo);
				proc.WaitForExit();
				stdOutput = proc.StandardOutput.ReadToEnd();
				stdError = proc.StandardError.ReadToEnd();
				int exitCode = proc.ExitCode;
				proc.Close();

				Log.LogMessageFromText(String.Format("Result {0}: {1}{2}", exitCode, stdOutput, stdError), MessageImportance.Normal);

				return exitCode == 0;
			}
			catch (Exception ex) 
			{
				Log.LogErrorFromException(ex);
				return false;
			}
		}

		private string[] GetCommandAndArgs(string command)
		{
			return command
					.Trim()
					.Split(new char[]{' '}, 2, StringSplitOptions.RemoveEmptyEntries)
					.Select(item => item.Trim())
					.ToArray( );
		}
	}
}

