using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// ゲームオーバーのメニューを管理するクラス
/// </summary>
public class GameClear : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI killText;
    Player player;

    SEManager SEManager;
    AudioSource audioSource;
    AudioClip openSE;
    AudioClip clickSE;

    void Awake()
    {
        player = gameManager.playerScript;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GameClearDisplay;
        clickSE = SEManager.SE_GameOverClick;
    }

    /// <summary>
    /// 起動したときのメソッド
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE);
        gameManager.DisplayCursor();
        
        //ゲームクリアしたときの時間・コイン・ステージ名・撃破数を取得
        int[] times = gameManager.ElapsedTime();
        timeText.text = times[0].ToString("0") + ":" + times[1].ToString("00") + ":" + times[2].ToString("00");
        coinText.text = player.holdCoin.ToString();
        stageText.text = gameManager.stageName.text;
        killText.text =  player.killCount.ToString();
    }

    /// <summary>
    /// エントランスへ遷移するメソッド
    /// </summary>
    public void ToEntrance()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(BlackOutAndTransition(1));
    }

    /// <summary>
    /// 暗転した後、少し待ってからシーン遷移するコルーチン
    /// </summary>
    /// <param name="transitionNumber"></param>
    /// <returns></returns>
    public IEnumerator BlackOutAndTransition(int transitionNumber)
    {
        //並び順を入れ替え、黒パネルを最前面に
        gameObject.transform.SetAsFirstSibling();

        yield return new WaitForSeconds(4.0f);

        //少し待ってから画面遷移
        gameManager.TransitionStage(transitionNumber);
    }
}
