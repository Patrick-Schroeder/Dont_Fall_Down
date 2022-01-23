public static class LevelNames
{
    public const string Intro = "Intro";
    public const string Tutorial = "Tutorial";
    public const string Level1 = "Level 1";
    public const string Level2 = "Level 2";

    public static string GetNextLevelName(string levelName)
    {
        switch (levelName)
        {
            case Tutorial:
                return Level1;
            case Level1:
                return Level2;
            default:
                return Intro;
        }
    }
}
