using System.Collections.Generic;

/// <summary>
/// �A�C�e���Əe�̃f�[�^���܂Ƃ߂��N���X
/// </summary>
internal static class ItemAndGunData
{
    /// <summary>
    /// �A�C�e���̕��ޔԍ��Əo���m�����܂Ƃ߂�����
    /// </summary>
    internal static Dictionary<int, int> itemDict = new Dictionary<int, int>()
    {
        {1, 5 }, //�n�[�g_�t��
        {2, 10 }, //�n�[�g_�n�[�t
        {3, 10 }, //�{��
        {4, 4 }, //�e��_��
        {5, 17 }, //�e��_��
        {6, 4 }, //�e��_��
        {7, 25 }, //�R�C��_��
        {8, 20 }, //�R�C��_��
        {9, 5} //�R�C��_��
    };

    /// <summary>
    /// �V���b�v�A�C�e���̕��ޔԍ����܂Ƃ߂����X�g
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
    /// �e�i�����NB�j�̕��ޔԍ����܂Ƃ߂����X�g
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
    /// �e�i�����NA)�̕��ޔԍ����܂Ƃ߂����X�g
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
    /// �e�i�����NS)�̕��ޔԍ����܂Ƃ߂����X�g
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
    /// �V���b�v�A�C�e���̃��X�g�����������郁�\�b�h
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
    /// �e�̃��X�g�����������郁�\�b�h
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