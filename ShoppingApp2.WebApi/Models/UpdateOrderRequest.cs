﻿namespace ShoppingApp2.WebApi.Models
{
    public class UpdateOrderRequest
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
