using UnityEngine;
using TMPro;
using Calc;
using Text;

/// <summary>
/// 看板を管理するクラス
/// </summary>
public class Signboard : MonoBehaviour
{
    GameManager gameManager;
    SEManager SEManager;
    [SerializeField] int textNumber;

    Transform myTransform;
    SpriteRenderer mySpriteRenderer;

    const float canReadRange = 3.0f; //看板を読むことができる範囲

    //ゲームマネージャから読み込む変数
    Material outline;
    Material defaultMaterial;
    GameObject signboardTextBackground;
    TextMeshProUGUI signboardText;
    GameObject miniMap;
    GameObject mapPoint;

    AudioClip signboardOpenSE;
    AudioClip signboardCloseSE;
    AudioSource audioSource;

    string text;
    bool isReading = false;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myTransform = gameObject.transform;
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        outline = gameManager.outline; //白フチ
        defaultMaterial = gameManager.defaultMaterial; //白フチなし

        signboardTextBackground = gameManager.signboardTextBackground;
        signboardText = gameManager.signboardText;

        miniMap = gameManager.miniMap.gameObject;
        mapPoint = gameManager.mapPoint;

        signboardOpenSE = SEManager.SE_SignboardOpen;
        signboardCloseSE = SEManager.SE_SignboardClose;

        text = TextData.setText(textNumber);
    }


    void Update()
    {
        Vector3 myPos = myTransform.position;
        Vector3 playerPos = gameManager.playerPos;

        mySpriteRenderer.material = defaultMaterial;

        //看板を読んでるとき、プレイヤーが離れるか、Eキーを押されたら看板を閉じる
        if (isReading)
        {
            if (Calculate.Distance(myPos, playerPos) > canReadRange)
            {
                HiddenText();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                HiddenText();
                mySpriteRenderer.material = outline;
                return;
            }
        }

        //プレイヤーがcanOpenRange内にいるか判定し、いたら白フチをつける
        if (Calculate.Distance(myPos, playerPos) > canReadRange) { return; }

        mySpriteRenderer.material = outline;

        //宝箱の近くにいるとき、Eキーを押されたら看板の文字を表示する
        if (Input.GetKeyDown(KeyCode.E))
        {
            DisplayText();
        }
    }

    /// <summary>
    /// 看板の文字を表示するメソッド
    /// </summary>
    void DisplayText()
    {
        audioSource.PlayOneShot(signboardOpenSE);

        //看板の文字を表示
        signboardTextBackground.SetActive(true);
        signboardText.text = text;

        //UIをいくつか非表示
        miniMap.SetActive(false);
        mapPoint.SetActive(false);

        isReading = true;
    }

    /// <summary>
    /// 看板の文字を閉じるメソッド
    /// </summary>
    void HiddenText()
    {
        audioSource.PlayOneShot(signboardCloseSE);

        //看板の文字を非表示
        signboardTextBackground.SetActive(false);

        //非表示にしたUIを表示
        miniMap.SetActive(true);
        mapPoint.SetActive(true);

        isReading = false;
    }
}
