using UnityEngine;

/// <summary>
/// 操作確認画面を管理するクラス
/// </summary>
public class OperationsConfirm : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    KeyInputManager keyInputManager;

    GameObject gunUIManager;
    GameObject miniMap;
    GameObject mapPoint;

    SEManager SEManager;
    AudioClip openSE;
    AudioClip closeSE;
    AudioSource audioSource;

    bool wasNoControl = false;
    bool wasNoShoot = false;


    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GunSelectOpen;
        closeSE = SEManager.SE_GunSelectClose;

        gunUIManager = gameManager.gunUIManager;
        miniMap = gameManager.miniMap.gameObject;
        mapPoint = gameManager.mapPoint;
    }


    /// <summary>
    /// 操作確認画面を開いたときのメソッド
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE); //操作確認画面を開いたときのSE

        //動けないように
        Time.timeScale = 0;
        //既に動けない状態なら解除後も動けないように
        if (keyInputManager.noControl) { wasNoControl = true; }
        else { keyInputManager.noControl = true; }
        //既に撃てない状態なら解除後も撃てないように
        if (gameManager.noShoot) { wasNoShoot = true; }
        else { gameManager.noShoot = true; }

        //いくつかのUIを非表示にする
        gunUIManager.SetActive(false);
        miniMap.SetActive(false);
        mapPoint.SetActive(false);

        //照準を非表示にし、カーソルを表示
        gameManager.DisplayCursor();
    }


    void Update()
    {
        //エスケープを押されたら操作確認画面を閉じる
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.DisplayAimImage(); //カーソルを非表示にし、照準を表示
            audioSource.PlayOneShot(closeSE); //操作確認画面を閉じたときの音
                                              //
            //非表示にしたUIを表示
            gunUIManager.SetActive(true);
            miniMap.SetActive(true);
            mapPoint.SetActive(true);

            //操作確認画面をを非表示にし、動けるようにする
            gameObject.SetActive(false);
            Time.timeScale = 1;
            if (!wasNoControl) { keyInputManager.noControl = false; }
            if (!wasNoShoot) { gameManager.noShoot = false; }
        }
    }
}
