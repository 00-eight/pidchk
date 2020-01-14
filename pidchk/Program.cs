/*
    Copyright (C) 2019, Kristian Lamb

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Module: Program.cs

    Author: Kristian Lamb

*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace pidchk
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    break;
                default:
                    Console.WriteLine("{\"status\":\"" + "error\", \"errormessage\": \"" + "Invalid number args, usage pidchk.exe XXXXX-XXXXX-XXXXX-XXXXX-XXXXX\"" + "}");
                    return;
            }

            Regex rgx = new Regex(@"^([A-Za-z0-9]{5}-){4}[A-Za-z0-9]{5}$");
            if (!rgx.IsMatch(args[0]))
            {
                Console.WriteLine("{\"status\":\"" + "error\", \"errormessage\": \"" + "Productkey not in valid format XXXXX-XXXXX-XXXXX-XXXXX-XXXXX\"" + "}");
                return;
            }

            string productKey = args[0];
            //string pwd = Directory.GetCurrentDirectory();
            string pwd = Environment.CurrentDirectory;
            DirectoryInfo objDirectoryInfo = new DirectoryInfo(pwd);
            FileInfo[] pkconfigFiles = objDirectoryInfo.GetFiles("*.xrm-ms", SearchOption.AllDirectories);
            if (pkconfigFiles.Length != 0)
            {
                bool success = false;
                string lasterr = "";

                try
                {
                    PidChecker pidCheck;
                    pidCheck = new PidChecker();

                    foreach (var file in pkconfigFiles)
                    {
                        string PKeyPath = file.FullName;
                        string result = pidCheck.CheckProductKey(productKey, PKeyPath);
                        LicenseInfo obj = JsonConvert.DeserializeObject<LicenseInfo>(result);
                        if (obj.Status == "success")
                        {
                            success = true;
                            Console.WriteLine(result);
                            break;
                        }
                        lasterr = result;
                    }

                    if (!success)
                    {
                        Console.WriteLine(lasterr);
                    }
                }
                catch (System.DllNotFoundException)
                {
                    Console.WriteLine("{\"status\":\"" + "error\", \"errormessage\": \"" + "ProductKeyUtilities.dll from adk-vamt not found or missing\"" + "}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("{\"status\":\"" + "error\", \"errormessage\": \"" + "pkeyconfig files from adk-vamt not found or missing.\"" + "}");
            }
            return;
        }
    }
}
