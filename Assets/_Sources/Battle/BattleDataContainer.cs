
public static class BattleDataContainer
{
    public static bool IsCompanyBattle { get; private set; }
    public static bool IsArenaBattle { get; private set; }
    public static Fraction CurentPlayerFraction { get; private set; }
    public static string EnemyName = "Enemy";

    public static int CompanyLvl;
    public static string ArenaEnemyId;
    public static int ArenaEnemyRating;
    public static int PlayerRating;
    public static bool IsLvlUp;
    public static int timeScale = 3;
    public static string playerId;

    public static void CompanyBattleData(Fraction fraction, int lvl)
    {
        IsCompanyBattle = true;
        IsArenaBattle = false;
        CurentPlayerFraction = fraction;
        CompanyLvl = lvl;
    }

    public static void ArenaBattle(string enemyId, int enemyRating, int playerRating, string enemyName)
    {
        EnemyName = enemyName;
        ArenaEnemyId = enemyId;
        ArenaEnemyRating = enemyRating;
        IsCompanyBattle = false;
        IsArenaBattle = true;
        PlayerRating = playerRating;
    }
}
