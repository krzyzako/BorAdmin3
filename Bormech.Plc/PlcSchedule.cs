namespace Bormech.Plc;

public class PlcSchedule
{
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
    public bool Saturday { get; set; }
    public bool Sunday { get; set; }

    public PlcSchedule(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
    {
        Monday = monday;
        Tuesday = tuesday;
        Wednesday = wednesday;
        Thursday = thursday;
        Friday = friday;
        Saturday = saturday;
        Sunday = sunday;
    }

    public PlcSchedule()
    {
        Monday = false;
        Tuesday = false;
        Wednesday = false;
        Thursday = false;
        Friday = false;
        Saturday = false;
        Sunday = false;
    }

    public PlcSchedule(List<IPlcShedule> schedule)
    {
        Monday = schedule[0].IsActive;
        Tuesday = schedule[1].IsActive;
        Wednesday = schedule[2].IsActive;
        Thursday = schedule[3].IsActive;
        Friday = schedule[4].IsActive;
        Saturday = schedule[5].IsActive;
        Sunday = schedule[6].IsActive;
    }
    public interface IPlcShedule
    {
        public string Day { get; set; }
        public bool IsActive { get; set; }
    }
    
}