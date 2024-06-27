using System;
using System.Collections.Generic;
public class Coupon
{
    public int CouponID { get; set; }
    public string CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool IsPercentage { get; set; }
    public bool IsActive { get; set; }
}
