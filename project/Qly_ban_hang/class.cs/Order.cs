using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


public class Order
{
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public int ProductID { get; set; }
    public string CouponCode { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; }
    public int TotalAmount { get; set; }
}
