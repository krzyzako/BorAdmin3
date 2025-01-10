namespace Bormech.Plc;

public class Schedule
{
    // Klasa do reprezentowania pojedynczego dnia
    public class DaySchedule 
    {
        public string Day { get; set; }
        public bool IsActive { get; set; }
    }

    /* Lista dni tygodnia, każdy z flagą aktywności,
       gdzie wartość true oznacza, że dany dzień jest w harmonogramie */
    public List<DaySchedule> Days { get; set; }

    public Schedule()
    {
        Days = new List<DaySchedule>
        {
            new DaySchedule { Day = "Poniedziałek", IsActive = false },
            new DaySchedule { Day = "Wtorek", IsActive = false },
            new DaySchedule { Day = "Środa", IsActive = false },
            new DaySchedule { Day = "Czwartek", IsActive = false },
            new DaySchedule { Day = "Piątek", IsActive = false },
            new DaySchedule { Day = "Sobota", IsActive = false },
            new DaySchedule { Day = "Niedziela", IsActive = false },
        };
    }

    public Schedule(PlcSchedule days)
    {
        Days = new List<DaySchedule>
        {
            new DaySchedule { Day = "Poniedziałek", IsActive = days.Monday },
            new DaySchedule { Day = "Wtorek", IsActive = days.Tuesday },
            new DaySchedule { Day = "Środa", IsActive = days.Wednesday },
            new DaySchedule { Day = "Czwartek", IsActive = days.Thursday },
            new DaySchedule { Day = "Piątek", IsActive = days.Friday },
            new DaySchedule { Day = "Sobota", IsActive = days.Saturday },
            new DaySchedule { Day = "Niedziela", IsActive = false },
        };
    }

    // Ustaw dany dzień jako aktywny lub nieaktywny
    public void SetDay(string day, bool isActive)
    {
        var daySchedule = Days.Find(d => d.Day == day);
        if(daySchedule != null) daySchedule.IsActive = isActive;
    }

    // Pobierz informacje o aktywności danego dnia
    public bool IsDayActive(string day)
    {
        var daySchedule = Days.Find(d => d.Day == day);
        return daySchedule?.IsActive ?? false;
    }
}