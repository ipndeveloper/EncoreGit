using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using Microsoft.VisualBasic.Devices;

namespace NetSteps.Data.Entities.Business
{
    public class ServerSystemInformation
    {
        private readonly ComputerInfo _computerInfo = new ComputerInfo();
        private static string _ipAddresses;
        private const int _bytesPerMegabyte = (1 << 20);

        public string IpAddresses
        {
            get
            {
                if (String.IsNullOrEmpty(_ipAddresses))
                {
                    _ipAddresses = string.Join(" ", Dns.GetHostAddresses(Dns.GetHostName())
                                                        .Where(x => x.ToString().Contains('.'))
                                                        .Select(x => x.ToString())
                                                        .OrderBy(x => x));
                }
                return _ipAddresses;
            }
        }

        public long DatabaseResponseTime 
        { 
            get
            {
                return GetDatabaseResponseTime();
            }
        }

        public string CodeVersion
        {
            get { return GetVersionNumber(); }
        }

        public int CpuUsage { get { return GetCpuLoadCounter();  } }
        public int AvailablePhysicalMemory { get { return Convert.ToInt32(_computerInfo.AvailablePhysicalMemory / (decimal)_bytesPerMegabyte); } }
        public int AvailableVirtualMemory { get { return Convert.ToInt32(_computerInfo.AvailableVirtualMemory / (decimal)_bytesPerMegabyte); } }
        public int TotalPhysicalMemory { get { return Convert.ToInt32(_computerInfo.TotalPhysicalMemory / (decimal)_bytesPerMegabyte); } }
        public int TotalVirtualMemory { get { return Convert.ToInt32(_computerInfo.TotalVirtualMemory / (decimal)_bytesPerMegabyte); } }

        public int PhysicalMemoryUsed
        {
            get { return TotalPhysicalMemory - AvailablePhysicalMemory; }
        }

        public int MemoryUsage
        {
            get { return PhysicalMemoryUsed / TotalPhysicalMemory; }
        }

        public string OperatingSystem { get { return _computerInfo.OSFullName; } }
        public Dictionary<string, string> DriveSpace { get { return GetDriveSpace(); } }

        private string GetVersionNumber()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion;
        }

        private long GetDatabaseResponseTime()
        {
            var _watch = Stopwatch.StartNew();
            AccountPropertyType.LoadAll();
            MailDomain.Load(1);
            _watch.Stop();
            return _watch.ElapsedMilliseconds;
        }

        private Dictionary<string, string> GetDriveSpace()
        {
            var drives = new Dictionary<string, string>();
            foreach (var drive in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                drives.Add(drive.Name, Convert.ToInt32(drive.TotalFreeSpace / (decimal)_bytesPerMegabyte).ToString() + "MB");
            }
            return drives;
        }

        private int GetCpuLoadCounter()
        {
            using (var searcher = new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name=\"_Total\"")))
            {
                var cpuPerfObj = searcher.Get().Cast<ManagementObject>().First();
                return 100 - Convert.ToInt32(cpuPerfObj["PercentIdleTime"]);
            }
        }
    }
}
