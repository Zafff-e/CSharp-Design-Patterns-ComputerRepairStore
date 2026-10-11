using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Factory
{
    public abstract class RepairFactory
    {
        protected RepairRequest _request;

        protected RepairFactory(RepairRequest request)
        {
            _request = request;
        }

        public abstract IRepairManager Create();

        public RepairRequest ProcessRepair()
        {
            IRepairManager manager = Create();
            _request.RequiredParts = manager.GetRequiredParts(_request);
            _request.TotalEstimate = manager.GetTotalEstimate(_request, _request.RequiredParts);
            return _request;
        }
    }
}
