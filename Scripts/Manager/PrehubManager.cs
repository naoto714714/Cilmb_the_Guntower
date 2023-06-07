using UnityEngine;

/// <summary>
/// プレハブを管理するクラス
/// </summary>
public class PrehubManager : MonoBehaviour
{
    /// <summary>
    /// 爆発
    /// </summary>
    public GameObject explosion;
    /// <summary>
    /// プレイヤーの弾（当たり判定は非連続）
    /// </summary>
    public GameObject playerBulletPrehub;
    /// <summary>
    /// プレイヤーの弾（当たり判定は連続）
    /// </summary>
    public GameObject playerConsecutiveBulletPrehub;
    /// <summary>
    /// 敵の弾
    /// </summary>
    public GameObject enemyBulletPrehub;
    /// <summary>
    /// ハートのUI
    /// </summary>
    public GameObject heartUIPrehub;
    /// <summary>
    /// ボムのUI
    /// </summary>
    public GameObject bombUIPrehub;

    //ーーーーーーーーーー宝箱のプレハブーーーーーーーーーー
    /// <summary>
    /// 木の宝箱（アイテム１つ）
    /// </summary>
    public GameObject Chest1_Wood_ItemSingle;
    /// <summary>
    /// 木の宝箱（アイテム２つ）
    /// </summary>
    public GameObject Chest2_Wood_ItemDouble;
    /// <summary>
    /// 青い宝箱（Bランクの武器）
    /// </summary>
    public GameObject Chest3_Blue;
    /// <summary>
    /// 赤い宝箱（Aランクの武器）
    /// </summary>
    public GameObject Chest4_Red;
    /// <summary>
    /// 黒い宝箱（Sランクの武器）
    /// </summary>
    public GameObject Chest5_Black;
    /// <summary>
    /// 宝箱が出現したときの煙
    /// </summary>
    public GameObject Chest_Smoke;

    //ーーーーーーーーーー宝箱から出現するアイテムのプレハブーーーーーーーーーー
    /// <summary>
    /// ハート　HPが２回復する
    /// </summary>
    public GameObject Item1_Heart_Full;
    /// <summary>
    /// ハート　HPが１回復する
    /// </summary>
    public GameObject Item2_Heart_Half;
    /// <summary>
    /// ボム　所持ボムを１つ増やす
    /// </summary>
    public GameObject Item3_Bomb;
    /// <summary>
    /// 弾薬箱　所持している全ての武器の残弾を少し回復する
    /// </summary>
    public GameObject Item4_BulletBox_Red;
    /// <summary>
    /// 弾薬箱　装備している武器の残弾を少し回復する
    /// </summary>
    public GameObject Item5_BulletBox_Green;
    /// <summary>
    /// 弾薬箱　装備している武器の残弾を完全に回復する
    /// </summary>
    public GameObject Item6_BulletBox_Blue;
    /// <summary>
    /// コイン　コインを少し獲得
    /// </summary>
    public GameObject Item7_Coin_Bronze;
    /// <summary>
    /// コイン　コインをまあまあ獲得
    /// </summary>
    public GameObject Item8_Coin_Silver;
    /// <summary>
    /// コイン　コインを多く獲得
    /// </summary>
    public GameObject Item9_Coin_Gold;
    /// <summary>
    /// ダイヤモンド　獲得するとゲームクリア
    /// </summary>
    public GameObject Item10_Diamond;

    //ーーーーーーーーーーショップのアイテムのプレハブーーーーーーーーーー
    /// <summary>
    /// ハート_フル
    /// </summary>
    public GameObject ShopItem1_Heart_Full;
    /// <summary>
    /// ハート_ハーフ
    /// </summary>
    public GameObject ShopItem2_Heart_Half;
    /// <summary>
    /// ボム
    /// </summary>
    public GameObject ShopItem3_Bomb;
    /// <summary>
    /// 弾薬箱_赤
    /// </summary>
    public GameObject ShopItem4_BulletBox_Red;
    /// <summary>
    /// 弾薬箱_緑
    /// </summary>
    public GameObject ShopItem5_BulletBox_Green;
    /// <summary>
    /// 弾薬箱_青
    /// </summary>
    public GameObject ShopItem6_BulletBox_Blue;

