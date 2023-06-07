using UnityEngine;
using Calc;

/// <summary>
/// アイテムを管理するクラス
/// </summary>
public class Item : MonoBehaviour
{
    GameManager gameManager;
    SEManager SEManager;
    [SerializeField] int itemClassNumber;
    [SerializeField] bool isShopItem;
    [SerializeField] int price;
    [SerializeField] GameObject priceCanvas;

    Transform myTransform;
    SpriteRenderer mySpriteRenderer;

    Player playerScript;
    Transform gunManagerTransform;
    GunManager gunManagerScript;
    Material outline;
    Material defaultMaterial;

    public bool canGet = false;
    bool isInRange;
    float canGetRange = 1.5f; //アイテムを取得可能な範囲

    AudioClip itemGetSE;
    AudioClip itemCantGetSE;
    AudioSource audioSource;

    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myTransform = gameObject.transform;
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        playerScript = gameManager.playerScript;
        if (gameManager.gunManager != null)
        {
            gunManagerTransform = gameManager.gunManager.gameObject.transform;
            gunManagerScript = gameManager.gunManager;
        }
        outline = gameManager.outline;
        defaultMaterial = gameManager.defaultMaterial;

        itemGetSE = SEManager.SE_ItemGet;
        
        //ダイヤモンドを入手したときは専用の効果音にする
        if (itemClassNumber == 10)
        {
            itemGetSE = SEManager.SE_GetDiamond;
        }

