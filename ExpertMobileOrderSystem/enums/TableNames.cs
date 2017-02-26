using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpertMobileOrderSystem
{
    public class TableNames
    {
        public const string OrderPGroup = "Order.PGroup";
        public const string OrderGroup = "Order.Group";
        public const string OrderProduct = "Order.Product";
        public const string OrderACT = "Order.ACT";        
        public const string OrderOrder = "Order.Order";
        public const string OrderOrder2 = "Order.Order2";        
        public const string OrderRate = "Order.Rate";
        public const string OrderRate2 = "Order.Rate2";
    }

    public enum OperationTypes
    {
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3
    }
}
