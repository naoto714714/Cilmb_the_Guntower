using UnityEngine;
using UnityEngine.Tilemaps;
using Calc;

/// <summary>
/// プレイヤーがエリア内に侵入したかどうかを判定し、侵入したらそのエリアをアクティブにするメソッド
/// </summary>
public class EnterCheck : MonoBehaviour
{
    /// <summary>
    /// 部屋がある方向、北：１，東：２，南：３、西：４
    /// </summary>
    [SerializeField] int directionNumber;
    [SerializeField] int activeRoadNumber;
    [SerializeField] bool bossArea;
    [SerializeField] bool shopArea;
    int activeAreaNumber;
    GameManager gameManager;
    MiniMap miniMap;
    StageManager stageManager;
    SEManager SEManager;

    Transform myTransform;
    bool myEnteredCheck = false;
    bool seCheck = false;
    bool isEnter = false;

    Transform parentArea;
    
    GameObject road;

    AudioClip roomOpenSE;
    AudioSource audioSourceSE;

    //ショップ用
    AudioClip exploreBGM;
    AudioClip shopBGM;
    AudioSource audioSourceBGM;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        Transform roadManagerTransform = gameManager.roadManagerTranform;
        miniMap = gameManager.miniMap;

        SEManager = gameManager.SEManager;
        audioSourceSE = gameManager.audioSourceSE;

        //アクティブロードナンバーによってアクティブにするロードを決定する
        foreach (Transform road in roadManagerTransform)
        {
            RoadNumber roadNumberScript = road.GetComponent<RoadNumber>();
            int roadNumber = roadNumberScript.roadnumber;
            if (activeRoadNumber == roadNumber)
            {
                this.road = road.gameObject;
                break;
            }
        }

        myTransform = gameObject.transform;

        parentArea = myTransform.parent.parent;

        stageManager = parentArea.GetComponent<StageManager>();
        activeAreaNumber = stageManager.areaNumber;

        roomOpenSE = SEManager.SE_RoomOpen;

        if (!shopArea) { return; }

        BGMManager BGMManager = gameManager.BGMManager;
        exploreBGM = BGMManager.BGM_Explore;
        shopBGM = BGMManager.BGM_Shop;
        audioSourceBGM = gameManager.audioSourceBGM;
    }

    /// <summary>
    /// プレイヤーが部屋に入ったかどうかを判定するメソッド
    /// </summary>
    void OnTriggerExit2D(Collider2D collision)
    {
        //１回侵入したら２回以上は判定しない、ショップは複数回判定
        if (myEnteredCheck && !shopArea) { return; }

        //侵入したのがプレイヤー以外ならリターン
        if (!collision.CompareTag("Player")) { return; }

        Vector2 vector = Calculate.Vector(myTransform.position, collision.transform.position);
        
        //プレイヤーが指定した方向に通り抜けた場合はエリアをアクティブ、逆に通り抜けた場合はロードをアクティブ
        switch (directionNumber)
        {
            case 1: //北
                if (vector.y > 0)
                {
                    AreaActive();
                }
                else if (vector.y < 0)
                {
                    RoadActive();
                }
                break;


            case 2: //東
                if (vector.x > 0)
                {
                    AreaActive();
                }
                else if (vector.x < 0)
                {
                    RoadActive();
                }
                break;

            case 3: //南
                if (vector.y < 0)
                {
                    AreaActive();
                }
                else if (vector.y > 0)
                {
                    RoadActive();
                }
                break;

            case 4: //西
                if (vector.x < 0)
                {
                    AreaActive();
                }
                else if (vector.x > 0)
                {
                    RoadActive();
                }
                break;
        }
    }

    /// <summary>
    /// エリア内の全てのオブジェクト、ドアを塞ぐブロックをアクティブにするメソッド
    /// </summary>
    void AreaActive()
    {
        //ショップならBGMを変え、撃てないようにする
        if (shopArea && !isEnter)
        {
            audioSourceBGM.Stop();
            audioSourceBGM.clip = shopBGM;
            audioSourceBGM.Play();
            gameManager.noShoot = true;
            isEnter = true;
        }

        if (stageManager.enteredCheck) { return; } //既に解放済みのエリアなら反応しない（ドアに立って戻ると２回解放されるのを防ぐ）
        if (stageManager.noChest) { return; } //宝箱がない部屋には反応しない（スタートエリア等）
        stageManager.enteredCheck = true;
        myEnteredCheck = true;
        RoomOpenSE();

        foreach (Transform childTransform in parentArea)
        {
            childTransform.gameObject.SetActive(true);

            //ステージ内の全てのドアオブジェクトの子オブジェクトに含まれる、ドアを塞ぐブロックもアクティブ
            if (childTransform.CompareTag("Door"))
            {
                Tilemap doorTile = childTransform.GetComponent<Tilemap>();
                doorTile.color = new Color(1, 1, 1, 1);
                Transform doorBlock = childTransform.Find("DoorBlock");
                doorBlock.gameObject.SetActive(true);
            }
        }
        
        //ボスエリアに侵入したとき、演出を再生する
        if (bossArea)
        {
            StagingManager stagingManager = gameManager.stagingManager;
            Vector2 areaPos = parentArea.localPosition;
            stagingManager.StagingBossArea(areaPos);
        }

        //ミニマップに開放したエリアを表示させ、戦闘中はミニマップを表示しない
        miniMap.ActiveArea(activeAreaNumber);
        miniMap.MapOff();
    }

    /// <summary>
    /// 道をアクティブにするメソッド
    /// </summary>
    void RoadActive()
    {
        //ショップから出たらBGMをもとに戻し、撃てるようにする
        if (shopArea && isEnter)
        {
            audioSourceBGM.Stop();
            audioSourceBGM.clip = exploreBGM;
            audioSourceBGM.Play();
            gameManager.noShoot = false;
            isEnter = false;
        }
        road.SetActive(true);
        miniMap.ActiveRoad(activeRoadNumber);
        RoomOpenSE();
        //ここにmyEnteredCheckを入れると、ドアで出入りするとステージが表示されなくなる
    }

    void RoomOpenSE()
    {
        //BGMは一度しかならないように
        //このようにしないと一度の出入りで大量に効果音が鳴る
        if (!seCheck)
        {
            audioSourceSE.PlayOneShot(roomOpenSE);
            seCheck = true;
        }
    }
}
