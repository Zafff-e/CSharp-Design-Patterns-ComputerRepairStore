using ComputerRepairStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Manager
{
    public interface IRepairManager
    {
        List<string> GetRequiredParts(RepairRequest request);
        double GetTotalEstimate(RepairRequest request, List<string> parts);
    }
}
