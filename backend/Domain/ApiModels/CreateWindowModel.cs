using System;

namespace Domain.ApiModels
{
    public class CreateWindowModel
    {
        public string WindowName { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfCheatDays { get; set; }
    }
}