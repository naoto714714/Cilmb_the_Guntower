using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// UIを管理するクラス
/// 主に、照準の画像をマウスの位置に合わせ、ハートとボムのUIを作成する
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    PrehubManager prehubManager;

    Canvas myCanvas;
    RectTransform myCanvasRect;

    bool entrance = false;

    //ゲームマネージャから読み込む変数
    Player playerScript;
    RectTransform aimImageRect;
    GameObject bomb;
    GameObject heart;

    //プレイヤーから読み込む変数
    int playerHP;
    int remainBomb;

    //UIを格納する配列
    GameObject[] heartUI;
    GameObject[] bombUI;


    void Start()
    {
        prehubManager = gameManager.prehubManager;

        entrance = gameManager.entrance;

        myCanvas = gameObject.GetComponent<Canvas>();
        myCanvasRect = myCanvas.GetComponent<RectTransform>();

        playerScript = gameManager.playerScript;
        aimImageRect = gameManager.aimImageRect;
        bomb = prehubManager.bombUIPrehub;
        heart = prehubManager.heartUIPrehub;

        playerHP = playerScript.playerHP;
        remainBomb = playerScript.remainBomb;
        int heartUINumber = playerScript.playerMaxHP / 2; //最大HPの半分の数、ハートのUIを生成する

        gameManager.DisplayAimImage(); //カーソルを非表示にし、照準を表示

        //エントランスなら、ハートとボムのUIを生成しない
        if (entrance) { return; }

        RectTransform heartUIParent = gameManager.heartUITransform;
        RectTransform bombUIParent = gameManager.bombUITransform;

        CreateUI(ref heartUI, heart, heartUINumber, 110, 80, -70, heartUIParent); //数値は、UI間のスペース、画面左上からのX座標とY座標
        CreateUI(ref bombUI, bomb, playerScript.remainBomb, 110, 80, -180, bombUIParent);
    }

 
    void FixedUpdate()
    {  
        //FixedUpdateじゃなくてUpdateだと照準の画像がガクガクする
        //aimImageの位置を、マウスの位置と合わせる
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvasRect, Input.mousePosition, myCanvas.worldCamera, out Vector2 mousePos);
        aimImageRect.anchoredPosition = new Vector2(mousePos.x, mousePos.y);
    }

    void Update()
    {
        if (entrance) { return; }

        int currentPlayerHP = playerScript.playerHP;
        int currentRemainBomb = playerScript.remainBomb;

        //プレイヤーのHPが変化したらUIも更新
        if (currentPlayerHP != playerHP)
        {
            if (currentPlayerHP <= playerScript.playerMaxHP)
            {
                StartCoroutine(UpdateHeartUI(currentPlayerHP));
            }
        }

        //プレイヤーのボムの数が減ったら、ボムのUIを減らしてフラッシュ発動
        if (currentRemainBomb < remainBomb)
        {
            DecreaseUI(ref bombUI, remainBomb - currentRemainBomb);
            remainBomb = currentRemainBomb;
            gameManager.Flash();
        }
        //プレイヤーのボムが増えたら、ボムのUIを増やす
        else if (currentRemainBomb > remainBomb)
        {
            IncreaseUI(ref bombUI, bomb, 110, 80, -180, currentRemainBomb - remainBomb);
            remainBomb = currentRemainBomb;
        }
    }

    /// <summary>
    /// プレハブのUIを、左上をアンカーとし、UISpace分間隔を開けながら生成するメソッド
    /// </summary>
    /// <param name="gameObjects">生成したゲームオブジェクトを格納する配列</param>
    /// <param name="gameObject">生成したいゲームオブジェクト</param>
    /// <param name="arrayLength">生成したい個数</param>
    /// <param name="UISpace">UI同士の間隔</param>
    /// <param name="x">画面左上からの、１つ目のX座標</param>
    /// <param name="y">画面左上からの、１つ目のY座標</param>
    void CreateUI(ref GameObject[] gameObjects,GameObject gameObject, int arrayLength, int UISpace, int x, int y, RectTransform parent)
    {
        Array.Resize(ref gameObjects, arrayLength);
        float space = UISpace;
        Vector3 UIPos = new Vector3(x, y, 0);

        for (int i = 0; i < arrayLength; i++)
        {
            gameObjects[i] = Instantiate(gameObject);

            //UIの配置と大きさの設定
            RectTransform objectRect = gameObjects[i].GetComponent<RectTransform>();
            objectRect.transform.SetParent(parent.transform);
            objectRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            objectRect.anchoredPosition = UIPos;

            UIPos.x += space;
        }
    }

    //外部からHPのUIの更新を行うためのメソッド
    public void CallUpdateHeartUI()
    {
        StartCoroutine(UpdateHeartUI(playerScript.playerHP));
    }


    /// <summary>
    /// 現在のHPによって、ハートのUIの更新するコルーチン
    /// </summary>
    /// <param name="currentPlayerHP">現在のHP</param>
    IEnumerator UpdateHeartUI(int currentPlayerHP)
    {
        int HPFullNumber = currentPlayerHP / 2;
        int HPHalfCheck = currentPlayerHP % 2;
        
        //配列の０～playerHeartまで、ハートのアニメをFullに
        //HPが奇数の場合、playerHeart+1のハートのアニメをHalfに
        //残りのハートのアニメをEmptyにする
        for (int i = 0; i < HPFullNumber; i++)
        {
            Animator heartAnime = heartUI[i].GetComponent<Animator>();
            heartAnime.SetTrigger("Full");
        }

        if (HPHalfCheck == 1)
        {
            Animator heartAnime = heartUI[HPFullNumber].GetComponent<Animator>();
            heartAnime.SetTrigger("Half");
            HPFullNumber += 1;
        }

        for (int i = HPFullNumber; i < heartUI.Length; i++)
        {
            Animator heartAnime = heartUI[i].GetComponent<Animator>();
            heartAnime.SetTrigger("Empty");
        }

        yield return null;

        playerHP = currentPlayerHP;
    }


    /// <summary>
    /// 表示しているUIを減らすメソッド
    /// </summary>
    /// <param name="gameObjects">減らしたいゲームオブジェクトが格納されている配列</param>
    /// <param name="number">減らす個数</param>
    void DecreaseUI(ref GameObject[] gameObjects, int number)
    {
        //配列の最後のゲームオブジェクトを消去
        for (int i = 0; i < number; i++)
        {
            if (gameObjects.Length == 0)
            {
                return;
            }
            int arrayLast = gameObjects.Length - 1;
            Destroy(bombUI[arrayLast]);
            Array.Resize(ref gameObjects, arrayLast);
        }
    }


    /// <summary>
    /// 表示しているUIを増やすメソッド
    /// </summary>
    /// <param name="gameObjects">増やしたいゲームオブジェクトが格納されている配列</param>
    /// <param name="gameObject">増やしたいゲームオブジェクト</param>
    /// <param name="UISpace">UI同士の間隔</param>
    /// <param name="x">画面左上からの、１つ目のX座標</param>
    /// <param name="y">画面左上からの、１つ目のY座標</param>
    /// <param name="number">増やしたい個数</param>
    void IncreaseUI(ref GameObject[] gameObjects, GameObject gameObject, int UISpace, int x, int y, int number)
    {
        //配列の最後にゲームオブジェクトを追加
        for (int i = 0; i < number; i++)
        {
            int arrayLast = gameObjects.Length + 1;
            float space = UISpace * arrayLast - UISpace;
            Vector3 UIPos = new Vector3(x + space, y, 0);

            Array.Resize(ref gameObjects, arrayLast);
            gameObjects[arrayLast - 1] = Instantiate(gameObject);
            RectTransform objectRect = gameObjects[arrayLast - 1].GetComponent<RectTransform>();
            RectTransform bombUIParent = gameManager.bombUITransform;
            objectRect.transform.SetParent(bombUIParent.transform);
            objectRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            objectRect.anchoredPosition = UIPos;
        }
    }
}
