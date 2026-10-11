using ComputerRepairStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement.Repositories
{
    public class RepairRepository : IRepairRepository
    {
        private readonly List<RepairRequest> _db;

        public RepairRepository()
        {
            _db = new List<RepairRequest>()
            {
                new RepairRequest()
                {
                    RequestId            = 1,
                    CustomerName         = "Tanvir Hossain",
                    DeviceDetails        = "Laptop , Windows 11",
                    ProblemDescription   = "Laptop screen is cracked and battery drains fast",
                    Category             = RepairCategory.Hardware,
                    Status               = RepairStatus.InProgress,
                    RequiredParts        = new List<string> { "Replacement Display Panel", "Battery Pack" },
                    TotalEstimate        = 7665,
                    AdvancePaid          = 2000,
                    WarrantyPeriodMonths = 3
                },
                new RepairRequest()
                {
                    RequestId            = 2,
                    CustomerName         = "Nusrat Jahan",
                    DeviceDetails        = "Desktop , Windows 10",
                    ProblemDescription   = "System infected with virus, OS needs reinstall",
                    Category             = RepairCategory.Software,
                    Status               = RepairStatus.Pending,
                    RequiredParts        = new List<string> { "Malware Removal Tool Suite Deployment", "OS License Key Update" },
                    TotalEstimate        = 4263,
                    AdvancePaid          = 0,
                    WarrantyPeriodMonths = 1
                },
                new RepairRequest()
                {
                    RequestId            = 3,
                    CustomerName         = "Abdur Rahman",
                    DeviceDetails        = "Laptop , Windows 11",
                    ProblemDescription   = "Overheating issue and loud fan noise during gaming",
                    Category             = RepairCategory.Hardware,
                    Status               = RepairStatus.Completed,
                    RequiredParts        = new List<string> { "Cooling Fan Assembly", "Thermal Paste Application" },
                    TotalEstimate        = 4830,
                    AdvancePaid          = 4830,
                    WarrantyPeriodMonths = 3
                }
            };
        }

        public void SaveRequest(RepairRequest request)
        {
            request.RequestId = _db.Count > 0 ? _db.Max(r => r.RequestId) + 1 : 1;
            _db.Add(request);
        }

        public IEnumerable<RepairRequest> GetAllRequestsList()
        {
            return _db;
        }

        public RepairRequest GetRequestById(int id)
        {
            return _db.FirstOrDefault(r => r.RequestId == id);
        }

        public void UpdateRequest(RepairRequest updated)
        {
            RepairRequest existing = GetRequestById(updated.RequestId);
            if (existing != null)
            {
                existing.CustomerName = updated.CustomerName;
                existing.DeviceDetails = updated.DeviceDetails;
                existing.ProblemDescription = updated.ProblemDescription;
                existing.Category = updated.Category;
                existing.Status = updated.Status;
                existing.RequiredParts = updated.RequiredParts;
                existing.TotalEstimate = updated.TotalEstimate;
                existing.AdvancePaid = updated.AdvancePaid;
                existing.WarrantyPeriodMonths = updated.WarrantyPeriodMonths;
            }
        }

        public void DeleteRequest(int id)
        {
            RepairRequest request = GetRequestById(id);
            if (request != null)
            {
                _db.Remove(request);
            }
        }
    }
}
