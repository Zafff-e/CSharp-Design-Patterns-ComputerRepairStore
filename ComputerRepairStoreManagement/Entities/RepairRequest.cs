using ComputerRepairStoreManagement.Exceptions;
using System;
using System.Collections.Generic;

namespace ComputerRepairStoreManagement.Entities
{
    public class RepairRequest
    {
        public RepairRequest()
        {
            RequiredParts = new List<string>();
        }

        public RepairRequest(
            int requestId,
            string customerName,
            string deviceDetails,
            string problemDescription,
            RepairCategory category)
        {
            RequestId = requestId;
            CustomerName = customerName;
            DeviceDetails = deviceDetails;
            ProblemDescription = problemDescription;
            Category = category;
            Status = RepairStatus.Pending;
            RequiredParts = new List<string>();
            TotalEstimate = 0;
            AdvancePaid = 0;
            WarrantyPeriodMonths = 0;
        }

        public int RequestId { get; set; }
        public string CustomerName { get; set; }
        public string DeviceDetails { get; set; }
        public string ProblemDescription { get; set; }
        public RepairCategory Category { get; set; }
        public RepairStatus Status { get; set; }
        public List<string> RequiredParts { get; set; }
        public double TotalEstimate { get; set; }
        public double AdvancePaid { get; set; }
        public int WarrantyPeriodMonths { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
                throw new InvalidRepairRequestException("Customer name cannot be empty.");
            if (string.IsNullOrWhiteSpace(DeviceDetails))
                throw new InvalidRepairRequestException("Device details cannot be empty.");
            if (AdvancePaid < 0)
                throw new InvalidRepairRequestException("Advance paid cannot be negative.");
        }
    }
}