    //ーーーーーーーーーー武器のプレハブーーーーーーーーーー
    //※※※ランクB※※※
    public GameObject Gun_RankB_1_HandGun;
    public GameObject Gun_RankB_2_SawedOffShotGun;
    public GameObject Gun_RankB_3_ThroughGun;
    public GameObject Gun_RankB_4_BoundGun;
    public GameObject Gun_RankB_5_MiniMachineGun;
    public GameObject Gun_RankB_6_AssaultRifle;
    public GameObject Gun_RankB_7_FullAutoPistol;
    public GameObject Gun_RankB_8_Pistol;
    public GameObject Gun_RankB_9_CompactRifle;
    public GameObject Gun_RankB_10_HeavyRifle;
    public GameObject Gun_RankB_11_TwinRifle;
    public GameObject Gun_RankB_12_LightMachineGun;
    public GameObject Gun_RankB_13_CompactSMG;

    //※※※ランクA※※※
    public GameObject Gun_RankA_1_GrenadeGun;
    public GameObject Gun_RankA_2_Revolver;
    public GameObject Gun_RankA_3_MarksmanRifle;
    public GameObject Gun_RankA_4_SniperRifle;
    public GameObject Gun_RankA_5_ShotGun;
    public GameObject Gun_RankA_6_SubMachineGun;
    public GameObject Gun_RankA_7_Launcher;
    public GameObject Gun_RankA_8_TacticalAssault;
    public GameObject Gun_RankA_9_BurstAssault;
    public GameObject Gun_RankA_10_MachineGun;
    public GameObject Gun_RankA_11_HeavySniper;
    public GameObject Gun_RankA_12_4Launcher;
    public GameObject Gun_RankA_13_BurstSMG;
    public GameObject Gun_RankA_14_TacticalSMG;

    //※※※ランクS※※※
    public GameObject Gun_RankS_1_GoldenHandGun;
    public GameObject Gun_RankS_2_GoldenPistol;
    public GameObject Gun_RankS_3_GoldenSawedOff;
    public GameObject Gun_RankS_4_GoldenShotGun;
    public GameObject Gun_RankS_5_GoldenMiniMachineGun;
    public GameObject Gun_RankS_6_GoldenSubMachineGun;
    public GameObject Gun_RankS_7_GoldenRevolver;
    public GameObject Gun_RankS_8_GoldenMagnum;
    public GameObject Gun_RankS_9_GoldenRifle;
    public GameObject Gun_RankS_10_GoldenSniper;
    public GameObject Gun_RankS_11_GoldenGrenedeGun;
    public GameObject Gun_RankS_12_GoldenLauncher;

    //ーーーーーーーーーーショップの武器のプレハブーーーーーーーーーー
    //※※※ランクB※※※
    public GameObject ShopGun_RankB_3_ThroughGun;
    public GameObject ShopGun_RankB_4_BoundGun;
    public GameObject ShopGun_RankB_5_MiniMachineGun;
    public GameObject ShopGun_RankB_6_AssaultRifle;
    public GameObject ShopGun_RankB_7_FullAutoPistol;
    public GameObject ShopGun_RankB_8_Pistol;
    public GameObject ShopGun_RankB_9_CompactRifle;
    public GameObject ShopGun_RankB_10_HeavyRifle;
    public GameObject ShopGun_RankB_11_TwinRifle;
    public GameObject ShopGun_RankB_12_LightMachineGun;
    public GameObject ShopGun_RankB_13_CompactSMG;

    //※※※ランクA※※※
    public GameObject ShopGun_RankA_1_GrenadeGun;
    public GameObject ShopGun_RankA_2_Revolver;
    public GameObject ShopGun_RankA_3_MarksmanRifle;
    public GameObject ShopGun_RankA_4_SniperRifle;
    public GameObject ShopGun_RankA_5_ShotGun;
    public GameObject ShopGun_RankA_6_SubMachineGun;
    public GameObject ShopGun_RankA_7_Launcher;
    public GameObject ShopGun_RankA_8_TacticalAssault;
    public GameObject ShopGun_RankA_9_BurstAssault;
    public GameObject ShopGun_RankA_10_MachineGun;
    public GameObject ShopGun_RankA_11_HeavySniper;
    public GameObject ShopGun_RankA_12_4Launcher;
    public GameObject ShopGun_RankA_13_BurstSMG;
    public GameObject ShopGun_RankA_14_TacticalSMG;

