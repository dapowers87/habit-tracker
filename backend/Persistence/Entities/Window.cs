using System;

namespace Persistence.Entities
{
    public class Window
    {
        public int WindowId { get; set; }
        public string Username { get; set; }
        public string WindowName { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfCheatDays { get; set; }
        public int NumberOfCheatDaysUsed { get; set; }
    }
}