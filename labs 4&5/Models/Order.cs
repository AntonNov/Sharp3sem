namespace Models
{
    public class Order
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
    }
}
