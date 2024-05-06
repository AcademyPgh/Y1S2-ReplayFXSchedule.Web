using System;

namespace ReplayFXSchedule.Web.Models
{
    public class PhotoProcessQueue
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string URL { get; set; }
        public PhotoProcessQueueStatus Status { get; set; }
        public string Error { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Processed { get; set; }
    }

    public enum PhotoProcessQueueStatus
    {
        New,
        Processing,
        Processed,
        Error
    }
}