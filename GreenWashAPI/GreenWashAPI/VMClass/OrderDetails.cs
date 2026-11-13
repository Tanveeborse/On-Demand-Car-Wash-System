namespace GreenWashAPI.VMClass
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public int? WasherId { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public string PackageName { get; set; }
        public List<string> AddOns { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
    }
}
