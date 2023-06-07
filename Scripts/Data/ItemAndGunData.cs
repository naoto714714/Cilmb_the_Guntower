using System.Collections.Generic;

/// <summary>
/// アイテムと銃のデータをまとめたクラス
/// </summary>
internal static class ItemAndGunData
{
    /// <summary>
    /// アイテムの分類番号と出現確率をまとめた辞書
    /// </summary>
    internal static Dictionary<int, int> itemDict = new Dictionary<int, int>()
    {
        {1, 5 }, //ハート_フル
        {2, 10 }, //ハート_ハーフ
        {3, 10 }, //ボム
        {4, 4 }, //弾薬箱_赤
        {5, 17 }, //弾薬箱_緑
        {6, 4 }, //弾薬箱_青
        {7, 25 }, //コイン_銅
        {8, 20 }, //コイン_銀
        {9, 5} //コイン_金
    };

    /// <summary>
    /// ショップアイテムの分類番号をまとめたリスト
    /// </summary>
    internal static List<int> shopItemList = new List<int>
    {
        1,
        2,
        3,
        4,
        5,
        6
    };

    /// <summary>
    /// 銃（ランクB）の分類番号をまとめたリスト
    /// </summary>
    internal static List<int> gunsListRankB = new List<int>
    {
        3,
        4,
        10,
        26,
        28,
        29,
        30,
        31,
        32,
        36,
        37
    };

    /// <summary>
    /// 銃（ランクA)の分類番号をまとめたリスト
    /// </summary>
    internal static List<int> gunsListRankA = new List<int>
    {
        5,
        6,
        7,
        8,
        9,
        11,
        12,
        25,
        27,
        33,
        34,
        35,
        38,
        39
    };


    /// <summary>
    /// 銃（ランクS)の分類番号をまとめたリスト
    /// </summary>
    internal static List<int> gunsListRankS = new List<int>
    {
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21,
        22,
        23,
        24
    };


    /// <summary>
    /// ショップアイテムのリストを初期化するメソッド
    /// </summary>
    internal static void ShopItemListInitialize()
    {
        shopItemList = new List<int>
        {
            1,
            2,
            3,
            4,
            5,
            6
        };
    }


    /// <summary>
    /// 銃のリストを初期化するメソッド
    /// </summary>
    internal static void GunsListInitialize()
    {
        gunsListRankB = new List<int>
        {
            3,
            4,
            10,
            26,
            28,
            29,
            30,
            31,
            32,
            36,
            37
        };

        gunsListRankA = new List<int>
        {
            5,
            6,
            7,
            8,
            9,
            11,
            12,
            25,
            27,
            33,
            34,
            35,
            38,
            39
        };

        gunsListRankS = new List<int>
        {
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24
        };
    }
}