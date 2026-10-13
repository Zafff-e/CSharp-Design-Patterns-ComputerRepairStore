using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Services;
using Xunit;

namespace ComputerRepairStoreManagement.Tests
{
    public class PricingCalculatorTests
    {
        [Theory]
        [InlineData("Laptop Dell XPS", 1000, 100)]
        [InlineData("Apple MacBook Pro", 5000, 500)]
        [InlineData("Macbook Air M2", 2500, 250)]
        public void CalculateHolidayDiscount_ShouldApply10Percent_WhenDeviceIsLaptopOrMacbook(string device, double total, double expectedDiscount)
        {
            var request = new RepairRequest(1, "Zafir", device, "Screen broken", RepairCategory.Hardware)
            {
                TotalEstimate = total
            };

            double discount = PricingCalculator.CalculateHolidayDiscount(request);

            Assert.Equal(expectedDiscount, discount);
        }

        [Fact]
        public void CalculateHolidayDiscount_ShouldReturnZero_ForDesktopOrTablet()
        {
            var request = new RepairRequest(2, "Test User", "Desktop Custom Rig", "RAM upgrade", RepairCategory.Hardware)
            {
                TotalEstimate = 3000
            };

            double discount = PricingCalculator.CalculateHolidayDiscount(request);

            Assert.Equal(0, discount);
        }

        [Fact]
        public void CalculateHolidayDiscount_ShouldReturnZero_WhenRequestIsNull()
        {
            double discount = PricingCalculator.CalculateHolidayDiscount(null);
            Assert.Equal(0, discount);
        }

        [Theory]
        [InlineData(1000, 50)]
        [InlineData(2000, 100)]
        [InlineData(4500, 225)]
        [InlineData(0, 0)]
        [InlineData(-100, 0)]
        public void CalculateVat_ShouldApply5Percent_OnTaxableAmount(double taxable, double expectedVat)
        {
            double vat = PricingCalculator.CalculateVat(taxable);
            Assert.Equal(expectedVat, vat);
        }

        [Fact]
        public void CalculateFinalTotal_ShouldDeductDiscountAndAddVat()
        {
            // Subtotal = 5000, Discount = 500 (10%) => Taxable = 4500 => VAT = 225 (5%) => Final = 4725
            double finalTotal = PricingCalculator.CalculateFinalTotal(5000, 500);
            Assert.Equal(4725, finalTotal);
        }

        [Fact]
        public void CalculateBalanceDue_ShouldSubtractAdvancePaidFromTotalEstimate()
        {
            double balance = PricingCalculator.CalculateBalanceDue(4725, 2000);
            Assert.Equal(2725, balance);
        }
    }
}
