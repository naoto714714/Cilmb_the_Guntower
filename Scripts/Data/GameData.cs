/// <summary>
/// シーン間のゲームデータを引き継ぐため、一時的にデータを保存しておくクラス
/// </summary>
public static class GameData
{
    public static int playerHP = 10;
    public static int holdCoin = 0;
    public static int killCount = 0;
    public static int hour = 0;
    public static int minite = 0;
    public static int seconds = 0;

    public static int[] equipGunNumbers = new int[2] { 1, 2 }; //手持ちの武器は2個、初期武器は1と2（ハンドガンとソードオフショットガン）
    public static int[] holdGunNumbers = new int[20];//控えの武器はとりあえず20個まで

    public static int[] equipGunRemainBullets = new int[2] { 100000, 150 };//手持ちの武器の残弾、初期の残弾は10万と150（ハンドガンとソードオフショットガン）
    public static int[] holdGunRemainBullets = new int[20]; //控えの武器の残弾
}
