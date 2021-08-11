using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ShadeX_Core
{
    public class Miner
    {
        private Process minerProcess;
        public bool isRunning;

        public Miner(string minerPath,string port, string pool, string wallletAdress,bool cuda,bool opencl)
        {
            // Port
            string args = " --http-port " + port;
            // OpenCl
            args += (opencl) ? " --opencl " : " ";
            // CUDA
            args += (cuda) ? " --cuda" : " ";
            // Pool
            args += " -o " + pool;
            // Wallet Adress
            args += " -u " + wallletAdress;
            args += " -k --tls";

            minerProcess = new Process();
            minerProcess.StartInfo = new ProcessStartInfo() {
            FileName = minerPath,
            Arguments = args,
            CreateNoWindow = true,
                UseShellExecute = false,
            };
        }

        public void Start()
        {
            isRunning = true;
            minerProcess.Start();
        }

        public void Stop()
        {
            isRunning = false;
            minerProcess.Kill();
            
        }
    }
}
