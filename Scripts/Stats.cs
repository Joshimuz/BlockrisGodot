using Godot;
using System;

public static class Stats
{
    static ConfigFile config;

    /// <summary>
    /// The Highest score the player has achived
    /// To set, use Stats.LatestScore
    /// </summary>
    public static ulong HighScore 
    { 
        get { return (ulong)config.GetValue("Stats", "HighScore", 0); }
        private set { config.SetValue("Stats", "HighScore", value); }
    }

    public static ulong LatestScore
    {
        get { return (ulong)config.GetValue("Stats", "LatestScore", 0); }
        set 
        { 
            if (value > HighScore) { HighScore = value; }
            config.SetValue("Stats", "LatestScore", value); 
        }
    }

    public static ulong TimesGoneLeft
    { 
        get { return (ulong)config.GetValue("Stats", "TimesGoneLeft", 0); }
        set { config.SetValue("Stats", "TimesGoneLeft", value); }
    }

    public static ulong TimesGoneRight
    {
        get { return (ulong)config.GetValue("Stats", "TimesGoneRight", 0); }
        set { config.SetValue("Stats", "TimesGoneRight", value); }
    }

    // The first time Stats gets used, this gets called
    static Stats()
    {
        config = new ConfigFile();

        Error err = config.Load("user://stats.cfg");

        if (err != Error.Ok)
        {
            config.Save("user://stats.cfg");
        }
    }

    public static void CommitToDisk()
    {
        config.Save("user://stats.cfg");
    }
}
