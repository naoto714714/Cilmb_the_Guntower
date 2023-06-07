using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

/// <summary>
/// 宝箱の中身の設定と、宝箱を開けたときにアイテムを出現させるクラス
/// </summary>
public class Chest : MonoBehaviour
{
    /// <summary>
    /// 宝箱のタイプによって選ぶ中身を変える
    /// </summary>
    [SerializeField] int chestTypeNumber;
    /// <summary>
    /// 落下をスローにする
    /// </summary>
    [SerializeField] bool appearSlow;

    GameManager gameManager;
    SEManager SEManager;
    PrehubManager prehubManager;

    Transform myTransform;
    Animator myAnime;
    SpriteRenderer mySpriteRenderer;

    float canOpenRange = 2;//宝箱を開けられる範囲
    int itemAppearCount = 1; //宝箱の中身の数

    Material outline;
    Material defaultMaterial;

    GameObject[] chestContent = new GameObject[2];
    bool openCheck = false;

    AudioClip chestAppearSE;
    AudioClip chestOpenSE;
    AudioSource audioSource;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        prehubManager = gameManager.prehubManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myTransform = gameObject.transform;
        myAnime = gameObject.GetComponent<Animator>();
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        outline = gameManager.outline; //白フチ
        defaultMaterial = gameManager.defaultMaterial;

        chestAppearSE = SEManager.SE_ChestAppear;
        chestOpenSE = SEManager.SE_ChestOpen;
        audioSource.PlayOneShot(chestAppearSE);

