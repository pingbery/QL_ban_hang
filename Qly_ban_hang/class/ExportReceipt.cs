using System;
using System.Collections.Generic;

public class ExportReceipt
{
    public int ExportReceiptID { get; set; }
    public int ProductID { get; set; }
    public DateTime ExportDate { get; set; }
    public string ShippingAddress { get; set; }
    public int TotalAmount { get; set; }
}