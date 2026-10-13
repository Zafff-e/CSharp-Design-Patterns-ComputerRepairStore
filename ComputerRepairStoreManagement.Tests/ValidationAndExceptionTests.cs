using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Exceptions;
using Xunit;

namespace ComputerRepairStoreManagement.Tests
{
    public class ValidationAndExceptionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_ThrowsInvalidRepairRequestException_WhenCustomerNameIsBlank(string? invalidName)
        {
            var request = new RepairRequest(1, invalidName, "Laptop", "Screen damaged", RepairCategory.Hardware);

            var ex = Assert.Throws<InvalidRepairRequestException>(() => request.Validate());
            Assert.Contains("Customer name cannot be empty", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_ThrowsInvalidRepairRequestException_WhenDeviceDetailsIsBlank(string? invalidDevice)
        {
            var request = new RepairRequest(1, "Karim Rahman", invalidDevice, "Screen damaged", RepairCategory.Hardware);

            var ex = Assert.Throws<InvalidRepairRequestException>(() => request.Validate());
            Assert.Contains("Device details cannot be empty", ex.Message);
        }

        [Fact]
        public void Validate_ThrowsInvalidRepairRequestException_WhenAdvancePaidIsNegative()
        {
            var request = new RepairRequest(1, "Karim Rahman", "Laptop", "Keyboard broken", RepairCategory.Hardware)
            {
                AdvancePaid = -500
            };

            var ex = Assert.Throws<InvalidRepairRequestException>(() => request.Validate());
            Assert.Contains("Advance paid cannot be negative", ex.Message);
        }

        [Fact]
        public void Validate_Succeeds_WhenAllFieldsAreValid()
        {
            var request = new RepairRequest(1, "Karim Rahman", "Laptop", "Keyboard broken", RepairCategory.Hardware)
            {
                AdvancePaid = 500
            };

            // Should not throw
            request.Validate();
        }

        [Fact]
        public void RepairEnums_Values_MapCorrectlyToOneBasedChoices()
        {
            Assert.Equal(1, (int)RepairCategory.Hardware);
            Assert.Equal(2, (int)RepairCategory.Software);

            Assert.Equal(1, (int)RepairStatus.Pending);
            Assert.Equal(2, (int)RepairStatus.InProgress);
            Assert.Equal(3, (int)RepairStatus.Completed);
        }
    }
}
