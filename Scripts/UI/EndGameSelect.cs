using UnityEngine;

/// <summary>
/// ゲームを終了するかどうかのメニューのクラス
/// </summary>
public class EndGameSelect : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    KeyInputManager keyInputManager;

    SEManager SEManager;
    AudioSource audioSource;
    AudioClip openSE;
    AudioClip closeSE;

    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GunSelectOpen;
        closeSE = SEManager.SE_GunSelectClose;
    }

    /// <summary>
    /// 起動したら効果音を鳴らし、動けないようにするメソッド
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE);
        keyInputManager.noControl = true;
        Time.timeScale = 0;
        gameManager.DisplayCursor(); //照準からカーソルに変更
    }

    //はいを押したら
    /// <summary>
    /// ゲームを終了するメソッドを呼び出すメソッド
    /// </summary>
    public void EndGameYes()
    {
        gameManager.EndGame();
    }

    //いいえを押したら
    /// <summary>
    /// プレイヤーを(0,0)に移動し、再び動けるようにするメソッド
    /// </summary>
    public void EndGameNo()
    {
        gameManager.DisplayAimImage();

        audioSource.PlayOneShot(closeSE);

        gameManager.playerTransform.position = new Vector2(0, 0);
        keyInputManager.noControl = false;

        gameObject.SetActive(false);
        Time.timeScale = 1;
        gameManager.noShoot = false;
    }
}
