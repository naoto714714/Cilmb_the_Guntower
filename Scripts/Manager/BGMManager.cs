using UnityEngine;

/// <summary>
/// BGMを管理するクラス
/// </summary>
public class BGMManager : MonoBehaviour
{
    /// <summary>
    /// 探索中のBGM
    /// </summary>
    public AudioClip BGM_Explore;
    
    /// <summary>
    /// ボスとの戦闘中のBGM
    /// </summary>
    public AudioClip BGM_Boss;
    
    /// <summary>
    /// エントランスのBGM
    /// </summary>
    public AudioClip BGM_Entrance;

    /// <summary>
    /// ショップのBGM
    /// </summary>
    public AudioClip BGM_Shop;
}