    /// <summary>
    /// クラスナンバーによってセットするアイテムのオブジェクトを変えるメソッド
    /// </summary>
    /// <param name="classNumber">分類番号</param>
    /// <returns>分類番号のアイテム</returns>
    public GameObject SetItem(int classNumber)
    {
        GameObject item = null;

        switch (classNumber)
        {
            case 1:
                item = Item1_Heart_Full;
                break;

            case 2:
                item = Item2_Heart_Half;
                break;

            case 3:
                item = Item3_Bomb;
                break;

            case 4:
                item = Item4_BulletBox_Red;
                break;

            case 5:
                item = Item5_BulletBox_Green;
                break;

            case 6:
                item = Item6_BulletBox_Blue;
                break;

            case 7:
                item = Item7_Coin_Bronze;
                break;

            case 8:
                item = Item8_Coin_Silver;
                break;

            case 9:
                item = Item9_Coin_Gold;
                break;

            case 10:
                item = Item10_Diamond;
                break;
        }

        return item;
    }

    /// <summary>
    /// クラスナンバーによってセットするショップアイテムのオブジェクトを変えるメソッド
    /// </summary>
    /// <param name="classNumber">分類番号</param>
    /// <returns>分類番号のアイテム</returns>
    public GameObject SetShopItem(int classNumber)
    {
        GameObject item = null;

        switch (classNumber)
        {
            case 1:
                item = ShopItem1_Heart_Full;
                break;

            case 2:
                item = ShopItem2_Heart_Half;
                break;

            case 3:
                item = ShopItem3_Bomb;
                break;

            case 4:
                item = ShopItem4_BulletBox_Red;
                break;

            case 5:
                item = ShopItem5_BulletBox_Green;
                break;

            case 6:
                item = ShopItem6_BulletBox_Blue;
                break;
        }

        return item;
    }

    /// <summary>
    /// クラスナンバーによってセットする銃のオブジェクトを変えるメソッド
    /// 主にシーン間のデータ引き継ぎ用
    /// </summary>
    /// <param name="classNumber">分類番号</param>
    /// <returns>分類番号の銃</returns>
    public GameObject SetGun(int classNumber)
    {
        GameObject gun = null;

        switch (classNumber)
        {
            case 1:
                gun = Gun_RankB_1_HandGun;
                break;

            case 2:
                gun = Gun_RankB_2_SawedOffShotGun;
                break;

            case 3:
                gun = Gun_RankB_3_ThroughGun;
                break;

            case 4:
                gun = Gun_RankB_4_BoundGun;
                break;

            case 5:
                gun = Gun_RankA_1_GrenadeGun;
                break;

            case 6:
                gun = Gun_RankA_2_Revolver;
                break;

            case 7:
                gun = Gun_RankA_3_MarksmanRifle;
                break;

            case 8:
                gun = Gun_RankA_4_SniperRifle;
                break;

            case 9:
                gun = Gun_RankA_5_ShotGun;
                break;

            case 10:
                gun = Gun_RankB_5_MiniMachineGun;
                break;

            case 11:
                gun = Gun_RankA_6_SubMachineGun;
                break;

            case 12:
                gun = Gun_RankA_7_Launcher;
                break;

            case 13:
                gun = Gun_RankS_1_GoldenHandGun;
                break;

            case 14:
                gun = Gun_RankS_2_GoldenPistol;
                break;

            case 15:
                gun = Gun_RankS_3_GoldenSawedOff;
                break;

            case 16:
                gun = Gun_RankS_4_GoldenShotGun;
                break;

            case 17:
                gun = Gun_RankS_5_GoldenMiniMachineGun;
                break;

            case 18:
                gun = Gun_RankS_6_GoldenSubMachineGun;
                break;

            case 19:
                gun = Gun_RankS_7_GoldenRevolver;
                break;

            case 20:
                gun = Gun_RankS_8_GoldenMagnum;
                break;

            case 21:
                gun = Gun_RankS_9_GoldenRifle;
                break;

            case 22:
                gun = Gun_RankS_10_GoldenSniper;
                break;

            case 23:
                gun = Gun_RankS_11_GoldenGrenedeGun;
                break;

            case 24:
                gun = Gun_RankS_12_GoldenLauncher;
                break;

            case 25:
                gun = Gun_RankA_8_TacticalAssault;
                break;

            case 26:
                gun = Gun_RankB_6_AssaultRifle;
                break;

            case 27:
                gun = Gun_RankA_9_BurstAssault;
                break;

            case 28:
                gun = Gun_RankB_7_FullAutoPistol;
                break;

            case 29:
                gun = Gun_RankB_8_Pistol;
                break;

            case 30:
                gun = Gun_RankB_9_CompactRifle;
                break;

            case 31:
                gun = Gun_RankB_10_HeavyRifle;
                break;

            case 32:
                gun = Gun_RankB_11_TwinRifle;
                break;

            case 33:
                gun = Gun_RankA_10_MachineGun;
                break;

            case 34:
                gun = Gun_RankA_11_HeavySniper;
                break;

            case 35:
                gun = Gun_RankA_12_4Launcher;
                break;

            case 36:
                gun = Gun_RankB_12_LightMachineGun;
                break;

            case 37:
                gun = Gun_RankB_13_CompactSMG;
                break;

            case 38:
                gun = Gun_RankA_13_BurstSMG;
                break;

            case 39:
                gun = Gun_RankA_14_TacticalSMG;
                break;
        }

        return gun;
    }

