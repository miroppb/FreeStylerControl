﻿public enum ScheduleActions { TurnAllOff, TurnAllOn, SundayLights, ToggleAll };

internal class ClSchedule
{
    public int id { get; set; }
    public DayOfWeek day { get; set; }
    public DateTime time { get; set; }
    public ScheduleActions action { get; set; }
}