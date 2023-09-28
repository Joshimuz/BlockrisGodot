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

    public static ulong PointsMissed
    {
        get { return (ulong)config.GetValue("Stats", "PointsMissed", 0); }
        set { config.SetValue("Stats", "PointsMissed", value); }
    }

    /// <summary>
    /// The Highest Difficulty the player has achived
    /// To set, use Stats.LatestDifficulty
    /// </summary>
    public static float HighestDifficulty
    {
        get { return (float)config.GetValue("Stats", "HighestDifficulty", 0); }
        private set { config.SetValue("Stats", "HighestDifficulty", value); }
    }

    public static float LatestDifficulty
    {
        get { return (float)config.GetValue("Stats", "LatestDifficulty", 0); }
        set
        {
            if (value > HighestDifficulty) { HighestDifficulty = value; }
            config.SetValue("Stats", "LatestDifficulty", value);
        }
    }

    public static ulong BlocksHit
    {
        get { return (ulong)config.GetValue("Stats", "BlocksHit", 0); }
        set { config.SetValue("Stats", "BlocksHit", value); }
    }

    public static ulong BlocksMissed
    {
        get { return (ulong)config.GetValue("Stats", "BlocksMissed", 0); }
        set { config.SetValue("Stats", "BlocksMissed", value); }
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

    public static double SecondsBoosted
    {
        get { return (double)config.GetValue("Stats", "SecondsBoosted", 0); }
        set { config.SetValue("Stats", "SecondsBoosted", value); }
    }

    public static double SecondsPlayed
    {
        get { return (double)config.GetValue("Stats", "SecondsPlayed", 0); }
        set { config.SetValue("Stats", "SecondsPlayed", value); }
    }

    #region EnemiesSeen
    public static ulong BasicEnemiesSeen
    {
        get { return (ulong)config.GetValue("Stats", "BasicEnemiesSeen", 0); }
        set { config.SetValue("Stats", "BasicEnemiesSeen", value); }
    }

    public static ulong SpikeyEnemiesSeen
    {
        get { return (ulong)config.GetValue("Stats", "SpikeyEnemiesSeen", 0); }
        set { config.SetValue("Stats", "SpikeyEnemiesSeen", value); }
    }

    public static ulong DVDEnemiesSeen
    {
        get { return (ulong)config.GetValue("Stats", "DVDEnemiesSeen", 0); }
        set { config.SetValue("Stats", "DVDEnemiesSeen", value); }
    }

    public static ulong OneUpEnemiesSeen
    {
        get { return (ulong)config.GetValue("Stats", "OneUpEnemiesSeen", 0); }
        set { config.SetValue("Stats", "OneUpEnemiesSeen", value); }
    }
    #endregion

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