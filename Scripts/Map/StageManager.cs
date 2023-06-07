using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// エリア内の敵を倒し終わったら、ドアのブロックを消し、宝箱を出現させるクラス
/// </summary>
public class StageManager : MonoBehaviour
{
    PrehubManager prehubManager;
    Player playerScript;
    MiniMap minimapScript;

    Transform myTransform;

    GameObject chest;
    GameObject smoke;
    bool chestOpen = false;

    public int areaNumber;
    public bool noChest;
    public bool enteredCheck = false;
    /// <summary>
    /// 出現させる宝箱の数
    /// </summary>
    [SerializeField] int chestNumber;

    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        GameManager gameManager = gameMGR.GetComponent<GameManager>();
        playerScript = gameManager.playerScript;
        minimapScript = gameManager.miniMap;

        prehubManager = gameManager.prehubManager;

        myTransform = gameObject.transform;

        smoke = prehubManager.Chest_Smoke;
    }

    void Update()
    {
        if (noChest) { return; } //宝箱なしならリターン（スタートエリア等）
        if (!enteredCheck) { return; } //プレイヤーが侵入してないならリターン
        if (chestOpen) { return; } //宝箱が既に出現したならリターン

        //戦闘中は、プレイヤーの移動速度を通常に
        playerScript.walkSpeedAddition = 1.0f;

        //ステージ内に敵が一体でもいるならリターン
        foreach (Transform childTransform in myTransform)
        {
            if (childTransform.CompareTag("Enemy"))
            {
                return;
            }
        }

        //StageManagerの子オブジェクト名の中に、"Enemy"のタグが付くものが無くなったら宝箱出現
        //chestNumberの数だけ、間隔５ずつを開けて宝箱出現
        //addX = -5, 10, -15, 20, -25, …
        //posX = -5, 5, -10, 10, -15, …
        float posX = 0;
        float addX = 0;
        float chestInterval = -5;
        
        //chestNumberの数だけ宝箱出現
        for (int i = 0; i < chestNumber; i++)
        {
            SetChest();
            Vector3 appearPos = new Vector3(myTransform.position.x + posX, myTransform.position.y, myTransform.position.z);
            GameObject chestClone = Instantiate(chest, appearPos, Quaternion.identity);
            GameObject smokeClone = Instantiate(smoke, appearPos, Quaternion.identity);
            chestClone.transform.parent = myTransform;
            smokeClone.transform.parent = myTransform;

            addX *= -1;
            addX += chestInterval;
            chestInterval *= -1;
            posX += addX;
        }

        chestOpen = true;

        //戦闘が終わったら、プレイヤーの移動速度アップ（チュートリアルでないなら）
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            playerScript.walkSpeedAddition = 1.3f;
        }

        //戦闘が終わったら、ミニマップオン
        minimapScript.MapOn();

        foreach (Transform childTransform in myTransform)
        {
            //全てのドアの出入り口を開ける
            if (childTransform.CompareTag("Door"))
            {
                Transform doorBlock = childTransform.Find("DoorBlock");
                doorBlock.gameObject.SetActive(false);
            }

            //部屋に入ったかどうかの判定を無くす
            if (childTransform.CompareTag("EnterCheck"))
            {
                childTransform.gameObject.SetActive(false);
            }
        }
    }

    //出現させる宝箱をランダムに決定するメソッド
    void SetChest()
    {
        int randDecideChestType = Random.Range(1, 101);

        //59％で木の宝箱（アイテム１つ）
        if (randDecideChestType <= 59)
        {
            chest = prehubManager.Chest1_Wood_ItemSingle;
        }
        //20%で木の宝箱（アイテム２つ）
        else if (randDecideChestType <= 79)
        {
            chest = prehubManager.Chest2_Wood_ItemDouble;
        }
        //10%で青い宝箱（ランクBの銃）
        else if (randDecideChestType <= 89)
        {
            chest = prehubManager.Chest3_Blue;
        }
        //8%で赤い宝箱（ランクAの銃）
        else if (randDecideChestType <= 97)
        {
            chest = prehubManager.Chest4_Red;
        }
        //3%で黒い宝箱（ランクSの銃）　
        else
        {
            chest = prehubManager.Chest5_Black;
        }
    }
}
