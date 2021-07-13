using System;

namespace Domain.ApiModels
{
    public class WindowWithoutUser
    {
        public int WindowId { get; set; }
        public string WindowName { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfCheatDays { get; set; }
        public int NumberOfCheatDaysUsed { get; set; }
    }
}