    /// <summary>
    /// クラスナンバーによってセットするショップの銃のオブジェクトを変えるメソッド
    /// </summary>
    /// <param name="classNumber">分類番号</param>
    /// <returns>分類番号の銃</returns>
    public GameObject SetShopGun(int classNumber)
    {
        GameObject gun = null;

        switch (classNumber)
        {
            case 3:
                gun = ShopGun_RankB_3_ThroughGun;
                break;

            case 4:
                gun = ShopGun_RankB_4_BoundGun;
                break;

            case 5:
                gun = ShopGun_RankA_1_GrenadeGun;
                break;

            case 6:
                gun = ShopGun_RankA_2_Revolver;
                break;

            case 7:
                gun = ShopGun_RankA_3_MarksmanRifle;
                break;

            case 8:
                gun = ShopGun_RankA_4_SniperRifle;
                break;

            case 9:
                gun = ShopGun_RankA_5_ShotGun;
                break;

            case 10:
                gun = ShopGun_RankB_5_MiniMachineGun;
                break;

            case 11:
                gun = ShopGun_RankA_6_SubMachineGun;
                break;

            case 12:
                gun = ShopGun_RankA_7_Launcher;
                break;

            case 25:
                gun = ShopGun_RankA_8_TacticalAssault;
                break;

            case 26:
                gun = ShopGun_RankB_6_AssaultRifle;
                break;

            case 27:
                gun = ShopGun_RankA_9_BurstAssault;
                break;

            case 28:
                gun = ShopGun_RankB_7_FullAutoPistol;
                break;

            case 29:
                gun = ShopGun_RankB_8_Pistol;
                break;

            case 30:
                gun = ShopGun_RankB_9_CompactRifle;
                break;

            case 31:
                gun = ShopGun_RankB_10_HeavyRifle;
                break;

            case 32:
                gun = ShopGun_RankB_11_TwinRifle;
                break;

            case 33:
                gun = ShopGun_RankA_10_MachineGun;
                break;

            case 34:
                gun = ShopGun_RankA_11_HeavySniper;
                break;

            case 35:
                gun = ShopGun_RankA_12_4Launcher;
                break;

            case 36:
                gun = ShopGun_RankB_12_LightMachineGun;
                break;

            case 37:
                gun = ShopGun_RankB_13_CompactSMG;
                break;

            case 38:
                gun = ShopGun_RankA_13_BurstSMG;
                break;

            case 39:
                gun = ShopGun_RankA_14_TacticalSMG;
                break;
        }

        return gun;
    }
}
