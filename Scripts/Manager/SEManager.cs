using UnityEngine;

/// <summary>
/// 効果音を管理するクラス
/// </summary>
public class SEManager : MonoBehaviour
{
    /// <summary>
    /// 射撃音_普通（ピストル等）
    /// </summary>
    public AudioClip SE_ShotNormal;
    /// <summary>
    /// 射撃音_軽（サブマシンガン等）
    /// </summary>
    public AudioClip SE_ShotLight;
    /// <summary>
    /// 射撃音_重（ショットガン等）
    /// </summary>
    public AudioClip SE_ShotHeavy;

    /// <summary>
    /// ボム使用したときのSE
    /// </summary>
    public AudioClip SE_Bomb;

    /// <summary>
    /// ダメージ受けたときのSE
    /// </summary>
    public AudioClip SE_Damage;
    
    /// <summary>
    /// リロードしたときのSE
    /// </summary>
    public AudioClip SE_Reload;

    /// <summary>
    /// ローリングしたときのSE
    /// </summary>
    public AudioClip SE_Rolling;

    /// <summary>
    /// 購入したときのSE
    /// </summary>
    public AudioClip SE_Buy;
    public AudioClip SE_CantBuy;


    /// <summary>
    /// 銃選択画面を開いたときのSE
    /// </summary>
    public AudioClip SE_GunSelectOpen;
    /// <summary>
    /// 銃選択画面を閉じたときのSE
    /// </summary>
    public AudioClip SE_GunSelectClose;
    /// <summary>
    /// 銃選択画面でカーソルを合わせたときのSE
    /// </summary>
    public AudioClip SE_GunSelectCursor;
    /// <summary>
    /// 銃選択画面で決定したときのSE
    /// </summary>
    public AudioClip SE_GunSelectDecide;

    /// <summary>
    /// 看板を開いたときのSE
    /// </summary>
    public AudioClip SE_SignboardOpen;
    /// <summary>
    /// 看板を閉じたときのSE
    /// </summary>
    public AudioClip SE_SignboardClose;

    /// <summary>
    /// ダイヤモンドをゲットしたときのSE
    /// </summary>
    public AudioClip SE_GetDiamond;
    /// <summary>
    /// ゲームクリアメニューを開いたときのSE
    /// </summary>
    public AudioClip SE_GameClearDisplay;

    /// <summary>
    /// ゲームオーバーのメニューを開いたときのSE
    /// </summary>
    public AudioClip SE_GameOverDisplay;
    /// <summary>
    /// ゲームオーバーのメニューでクリックしたときのSE
    /// </summary>
    public AudioClip SE_GameOverClick;

    /// <summary>
    /// 宝箱出現時のSE
    /// </summary>
    public AudioClip SE_ChestAppear;
    /// <summary>
    /// 宝箱を開けたときのSE
    /// </summary>
    public AudioClip SE_ChestOpen;

    /// <summary>
    /// アイテムを取得したときのSE
    /// </summary>
    public AudioClip SE_ItemGet;

    /// <summary>
    /// 敵を倒して部屋が開いたときのSE
    /// </summary>
    public AudioClip SE_RoomOpen;

    /// <summary>
    /// 階段でシーン遷移するときのSE
    /// </summary>
    public AudioClip SE_Stairs;

    /// <summary>
    /// 爆発音のSE
    /// </summary>
    public AudioClip SE_Explosion;

    /// <summary>
    /// 敵が攻撃したときのSE
    /// </summary>
    public AudioClip SE_EnemyShot;
    /// <summary>
    /// 敵を倒したときのSE
    /// </summary>
    public AudioClip SE_EnemyDown;

    /// <summary>
    /// ボス戦開始時の演出用のSE（シャキーン）
    /// </summary>
    public AudioClip SE_BossStaging;
    /// <summary>
    /// スカルキングが攻撃したときのSE1（着火）
    /// </summary>
    public AudioClip SE_BossSkullAttack1;
    /// <summary>
    /// スカルキングが攻撃したときのSE2(攻撃）
    /// </summary>
    public AudioClip SE_BossSkullAttack2;
    /// <summary>
    /// メガントロックが攻撃したときのSE
    /// </summary>
    public AudioClip SE_BossRock;
    /// <summary>
    /// カースバニーの演出用のSE
    /// </summary>
    public AudioClip SE_BossBunny;
}
