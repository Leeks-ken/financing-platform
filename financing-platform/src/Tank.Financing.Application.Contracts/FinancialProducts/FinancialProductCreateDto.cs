using Tank.Financing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tank.Financing.FinancialProducts
{
    public class FinancialProductCreateDto
    {
        public TJDistrict AvailableDistricts { get; set; } = ((TJDistrict[])Enum.GetValues(typeof(TJDistrict)))[0];
        [Required]
        [Range(FinancialProductConsts.TimeLimitMinLength, FinancialProductConsts.TimeLimitMaxLength)]
        public int TimeLimit { get; set; }
        [Required]
        public GuaranteeMethod GuaranteeMethod { get; set; } = ((GuaranteeMethod[])Enum.GetValues(typeof(GuaranteeMethod)))[0];
        [Required]
        public decimal CreditCeiling { get; set; }
        [Required]
        [StringLength(FinancialProductConsts.OrganizationMaxLength, MinimumLength = FinancialProductConsts.OrganizationMinLength)]
        public string Organization { get; set; }
        public int? AppliedNumber { get; set; }
        [Required]
        public decimal APR { get; set; }
        public int? Rating { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid? FinancialProductId { get; set; }
    }
}