using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Factory
{
    public class SoftwareRepairFactory : RepairFactory
    {
        public SoftwareRepairFactory(RepairRequest request) : base(request)
        {
            _request.Category = RepairCategory.Software;
        }

        public override IRepairManager Create()
        {
            return new SoftwareRepairManager();
        }
    }
}
