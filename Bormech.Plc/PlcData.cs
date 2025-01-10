namespace Bormech.Plc
{

    public class OutGoPlc
    {
        public short Wanna1Temperature { get; set; }
        public short Wanna2Temperature { get; set; }
        public Status Status { get; set; }
    }

    public struct Status
    {
        public Inverter Inverter { get; set; }
        public Operation Operation { get; set; }
        public short Wanna1SetHmi { get; set; }
        public short Wanna2SetHmi { get; set; }
    }

    public struct Operation
    {
        public ManualHand ManualHand { get; set; }
        public WannaBase Wanna1 { get; set; }
        public WannaBase Wanna2 { get; set; }
        public Suszarka Suszarka { get; set; }
        public bool StartPump { get; set; }
        public byte TempFreeze { get; set; }
        public short TempFreeze1 { get; set; }
    }

    public class Suszarka
    {
        public bool SetPompa { get; set; }
        public bool Grzałka { get; set; }
    }

    public class WannaBase
    {
        public short TempSet { get; set; }
        public short Histereza { get; set; }
        public bool SetPompa { get; set; }
    }

    public class ManualHand
    {
        public bool SetSuszarka  { get; set; }
        public bool SetPompa1 { get; set; }
        public bool SetPompa2 { get; set; }
        public bool SetManual { get; set; }
        public bool SetSuszarka1 { get; set; }


    }

    public struct Inverter
    {
        public short Speed {get; set;}
        public short PowerStageSupplyDisabled {get; set;}

    }

    public class PlcData
    {
        public float TemperatureC { get; set; }
        public float TemperatureCWanna1 { get; set; }


        public PlcData()
        {

        }

        public class DayOfWeekItem
        {

            public string Name { get; set; } = string.Empty;
            public bool IsSelected { get; set; }
        }
    }
}