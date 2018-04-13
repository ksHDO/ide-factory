using System;
using System.Diagnostics;
using System.IO;

namespace AppBuilder
{
    public class CmdProcess : IDisposable
    {
        private Process m_process;
        private StreamWriter m_streamWriter;


        public static void RunCommand(string command)
        {
            Process p;
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/K " + command
            };
            p = Process.Start(psi);
            p.WaitForExit();
        }

        public CmdProcess()
        {
            var psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal
            };
            m_process = Process.Start(psi);
            m_streamWriter = m_process.StandardInput;
        }

        public void Run(string cmdCommand)
        {
            m_streamWriter.WriteLine(cmdCommand);
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                Run("exit");
                m_process?.WaitForExit();
                m_process?.Dispose();
                m_streamWriter?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}