using ComputerRepairStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Manager
{
    public class HardwareRepairManager : IRepairManager
    {
        private static readonly List<(string Keyword, string PartName, double Cost)> ComponentMap =
            new List<(string, string, double)>
            {
                ("screen",      "Replacement Display Panel",        5000),
                ("display",     "Replacement Display Panel",        5000),
                ("battery",     "Battery Pack",                     2000),
                ("keyboard",    "Keyboard Replacement Unit",        1500),
                ("motherboard", "Motherboard",                      8000),
                ("ram",         "RAM Module",                       3000),
                ("memory",      "RAM Module",                       3000),
                ("hard disk",   "HDD Storage Drive",                4000),
                ("hdd",         "HDD Storage Drive",                4000),
                ("ssd",         "SSD Storage Drive",                5500),
                ("fan",         "Cooling Fan Assembly",              800),
                ("charger",     "Power Adapter / Charger",          1200),
                ("adapter",     "Power Adapter / Charger",          1200),
                ("speaker",     "Speaker Unit",                     1000),
                ("wifi",        "Wi-Fi Network Card",               1800),
            };

        private const double BaseDiagnosticLabor = 100;

        public List<string> GetRequiredParts(RepairRequest request)
        {
            var parts = new List<string>();
            string desc = (request.ProblemDescription ?? string.Empty).ToLower();
            var addedParts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in ComponentMap)
            {
                if (desc.Contains(entry.Keyword) && addedParts.Add(entry.PartName))
                {
                    parts.Add($"{entry.PartName}  (+{entry.Cost:N0} BDT)");
                }
            }

            if (parts.Count == 0)
            {
                parts.Add("General Hardware Inspection & Component Check  (+500 BDT)");
            }
            return parts;
        }

        public double GetTotalEstimate(RepairRequest request, List<string> parts)
        {
            double total = BaseDiagnosticLabor;
            string desc = (request.ProblemDescription ?? string.Empty).ToLower();
            var addedParts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in ComponentMap)
            {
                if (desc.Contains(entry.Keyword) && addedParts.Add(entry.PartName))
                {
                    total += entry.Cost;
                }
            }

            if (addedParts.Count == 0)
            {
                total += 500;
            }
            return total;
        }
    }
}