        //チェストタイプナンバーによって宝箱の中身を変える
        switch (chestTypeNumber)
        {
            //1,2はアイテムが出る宝箱、2はアイテムが２つ出現
            case 1:
            case 2:
                if (chestTypeNumber == 2)
                {
                    itemAppearCount = 2;
                }

                chestContent = SetChestContentItem(itemAppearCount);
                break;

            //3はBランクの武器
            case 3:
                {
                    chestContent[0] = SetChestContentGun(ref ItemAndGunData.gunsListRankB);
                    break;
                }

            //4はAランクの武器
            case 4:
                {
                    chestContent[0] = SetChestContentGun(ref ItemAndGunData.gunsListRankA);
                    break;
                }

            //5はSランクの武器
            case 5:
                {
                    chestContent[0] = SetChestContentGun(ref ItemAndGunData.gunsListRankS);
                    break;
                }

            case 100: //ゴール用　ダイヤモンド
                {
                    chestContent[0] = prehubManager.Item10_Diamond;
                    canOpenRange = 3; //ダイヤモンドの宝箱は大きいから開けられる範囲も広くする
                    break;
                }

            case 101: //チュートリアル用　グレネードガン
                {
                    chestContent[0] = prehubManager.Gun_RankA_1_GrenadeGun;
                    break;
                }

            case 102: //チュートリアル用　ボム２つ
                {
                    itemAppearCount = 2;
                    chestContent[0] = prehubManager.Item3_Bomb;
                    chestContent[1] = prehubManager.Item3_Bomb;
                    break;
                }

            case 103: //チュートリアル用　ハート２つ
                {
                    itemAppearCount = 2;
                    chestContent[0] = prehubManager.Item1_Heart_Full;
                    chestContent[1] = prehubManager.Item1_Heart_Full;
                    break;
                }
        }
    }


    void Update()
    {
        if (openCheck) { return; } //宝箱が開いたらUpdateメソッド停止

        Vector3 myPos = myTransform.position;
        Vector3 playerPos = gameManager.playerPos;

        mySpriteRenderer.material = defaultMaterial;

        //プレイヤーがcanOpenRange内にいるか判定し、いたら白フチをつける
        if (Calculate.Distance(myPos, playerPos) > canOpenRange) { return; }

        mySpriteRenderer.material = outline;

        //宝箱の近くにいるとき、Eキーを押されたら宝箱を開けてアイテム出現
        if (Input.GetKey(KeyCode.E))
        {
            ChestOpen();
        }
    }

    /// <summary>
    /// アイテムの出現確率を考慮し、宝箱の中身のアイテムを決めるメソッド
    /// </summary>
    /// <param name="itemAppearCount">アイテムの個数</param>
    /// <returns>宝箱の中身のアイテム</returns>
    GameObject[] SetChestContentItem(int itemAppearCount)
    {
        GameObject[] chestContents = new GameObject[] { null, null };

        for (int i = 0; i < itemAppearCount; i++)
        {
            float sumProbability = 0; //累積確率
            int rand = Random.Range(1, 101); //１～１００の間の乱数

            //乱数が累積確率以下になったとき、その時のゲームオブジェクトがchestContentに入れられる
            foreach (KeyValuePair<int, int> item in ItemAndGunData.itemDict)
            {
                sumProbability += item.Value;

                if (rand > sumProbability) { continue; }

                chestContents[i] = prehubManager.SetItem(item.Key);
                break;
            }
        }

        return chestContents;
    }

    /// <summary>
    /// 各銃の出現確率は等確率で、宝箱の中身の銃を決めるメソッド
    /// </summary>
    /// <param name="gunsList">銃の分類番号をまとめたリスト</param>
    /// <returns>宝箱の中身の銃</returns>
    GameObject SetChestContentGun(ref List<int> gunsList)
    {
        GameObject chestContent = null;
        float sumProbability = 0; //累積確率
        int rand = Random.Range(1,101); //１～１００の間の乱数

        float eachProbablity = 100.0f / gunsList.Count; //各銃の出現確率（等確率）

        //乱数が累積確率以下になったとき、その時のゲームオブジェクトがchestContentに入れられる
        foreach (int gunClassNumber in gunsList)
        {
            sumProbability += eachProbablity;

            if (rand > sumProbability) { continue; }

            chestContent = prehubManager.SetGun(gunClassNumber);
            gunsList.Remove(gunClassNumber);

            break;
        }

        //既に全種類の銃を持っている場合、コインに置き換える
        if (gunsList.Count == 0)
        {
            chestContent = prehubManager.Item8_Coin_Silver;
        }

        return chestContent;
    }


    /// <summary>
    /// 宝箱が開かれたとき、宝箱が開いたアニメーションに変更し、アイテムを出現させるメソッド
    /// </summary>
    void ChestOpen()
    {
        //アニメを開いたアニメに、白フチをなくす、効果音再生
        myAnime.SetTrigger("Open");
        mySpriteRenderer.material = defaultMaterial;
        audioSource.PlayOneShot(chestOpenSE);

        openCheck = true;

        int directionX = 0; //アイテムが出現する方向（０は真上）
        for (int i = 0; i < itemAppearCount; i++)
        {
            GameObject itemClone = Instantiate(chestContent[i], myTransform.position, Quaternion.identity);
            itemClone.transform.parent = myTransform;

            //アイテムが２個出現するなら、１個目を右側に、２個目を左側に出現させる
            if (itemAppearCount == 2 && i == 0)
            {
                directionX = 1;
            }
            else if (itemAppearCount == 2 && i == 1)
            {
                directionX = -1;
            }
            StartCoroutine(ItemAppear(itemClone.transform, directionX));
        }
    }
    
    /// <summary>
    /// 宝箱からアイテムが飛び出すように出現させるコルーチン、アイテム出現直後は取得不可
    /// </summary>
    /// <param name="itemTransform">選ばれた宝箱の中身のトランスフォーム</param>
    /// <param name="directionX">飛び出す方向（０は真上）</param>
    IEnumerator ItemAppear(Transform itemTransform, int directionX)
    {
        const float itemAppearMoveX = 1.5f; //アイテムが移動するX座標
        const float itemFallMoveY = 2.0f; //アイテムが移動するY座標
        float itemAppearUpSpeed = 2.0f; //アイテム上昇速度
        float itemStopTime = 0.3f; //空中での停止時間
        float itemFallDownSpeed = 1.6f; //アイテム下降速度
        float elapsedTime = 0; //経過時間

        if (appearSlow)
        {
            itemAppearUpSpeed = 0.8f;
            itemStopTime = 1.8f;
            itemFallDownSpeed = 0.4f;
        }

        //directionXの方向に、アイテムを山なりに移動させる
        //宝箱の位置から(directionX, 1.5)を目指し、徐々に移動する
        for (float i = 0; i <= itemAppearMoveX;)
        {
            elapsedTime += Time.deltaTime;
            i = itemAppearMoveX * itemAppearUpSpeed * elapsedTime;
            //y = -2/3 * (x - 3/2)^2 + 1  → 原点を(1.5, 1)とし、(0, 0)を通る二次関数
            double PosY = (-2.0 / 3.0) * (i - 3.0 / 2.0) * (i - 3.0 / 2.0) + 1;
            itemTransform.localPosition = new Vector3(i * directionX, (float)PosY, -1);
            yield return null;
        }

        //(directionX, 1.5)に移動したら、少し停止し、取得可能にする
        yield return new WaitForSeconds(itemStopTime);

        Item itemScript = itemTransform.gameObject.GetComponent<Item>();
        itemScript.canGet = true;
        float x = itemTransform.localPosition.x;
        float y = itemTransform.localPosition.y;
        elapsedTime = 0;

        //少し停止後、その場から徐々に落下する（宝箱のy座標-1まで）
        for (float i = 0; i <= itemFallMoveY;)
        {
            elapsedTime += Time.deltaTime;
            i = itemAppearMoveX * itemFallDownSpeed * elapsedTime;
            itemTransform.localPosition = new Vector3(x, y - i, -1);
            yield return null;
        }

        //自分自身を削除（スクリプトのみ）
        Destroy(gameObject.GetComponent<Chest>());
    }

}
