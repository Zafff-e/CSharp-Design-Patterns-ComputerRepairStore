using ComputerRepairStoreManagement.Entities;
using System;

namespace ComputerRepairStoreManagement.Services
{
    public static class PricingCalculator
    {
        public const double VatRate = 0.05;
        public const double PromotionalDiscountRate = 0.10;

        public static double CalculateHolidayDiscount(RepairRequest request)
        {
            if (request == null) return 0;
            string combined = ((request.ProblemDescription ?? "") + " " + (request.DeviceDetails ?? "")).ToLower();
            return (combined.Contains("laptop") || combined.Contains("macbook"))
                ? Math.Round(request.TotalEstimate * PromotionalDiscountRate, 2)
                : 0;
        }

        public static double CalculateVat(double taxableAmount)
        {
            if (taxableAmount <= 0) return 0;
            return Math.Round(taxableAmount * VatRate, 2);
        }

        public static double CalculateFinalTotal(double subtotal, double discount)
        {
            double taxable = Math.Max(0, subtotal - discount);
            double vat = CalculateVat(taxable);
            return Math.Round(taxable + vat, 2);
        }

        public static double CalculateBalanceDue(double totalEstimate, double advancePaid)
        {
            return Math.Round(totalEstimate - advancePaid, 2);
        }
    }
}
