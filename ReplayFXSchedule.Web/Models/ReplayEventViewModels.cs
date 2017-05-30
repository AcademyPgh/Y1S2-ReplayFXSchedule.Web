//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace ReplayFXSchedule.Web.Models
//{
//    public class ReplayEventViewModels
//    {
//        public int Id { get; set; }
//        public string Title { get; set; }
//        [DataType(DataType.Date)]
//        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
//        [Display(Name = "Date")]
//        public DateTime Date { get; set; }
//        [DataType(DataType.Time)]
//        [Display(Name = "Start Time")]
//        public string StartTime { get; set; }
//        [DataType(DataType.Time)]
//        [Display(Name = "End Time")]
//        public string EndTime { get; set; }
//        public string Description { get; set; }
//        [Display(Name = "Extended Description")]
//        public string ExtendedDescription { get; set; }
//        public string Location { get; set; }
//        public string Image { get; set; }

//        [Display(Name = "Event Type")]
//        public virtual List<ReplayEventType> ReplayEventTypes { get; set; }
//    }
//}