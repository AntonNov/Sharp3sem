using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderInfo>> GetOrdersInfo();
    }
}