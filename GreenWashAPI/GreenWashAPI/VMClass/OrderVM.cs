namespace GreenWashAPI.VMClass
{
    public class OrderVM
    {
        public int CarId { get; set; }

        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int? WasherId { get; set; }

        public int? PackageId { get; set; }

        public string Status { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime? ScheduledDatetime { get; set; }

        public DateTime? ActualStartTime { get; set; }

        public DateTime? ActualEndTime { get; set; }

        public decimal TotalAmount { get; set; }
    }

    // Request DTO for rescheduling an order
    public class RescheduleOrderRequest
    {
        public DateTime NewSchedule { get; set; }
        public string NewLocation { get; set; }
    }
    // Request DTO for updating payment status
    public class PaymentStatusRequest
    {
        public string PaymentStatus { get; set; }
    }
    // Request DTO for admin report filtering
    public class AdminReportRequest
    {
        public string OrderNumber { get; set; }
        public string WasherName { get; set; }
        public string Type { get; set; }
        public string Service { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