        //ショップアイテムの場合は取得可能範囲を広げ、効果音を変える
        if (isShopItem)
        {
            itemGetSE = SEManager.SE_Buy;
            itemCantGetSE = SEManager.SE_CantBuy;
            canGetRange = 1.8f;
        }
    }


    void Update()
    {
        Vector3 myPos = myTransform.position;
        Vector3 playerPos = gameManager.playerPos;

        if (!canGet) { return; }　//取得不可能な場合はリターン

        if (Calculate.Distance(myPos, playerPos) > canGetRange)//プレイヤーが取得可能範囲外の場合はリターン
        {
            if (isInRange)
            {
                isInRange = false;
                mySpriteRenderer.material = defaultMaterial;
                if (isShopItem) { priceCanvas.SetActive(false); }
            }
            return;
        }

        //プレイヤーが取得可能範囲にいる場合
        //プレイヤーが取得可能範囲内にいるとき、アイテムに白フチを付け、Eキーで取得可能にする
        if (!isInRange)
        {
            isInRange = true;
            mySpriteRenderer.material = outline;
            if (isShopItem) { priceCanvas.SetActive(true); } //ショップアイテムなら説明文を表示
        }
        
        //Eキーでアイテム取得
        if (Input.GetKeyDown(KeyCode.E))//ショップアイテムなら所持コインから価格を引き、説明文を非表示
        {
            if (isShopItem)
            {
                if (playerScript.holdCoin < price)
                {
                    audioSource.PlayOneShot(itemCantGetSE);
                    return;
                }
                playerScript.holdCoin -= price;
                priceCanvas.SetActive(false);
            }
            audioSource.PlayOneShot(itemGetSE); //取得の効果音を鳴らす

            //アイテムが銃なら、その銃をgunManagerオブジェクトの子オブジェクトにする
            if (gameObject.CompareTag("Gun"))
            {
                Gun gunScript = gameObject.GetComponent<Gun>();
                gunScript.enabled = true;
                Item itemScript = gameObject.GetComponent<Item>();
                itemScript.enabled = false;

                myTransform.parent = gameManager.gunManager.transform;
                mySpriteRenderer.material = defaultMaterial;
                gameObject.SetActive(false);
            }
            //銃以外なら、アイテムクラスナンバーによって処理を変える
            else
            {
                switch (itemClassNumber)
                {
                    //1ならHPを２回復、2ならHPを1回復
                    case 1:
                    case 2:
                        GetHeart(itemClassNumber);
                        break;

                    //3ならボムを１つ増やす
                    case 3:
                        GetBomb();
                        break;
                    
                    //4なら、所持中の全ての武器の残弾を少し回復
                    //5なら、装備中の武器の残弾を少し回復
                    //6なら、装備中の武器の残弾を完全に回復
                    case 4:
                    case 5:
                    case 6:
                        GetBulletBox(itemClassNumber);
                        break;

                    //7なら、コインを少し獲得
                    //8なら、コインをまあまあ獲得
                    //9なら、コインを大量に獲得
                    case 7:
                    case 8:
                    case 9:
                        GetCoin(itemClassNumber);
                        break;
                    //10なら、ゲームクリア演出
                    case 10:
                        gameManager.CallClearGame();
                        break;
                }
                gameObject.SetActive(false);
                Destroy(gameObject,2.0f); //アイテム落下中に取得するとエラーになるから、時間差で破壊する
            }
        }
    }


    /// <summary>
    /// ハートを取得したときのメソッド
    /// </summary>
    /// <param name="itemClassNumber">アイテム番号</param>
    void GetHeart(int itemClassNumber)
    {
        switch (itemClassNumber)
        {
            //アイテム番号が１なら、２回復
            case 1:
                playerScript.playerHP += 2;
                break;

            //アイテム番号が２なら、１回復
            case 2:
                playerScript.playerHP += 1;
                break;
        }
    }


    /// <summary>
    /// ボムを取得したときのメソッド
    /// </summary>
    void GetBomb()
    {
        //プレイヤーのボムを１つ増やす
        playerScript.remainBomb += 1;
    }


    /// <summary>
    /// 弾薬箱を取得したときのメソッド
    /// </summary>
    /// <param name="itemClassNumber">アイテム番号</param>
    void GetBulletBox(int itemClassNumber)
    {
        Gun gunScript = null;
        const float bitBulletRatio = 0.3f; //最大の残弾*0.3の弾を取得

        //現在装備している銃のスクリプトを取得
        switch (gunManagerScript.equipGunNumber)
        {
            case 1:
                gunScript = gunManagerScript.gun1.GetComponent<Gun>();
                break;

            case 2:
                gunScript = gunManagerScript.gun2.GetComponent<Gun>();
                break;
        }

        //アイテム番号によって処理を変える
        switch (itemClassNumber)
        {   
            //4なら、所持中の全ての銃の残弾を少し回復
            case 4:
                foreach (Transform guns in gunManagerTransform)
                {
                    gunScript = guns.GetComponent<Gun>();
                    gunScript.remainBullet += (int)(gunScript.maxBullet * bitBulletRatio);
                    if (gunScript.remainBullet > gunScript.maxBullet)
                    {
                        gunScript.remainBullet = gunScript.maxBullet;
                    }
                }
                break;

            //5なら、装備中の武器の残弾を少し回復
            case 5:
                gunScript.remainBullet += (int)(gunScript.maxBullet * bitBulletRatio);
                break;

            //6なら、装備中の武器の残弾を完全に回復
            case 6:
                gunScript.remainBullet = gunScript.maxBullet;
                break;
        }

        //残弾が上限を超えないようにする
        if(gunScript.remainBullet > gunScript.maxBullet)
        {
            gunScript.remainBullet = gunScript.maxBullet;
        }
    }


    /// <summary>
    /// コインを取得したときのメソッド
    /// </summary>
    /// <param name="itemClassNumber">アイテム番号</param>
    void GetCoin(int itemClassNumber)
    {
        const int bronzeCoinNumber = 3; //ブロンズコインなら3枚獲得
        const int silverCoinNumber = 10; //シルバーコインなら10枚獲得
        const int goldCoinNumber = 30; //ゴールドコインなら30枚獲得
        switch (itemClassNumber)
        {
            case 7:
                playerScript.holdCoin += bronzeCoinNumber;
                break;

            case 8:
                playerScript.holdCoin += silverCoinNumber;
                break;

            case 9:
                playerScript.holdCoin += goldCoinNumber;
                break;
        }
    }
}

