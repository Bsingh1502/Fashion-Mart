using FashionMart.Models;

namespace FashionMart.EntityFramework
{
    public interface IOrderRepository
    {
        Task<List<Order>> FetchOrders();
        Task<List<Order>> FetchOrders(string userId);
        Task InsertOrder(Order order);
        Task RemoveOrder(int orderId);
        Task ModifyOrder(Order order);
        Task<Order> FetchOrderId(int orderId);

    }
}
