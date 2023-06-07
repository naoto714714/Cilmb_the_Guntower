using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// ゲームオーバーのメニューを管理するクラス
/// </summary>
public class GameOverMenu : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI killText;
    KeyInputManager keyInputManager;
    Player player;

    SEManager SEManager;
    AudioSource audioSource;
    AudioClip openSE;
    AudioClip clickSE;

    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;
        player = gameManager.playerScript;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GameOverDisplay;
        clickSE = SEManager.SE_GameOverClick;
    }

    /// <summary>
    /// 起動したら動けないようにするメソッド
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE);
        keyInputManager.noControl = true;
        gameManager.DisplayCursor();
        
        //ゲームオーバーになったときの、時間・コイン・ステージ名・撃破数を取得
        int[] times = gameManager.ElapsedTime();
        timeText.text = times[0].ToString("0") + ":" + times[1].ToString("00") + ":" + times[2].ToString("00");
        coinText.text = player.holdCoin.ToString();
        stageText.text = gameManager.stageName.text;
        killText.text =  player.killCount.ToString();
    }

    //リスタートを押したら
    /// <summary>
    /// ステージ１へ遷移するコルーチンを呼び出す
    /// </summary>
    public void Restart()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(gameManager.BlackOutAndTransition(3));
    }

    //エントランスへを押したら
    /// <summary>
    /// エントランスへシーン遷移するコルーチンを呼び出す
    /// </summary>
    public void ToEntrance()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(gameManager.BlackOutAndTransition(1));
    }

    //チュートリアルのりスタートはチュートリアルからにする
    public void RestartTutorial()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(gameManager.BlackOutAndTransition(2));
    }
}
