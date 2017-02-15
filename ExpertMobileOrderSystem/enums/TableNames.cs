using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpertMobileOrderSystem.enums
{
    public class TableNames
    {
        public const string OrderACT = "[Order.ACT]";
        public const string OrderPGroup = "[Order.PGroup]";
        public const string OrderProduct = "[Order.Product]";
        public const string OrderRate = "[Order.Rate]";
        public const string OrderRate2 = "[Order.Rate2]";
    }

    public enum OperationTypes
    {
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3
    }
}
