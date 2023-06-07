using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// ほとんど全てのオブジェクトにアクセスでき、汎用的なメソッドをまとめたクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    //※※※※※※※※※※シリアライズする変数※※※※※※※※※※

    //ーーーーーーーーーーゲームオブジェクトーーーーーーーーーー
    /// <summary>
    /// オブジェクト　プレイヤー
    /// </summary>
    public GameObject player;
    /// <summary>
    /// オブジェクト　装備中の武器のUI
    /// </summary>
    public GameObject gunUIManager;
    /// <summary>
    /// オブジェクト　武器選択画面
    /// </summary>
    public GameObject gunSelectUIMGR;
    /// <summary>
    /// オブジェクト　ゲームを終了しますか？のメニュー
    /// </summary>
    public GameObject endGameSelectUI;
    /// <summary>
    /// オブジェクト　ゲームクリアのメニュー
    /// </summary>
    public GameObject GameClear;
    /// <summary>
    /// オブジェクト　ゲームオーバーのメニュー
    /// </summary>
    public GameObject gameOverMenu;
    /// <summary>
    /// オブジェクト　操作確認画面
    /// </summary>
    public GameObject operationsConfirm;
    /// <summary>
    /// オブジェクト　UIをまとめたマネージャー
    /// </summary>
    public GameObject UIMGR;
    /// <summary>
    /// オブジェクト　マップに表示する、プレイヤーの位置を表示する点
    /// </summary>
    public GameObject mapPoint;
    /// <summary>
    /// オブジェクト　看板を読んだときに表示する、文字の背景
    /// </summary>
    public GameObject signboardTextBackground;
    /// <summary>
    /// オブジェクト　プレハブマネージャー
    /// </summary>
    public GameObject prehubMGR;
    /// <summary>
    /// オブジェクト　SEマネージャー
    /// </summary>
    public GameObject SEMGR;
    /// <summary>
    /// オブジェクト　BGMマネージャー
    /// </summary>
    public GameObject BGMMGR;

    //ーーーーーーーーーートランスフォームーーーーーーーーーー
    /// <summary>
    /// トランスフォーム　メインカメラ
    /// </summary>
    public Transform cameraTransform;
    /// <summary>
    /// トランスフォーム　敵の弾の親オブジェクト
    /// </summary>
    public Transform enemyBulletManagerTransform;
    /// <summary>
    /// トランスフォーム　ロードの親オブジェクト
    /// </summary>
    public Transform roadManagerTranform;
    /// <summary>
    /// レクトトランスフォーム　ハートのUI
    /// </summary>
    public RectTransform heartUITransform;
    /// <summary>
    /// レクトトランスフォーム　ボムのUI
    /// </summary>
    public RectTransform bombUITransform;

    //ーーーーーーーーーーキャンバスーーーーーーーーーー
    /// <summary>
    /// キャンバス　UIをまとめたキャンバス
    /// </summary>
    public Canvas UICanvas;

    //ーーーーーーーーーーイメージーーーーーーーーーー
    /// <summary>
    /// イメージ　照準
    /// </summary>
    public Image aimImage;
    /// <summary>
    /// イメージ　フラッシュ（画面いっぱいの白パネル）
    /// </summary>
    public Image flash;
    /// <summary>
    /// イメージ　ブラックパネル（画面いっぱいの黒パネル）
    /// </summary>
    public Image blackPanel;
    /// <summary>
    /// イメージ　リロードの度合いを示すサークル
    /// </summary>
    public Image reloadCircle;

    //ーーーーーーーーーーマテリアルーーーーーーーーーー
    /// <summary>
    /// マテリアル　画像に白フチをつける
    /// </summary>
    public Material outline;
    /// <summary>
    /// マテリアル　デフォルト
    /// </summary>
    public Material defaultMaterial;

    //ーーーーーーーーーースライダーーーーーーーーーーー
    /// <summary>
    /// スライダー　ボスのHPバー
    /// </summary>
    public Slider bossHPSlider;

    //ーーーーーーーーーーテキストーーーーーーーーーー
    /// <summary>
    /// テキスト　ボスの名前
    /// </summary>
    public TextMeshProUGUI bossNameText;
    /// <summary>
    /// テキスト　看板の文字
    /// </summary>
    public TextMeshProUGUI signboardText;
    /// <summary>
    /// テキスト　ステージの名前
    /// </summary>
    public TextMeshProUGUI stageName;
    /// <summary>
    /// テキスト　コインの枚数
    /// </summary>
    public TextMeshProUGUI coin;

    //ーーーーーーーーーースクリプトーーーーーーーーーー
    /// <summary>
    /// スクリプト　持っている武器を管理するクラス
    /// </summary>
    public GunManager gunManager;
    /// <summary>
    /// スクリプト　ミニマップを管理するクラス
    /// </summary>
    public MiniMap miniMap;
    /// <summary>
    /// スクリプト　キー入力を管理するクラス
    /// </summary>
    public KeyInputManager keyInputManager;
    /// <summary>
    /// スクリプト　演出を管理するクラス
    /// </summary>
    public StagingManager stagingManager;
    /// <summary>
    ///  敵のメソッドをまとめたクラス
    /// </summary>
    public EnemyMethod enemyMethod;
    /// <summary>
    /// スクリプト　UIを管理するクラス
    /// </summary>
    public UIManager uiManager;
    /// <summary>
    /// スクリプト　武器切替画面のクラス
    /// </summary>
    public GunSelectUIManager gunSelectUIManager;


    //※※※※※※※※※※Awakeで代入する変数※※※※※※※※※※
    /// <summary>
    /// トランスフォーム　プレイヤー
    /// </summary>
    public Transform playerTransform;
    /// <summary>
    /// スクリプト　プレイヤーを管理するクラス
    /// </summary>
    public Player playerScript;
    /// <summary>
    /// トランスフォーム　照準のトランスフォーム
    /// </summary>
    public Transform aimImageTransform;
    /// <summary>
    /// レクトトランスフォーム　照準のレクトトランスフォーム
    /// </summary>
    public RectTransform aimImageRect;
    /// <summary>
    ///レクトトランスフォーム　武器切替画面のレクトトランスフォーム
    /// </summary>
    public RectTransform gunSelectUIRect;
    /// <summary>
    /// スクリプト　プレハブを管理するクラス
    /// </summary>
    public PrehubManager prehubManager;
    /// <summary>
    /// スクリプト　BGMを管理するクラス
    /// </summary>
    public BGMManager BGMManager;
    /// <summary>
    /// オーディオソース　BGMのオーディオソース
    /// </summary>
    public AudioSource audioSourceBGM;
    /// <summary>
    /// スクリプト　SEを管理するクラス
    /// </summary>
    public SEManager SEManager;
    /// <summary>
    /// オーディオソース　SEのオーディオソース
    /// </summary>
    public AudioSource audioSourceSE;


    //※※※※※※※※※※常に更新される変数※※※※※※※※※※
    /// <summary>
    /// ３次元ベクトル　プレイヤーのポジション
    /// </summary>
    public Vector3 playerPos;
    /// <summary>
    /// ３次元ベクトル　照準のポジション
    /// </summary>
    public Vector3 aimImagePos;


    //※※※※※※※※※※外部から切り替える変数※※※※※※※※※※
    /// <summary>
    /// ブール　ボスを動きを開始するかどうか
    /// </summary>
    public bool bossMoveStart = false;
    /// <summary>
    /// ブール　ボスが攻撃アニメを再生するかどうか
    /// </summary>
    public bool bossAttackAnime = false;
    /// <summary>
    /// ブール　ボスが攻撃アニメを停止するかどうか
    /// </summary>
    public bool bossAttackAnimeOff = false;
    /// <summary>
    /// ブール　プレイヤーの操作を可能にするかどうか
    /// </summary>
    public bool noShoot = false;
    /// <summary>
    /// ブール　ステージはエントランスかどうか
    /// </summary>
    public bool entrance = false;
    /// <summary>
    /// チュートリアルかどうか
    /// </summary>
    public bool tutorial = false;


    //※※※※※※※※※※経過時間を格納する変数※※※※※※※※※※
    /// <summary>
    /// 時間
    /// </summary>
    int hour;
    /// <summary>
    /// 分
    /// </summary>
    int minite;
    /// <summary>
    /// 秒
    /// </summary>
    float seconds;



    void Awake()
    {
        hour = GameData.hour;
        minite = GameData.minite;
        seconds = GameData.seconds;

        playerTransform = player.transform;
        playerScript = player.GetComponent<Player>();
        aimImageTransform = aimImage.transform;
        aimImageRect = aimImage.rectTransform;
        if (gunSelectUIMGR != null)
        {
            gunSelectUIRect = gunSelectUIMGR.GetComponent<RectTransform>();
            gunSelectUIManager = gunSelectUIMGR.GetComponent<GunSelectUIManager>();
        }
        uiManager = UIMGR.GetComponent<UIManager>();
        prehubManager = prehubMGR.GetComponent<PrehubManager>();
        BGMManager = BGMMGR.GetComponent<BGMManager>();
        audioSourceBGM = BGMMGR.GetComponent<AudioSource>();
        SEManager = SEMGR.GetComponent<SEManager>();
        audioSourceSE = SEMGR.GetComponent<AudioSource>();

        StartCoroutine(BlackIn()); //ゲーム開始時に黒画面からフェードインする
        StartCoroutine(StageNameDisplay()); //ゲーム開始時にステージ名を表示する
    }

    void Update()
    {
        playerPos = playerTransform.position;
        aimImagePos = aimImageTransform.position;

        TimeCount(); //常に経過時間をカウントする
    }


    /// <summary>
    /// 経過時間をカウントするメソッド
    /// </summary>
    void TimeCount()
    {
        seconds += Time.deltaTime;
        //60秒たったら１分に直す
        if (seconds >= 60f)
        {
            minite++;
            seconds -= 60;
        }
        //60分たったら1時間に直す
        if (minite >= 60)
        {
            hour++;
            minite -= 60;
        }
    }

    /// <summary>
    /// 現在の経過時間を返すメソッド
    /// </summary>
    /// <returns>経過時間の配列、[0]時間、[1]分、[2]秒</returns>
    public int[] ElapsedTime()
    {
        int[] times = new int[3];
        times[0] = hour;
        times[1] = minite;
        times[2] = (int)seconds;
        return times;
    }

    /// <summary>
    /// 黒画面からフェードインするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator BlackIn()
    {
        GameObject blackPanelObj = blackPanel.gameObject;

        //黒パネルを表示
        blackPanelObj.SetActive(true);
        blackPanel.color = new Color(1, 1, 1, 1);

        //黒パネルの透明度を徐々に上げる
        for (float i = 0; i < 1; i += 0.008f)
        {
            blackPanel.color = new Color(1, 1, 1, 1 - i);
            yield return null;
        }

        //黒パネルを非表示
        blackPanel.color = new Color(1, 1, 1, 0);
        blackPanelObj.SetActive(false);
    }

    /// <summary>
    /// ステージ開始時にステージの名前を表示するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator StageNameDisplay()
    {
        //ステージ名を透明に
        stageName.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(1.0f);

        //少し待ってからステージ名の透明度を徐々に下げる
        for (float i = 0; i < 1; i += 0.008f)
        {
            stageName.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);　//ステージ名を表示したまま少し停止

        //ステージ名の透明度を徐々に上げる
        for (float i = 0; i < 1; i += 0.008f)
        {
            stageName.color = new Color(1, 1, 1, 1 - i);
            yield return null;
        }
        //ステージ名を透明に
        stageName.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 揺れの時間と大きさを引数とし、カメラを揺らすコルーチンを呼び出すメソッド
    /// </summary>
    /// <param name="shakeTime">揺れの時間</param>
    /// <param name="shakeSize">揺れの大きさ</param>
    public void CameraShake(float shakeTime, float shakeSize)
    {
        StartCoroutine(DoShake(shakeTime, shakeSize));
    }

    /// <summary>
    /// 揺れの時間と大きさを引数とし、カメラを揺らすコルーチン
    /// </summary>
    /// <param name="shakeTime">揺れの時間</param>
    /// <param name="shakeSize">揺れの大きさ</param>
    /// <returns></returns>
    IEnumerator DoShake(float shakeTime, float shakeSize)
    {
        Vector3 cameraPos = cameraTransform.position; //揺れ開始時のカメラ位置を保存      
        float time = 0.0f;

        //指定した時間が経過するまで揺らし続ける
        while (shakeTime > time)
        {
            //カメラをランダムに揺らす
            float x = cameraPos.x + Random.Range(-1.0f, 1.0f) * shakeSize;
            float y = cameraPos.y + Random.Range(-1.0f, 1.0f) * shakeSize;
            cameraTransform.position = new Vector3(x, y, cameraPos.z);

            time += Time.deltaTime;

            if (Time.timeScale == 0) { yield break; } //カメラ揺れ中に時間を止めると揺れ続けるのを防ぐ

            yield return null;
        }
        //カメラを初期位置に戻す
        cameraTransform.position = cameraPos;
    }


    /// <summary>
    /// 画面をフラッシュするコルーチンを呼び出すメソッド
    /// </summary>
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    /// <summary>
    /// 画面をフラッシュするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DoFlash()
    {
        float maxAlpha = 0.6f; //画面を真っ白にすると目に悪いため、ある程度の透明度から始める

        //白パネルを表示
        GameObject flashObj = flash.gameObject;
        flashObj.SetActive(true);
        flash.color = new Color(1, 1, 1, maxAlpha);

        //徐々に透明度を下げる
        for (float i = 0; i < maxAlpha; i += 0.01f)
        {
            flash.color = new Color(1, 1, 1, maxAlpha - i);
            yield return null;
        }

        //白パネルを非表示
        flash.color = new Color(1, 1, 1, 0);
        flashObj.SetActive(false);
    }


    /// <summary>
    /// 照準を透明にしてカーソルを表示するメソッド
    /// </summary>
    public void DisplayCursor()
    {
        aimImage.color = new Color(1, 1, 1, 0);
        Cursor.visible = true;
    }

    /// <summary>
    /// 照準を不透明にしてカーソルを非表示にするメソッド
    /// </summary>
    public void DisplayAimImage()
    {
        aimImage.color = new Color(1, 1, 1, 1);
        Cursor.visible = false;
    }


    /// <summary>
    /// 指定された時間の後オブジェクトを破壊するコルーチン、を呼び出すメソッド
    /// </summary>
    /// <param name="obj">破壊するオブジェクト</param>
    /// <param name="destroyTime">破壊までの時間</param>
    public void DestroyObj(GameObject obj, float destroyTime)
    {
        StartCoroutine(DestObj(obj, destroyTime));
    }

    /// <summary>
    /// 指定された時間の後、オブジェクトを破壊するコルーチン
    /// </summary>
    /// <param name="obj">破壊するオブジェクト</param>
    /// <param name="destroyTime">破壊までの時間</param>
    /// <returns></returns>
    IEnumerator DestObj(GameObject obj, float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(obj);
    }

    /// <summary>
    /// 敵の弾を全て消すメソッド
    /// </summary>
    public void DestroyEnemyBullet()
    {
        foreach (Transform childTransform in enemyBulletManagerTransform) //全ての敵の弾オブジェクトをDestroy
        {
            Destroy(childTransform.gameObject);
        }
    }

    /// <summary>
    /// プレイヤーのキル数を増やすメソッド
    /// </summary>
    public void KillCountUp()
    {
        playerScript.killCount += 1;
    }

    /// <summary>
    /// 暗転した後、少し待ってからシーン遷移するコルーチン
    /// </summary>
    /// <param name="transitionNumber"></param>
    /// <returns></returns>
    public IEnumerator BlackOutAndTransition(int transitionNumber)
    {
        //黒パネルを表示
        GameObject blackPanelObj = this.blackPanel.gameObject;
        blackPanelObj.SetActive(true);
        Image blackPanel = this.blackPanel;

        //黒パネルを透明に
        blackPanel.color = new Color(1, 1, 1, 0);

        //黒パネルの透明度を徐々に下げる
        for (float i = 0; i < 1; i += 0.008f)
        {
            blackPanel.color = new Color(1, 1, 1, i);
            yield return null;
        }

        //黒パネルを不透明に
        blackPanel.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2.0f);

        //少し待ってから画面遷移
        TransitionStage(transitionNumber);
    }

    /// <summary>
    /// 画面遷移するメソッド
    /// </summary>
    public void TransitionStage(int transitionStageNumber)
    {
        //引数によって遷移先を変化
        switch (transitionStageNumber)
        {
            //ゲーム終了しますか？のメニュー
            case 0:
                endGameSelectUI.SetActive(true);
                break;

            //エントランスへ遷移し、データを初期化
            case 1:
                SceneManager.LoadScene("Entrance");
                GameDataInitialize();
                ItemAndGunData.GunsListInitialize();
                break;

            //チュートリアルへ遷移し、データを初期化
            case 2:
                SceneManager.LoadScene("Tutorial");
                GameDataInitialize();
                ItemAndGunData.GunsListInitialize();
                break;

            //ステージ１へ遷移し、データを初期化
            case 3:
                SceneManager.LoadScene("Stage1");
                GameDataInitialize();
                ItemAndGunData.GunsListInitialize();
                break;

            //ステージ２へ遷移し、データを更新
            case 4:
                SceneManager.LoadScene("Stage2");
                GameDataUpdate();
                break;

            //ステージ３へ遷移し、データを更新
            case 5:
                SceneManager.LoadScene("Stage3");
                GameDataUpdate();
                break;

            case 6: //ゴールへ遷移し、データを更新
                SceneManager.LoadScene("Goal");
                GameDataUpdate();
                break;
        }
    }

    /// <summary>
    /// ゲームデータの初期化を行うメソッド
    /// </summary>
    void GameDataInitialize()
    {
        GameData.playerHP = 20; //チュートリアルに合わせて高く設定（最大値を超えたら自動的に最大値になる）
        GameData.holdCoin = 0;
        GameData.killCount = 0;
        GameData.hour = 0;
        GameData.minite = 0;
        GameData.seconds = 0;

        GameData.equipGunNumbers = new int[2] { 1, 2 }; //手持ちの武器は2個、初期武器は1と2（ハンドガンとソードオフショットガン）
        GameData.holdGunNumbers = new int[20]; //控えの武器はとりあえず20個まで

        GameData.equipGunRemainBullets = new int[2] { 100000, 150 };//手持ちの武器の残弾、初期の残弾は10万と150（ハンドガンとソードオフショットガン）
        GameData.holdGunRemainBullets = new int[20]; //控えの武器の残弾
    }

    /// <summary>
    /// ゲームデータの更新を行うメソッド
    /// </summary>
    void GameDataUpdate()
    {
        GameData.playerHP = playerScript.playerHP;
        GameData.holdCoin = playerScript.holdCoin;
        GameData.killCount = playerScript.killCount;
        GameData.hour = ElapsedTime()[0];
        GameData.minite = ElapsedTime()[1];
        GameData.seconds = ElapsedTime()[2];

        gunManager.SetGunData();
    }

    /// <summary>
    /// ゲームクリア時のコルーチンを呼ぶメソッド
    /// </summary>
    public void CallClearGame()
    {
        StartCoroutine(ClearGame());
    }

    /// <summary>
    /// ゲームクリア時のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ClearGame()
    {
        keyInputManager.noControl = true;
        yield return new WaitForSeconds(2.0f);

        GameObject blackPanelObj = blackPanel.gameObject;

        //黒パネルを表示して透明に
        blackPanelObj.SetActive(true);
        blackPanel.color = new Color(1, 1, 1, 0);

        //黒パネルの透明度を徐々に下げる
        for (float i = 0; i < 1; i += 0.002f)
        {
            blackPanel.color = new Color(1, 1, 1, i);
            yield return null;
        }

        //黒パネルを表示
        blackPanel.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2.0f);

        GameClear.SetActive(true);
    }


    /// <summary>
    /// ゲームを終了するメソッド
    /// </summary>
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

}