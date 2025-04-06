﻿namespace ShoppingApp2.Business.Operations.Order.Dtos
{
    public class AddOrderDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}


