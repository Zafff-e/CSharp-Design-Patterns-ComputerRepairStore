using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Factory;
using ComputerRepairStoreManagement.Manager;
using System.Linq;
using Xunit;

namespace ComputerRepairStoreManagement.Tests
{
    public class FactoryTests
    {
        [Fact]
        public void HardwareRepairFactory_Create_ReturnsHardwareRepairManager()
        {
            var request = new RepairRequest(1, "John Doe", "Desktop", "Screen flickering", RepairCategory.Hardware);
            var factory = new HardwareRepairFactory(request);

            IRepairManager manager = factory.Create();

            Assert.NotNull(manager);
            Assert.IsType<HardwareRepairManager>(manager);
        }

        [Fact]
        public void HardwareRepairFactory_ProcessRepair_CalculatesHardwarePartsAndEstimate()
        {
            // BaseDiagnosticLabor = 100, Screen = 5000, Battery = 2000 => 7100
            var request = new RepairRequest(1, "Alice", "Laptop", "Cracked screen and swollen battery", RepairCategory.Hardware);
            var factory = new HardwareRepairFactory(request);

            RepairRequest processed = factory.ProcessRepair();

            Assert.Equal(RepairCategory.Hardware, processed.Category);
            Assert.Equal(2, processed.RequiredParts.Count);
            Assert.Contains(processed.RequiredParts, p => p.Contains("Replacement Display Panel"));
            Assert.Contains(processed.RequiredParts, p => p.Contains("Battery Pack"));
            Assert.Equal(7100, processed.TotalEstimate);
        }

        [Fact]
        public void HardwareRepairFactory_ProcessRepair_UsesFallbackWhenNoKeywordsMatched()
        {
            // Fallback = 500, BaseDiagnosticLabor = 100 => 600
            var request = new RepairRequest(2, "Bob", "Desktop", "Random clicking noise", RepairCategory.Hardware);
            var factory = new HardwareRepairFactory(request);

            RepairRequest processed = factory.ProcessRepair();

            Assert.Single(processed.RequiredParts);
            Assert.Contains("General Hardware Inspection & Component Check", processed.RequiredParts.First());
            Assert.Equal(600, processed.TotalEstimate);
        }

        [Fact]
        public void SoftwareRepairFactory_Create_ReturnsSoftwareRepairManager()
        {
            var request = new RepairRequest(3, "Carol", "Laptop", "Infected with virus", RepairCategory.Software);
            var factory = new SoftwareRepairFactory(request);

            IRepairManager manager = factory.Create();

            Assert.NotNull(manager);
            Assert.IsType<SoftwareRepairManager>(manager);
        }

        [Fact]
        public void SoftwareRepairFactory_ProcessRepair_CalculatesSoftwareServicesAndEstimate()
        {
            // BaseDiagnosticLabor = 60, Virus = 2000, Reinstall OS = 2000 => 4060
            var request = new RepairRequest(4, "David", "Desktop", "Severe virus and windows reinstall needed", RepairCategory.Software);
            var factory = new SoftwareRepairFactory(request);

            RepairRequest processed = factory.ProcessRepair();

            Assert.Equal(RepairCategory.Software, processed.Category);
            Assert.Equal(2, processed.RequiredParts.Count);
            Assert.Contains(processed.RequiredParts, p => p.Contains("Malware Removal Tool Suite"));
            Assert.Contains(processed.RequiredParts, p => p.Contains("OS License Key Update"));
            Assert.Equal(4060, processed.TotalEstimate);
        }

        [Fact]
        public void SoftwareRepairFactory_ProcessRepair_UsesFallbackWhenNoKeywordsMatched()
        {
            // Fallback = 500, BaseDiagnosticLabor = 60 => 560
            var request = new RepairRequest(5, "Eve", "Laptop", "Strange system behaviour", RepairCategory.Software);
            var factory = new SoftwareRepairFactory(request);

            RepairRequest processed = factory.ProcessRepair();

            Assert.Single(processed.RequiredParts);
            Assert.Contains("General Software Diagnostic & Health Check", processed.RequiredParts.First());
            Assert.Equal(560, processed.TotalEstimate);
        }
    }
}
