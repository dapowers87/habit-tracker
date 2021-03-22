using System;

namespace Domain.ApiModels
{
    public class CreateWindowModel
    {
        public DateTime StartDate { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfCheatDays { get; set; }
    }
}