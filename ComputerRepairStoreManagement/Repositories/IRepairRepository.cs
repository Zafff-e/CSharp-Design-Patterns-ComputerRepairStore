using ComputerRepairStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Repositories
{
    public interface IRepairRepository
    {
        void SaveRequest(RepairRequest request);
        IEnumerable<RepairRequest> GetAllRequestsList();
        RepairRequest GetRequestById(int id);
        void UpdateRequest(RepairRequest request);
        void DeleteRequest(int id);
    }
}
