using ComputerRepairStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Manager
{
    public class SoftwareRepairManager : IRepairManager
    {
        private static readonly List<(string Keyword, string ServiceName, double Cost)> ServiceMap =
            new List<(string, string, double)>
            {
                ("virus",     "Malware Removal Tool Suite Deployment",     2000),
                ("malware",   "Malware Removal Tool Suite Deployment",     2000),
                ("spyware",   "Malware Removal Tool Suite Deployment",     2000),
                ("ransomware","Malware Removal Tool Suite Deployment",     2000),
                ("os",        "OS License Key Update",                     2000),
                ("windows",   "OS License Key Update",                     2000),
                ("install",   "OS License Key Update",                     2000),
                ("reinstall", "OS License Key Update",                     2000),
                ("driver",    "Driver Package Bundle Installation",         500),
                ("recovery",  "Data Recovery & Backup Software Deployment",1500),
                ("backup",    "Data Recovery & Backup Software Deployment",1500),
                ("slow",      "System Optimisation & Cleanup Service",      800),
                ("crash",     "System Stability Diagnostic & Patch",       1000),
                ("blue screen","System Stability Diagnostic & Patch",      1000),
                ("bsod",      "System Stability Diagnostic & Patch",       1000),
                ("startup",   "Boot Configuration Repair",                  700),
                ("boot",      "Boot Configuration Repair",                  700),
                ("network",   "Network & Connectivity Troubleshooting",     600),
                ("internet",  "Network & Connectivity Troubleshooting",     600),
            };

        private const double BaseDiagnosticLabor = 60;

        public List<string> GetRequiredParts(RepairRequest request)
        {
            var parts = new List<string>();
            string desc = (request.ProblemDescription ?? string.Empty).ToLower();
            var addedServices = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in ServiceMap)
            {
                if (desc.Contains(entry.Keyword) && addedServices.Add(entry.ServiceName))
                {
                    parts.Add($"{entry.ServiceName}  (+{entry.Cost:N0} BDT)");
                }
            }

            if (parts.Count == 0)
            {
                parts.Add("General Software Diagnostic & Health Check  (+500 BDT)");
            }
            return parts;
        }

        public double GetTotalEstimate(RepairRequest request, List<string> parts)
        {
            double total = BaseDiagnosticLabor;
            string desc = (request.ProblemDescription ?? string.Empty).ToLower();
            var addedServices = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in ServiceMap)
            {
                if (desc.Contains(entry.Keyword) && addedServices.Add(entry.ServiceName))
                {
                    total += entry.Cost;
                }
            }

            if (addedServices.Count == 0)
            {
                total += 500;
            }
            return total;
        }
    }
}
