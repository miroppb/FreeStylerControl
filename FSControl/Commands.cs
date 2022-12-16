namespace FSControl
{
    public static class Commands
    {
        static public readonly string SELECTALL = "FSOC000255";
        static public readonly string SELECTALL2 = "FSOC000000";
        static public readonly string POWERON_INTENSITY = "FSOC138255";
        static public readonly string POWEROFF_INTENSITY = "FSOC138000";
        static public readonly string SELECTGROUP1 = "FSOC034255";
        static public readonly string SELECTGROUP2 = "FSOC035255";
        static public readonly string SELECTGROUP3 = "FSOC036255";
        static public readonly string LIGHTSWHITE = "FSOC583255";
        static public readonly string LIGHTSBLUE = "FSOC132220";
        static public readonly string LIGHTSRED = "FSOC130220";
        static public readonly string LIGHTSREDOFF = "FSOC130000";
        static public readonly string LIGHTSBLUEOFF = "FSOC132000";

        static public readonly string GROUPSTATUS = "FSBC023000";

        static public readonly string[] LIGHTS_BLUE_PURPLE = { "FSOC130165", "FSOC131135", "FSOC132255" };
        static public readonly string[] LIGHTS_BLUE_COMBO = { "FSOC130013", "FSOC131034", "FSOC132255" };
        static public readonly string[] LIGHTS_LIGHT_BLUE = { "FSOC130021", "FSOC131243", "FSOC132255" };
        static public readonly string[] LIGHTS_GREENBLUE = { "FSOC130127", "FSOC131254", "FSOC132254" };
        static public readonly string[] LIGHTS_HARVEST_YELLOW = { "FSOC130254", "FSOC131243", "FSOC132021" };
    }
}