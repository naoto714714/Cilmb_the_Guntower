using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// プレイヤーが持っている銃を管理するクラス
/// </summary>
public class GunManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    PrehubManager prehubManager;

    public GameObject gun1;
    public GameObject gun2;

    Gun gun1Script;
    Gun gun2Script;
    public int equipGunNumber = 1;

    public GameObject muzzleFlash1;
    public GameObject muzzleFlash2;

    Image reloadCircle;

    float timeCountUp;
    bool autoReloadCheck;


    void Awake()
    {
        prehubManager = gameManager.prehubManager;

        reloadCircle = gameManager.reloadCircle;

        StartCoroutine(StartSetUpEquipGun()); //装備中の武器をセットし、１フレーム開けてから残弾をセットする
        StartCoroutine(StartSetUpHoldGun()); //１フレーム開けてから控えの武器をセットする
    }


    void Update()
    {
        //右クリックで装備武器切り替え
        if (Input.GetMouseButtonDown(1))
        {
            GunChange();
        }

        if (autoReloadCheck) { return; }

        //武器を切り替えてから３秒経ったら控えの武器を自動リロード
        AutoReload();
    }

    /// <summary>
    /// １フレーム開けてから、装備中の武器のデータを読み込んで生成し、１フレーム開けてから残弾をセットするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator StartSetUpEquipGun()
    {
        //データを読み込んで銃を生成する
        int[] gunClassNumber = GameData.equipGunNumbers;
        GameObject[] _gun = new GameObject[] { prehubManager.SetGun(gunClassNumber[0]), prehubManager.SetGun(gunClassNumber[1]) };
        gun1 = Instantiate(_gun[0]);
        gun1.transform.SetParent(gameObject.transform);
        gun2 = Instantiate(_gun[1]);
        gun2.transform.SetParent(gameObject.transform);

        //生成した武器の初期設定をする
        gun1.SetActive(true);
        gun2.SetActive(false);

        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        gun1Script.enabled = true;
        gun2Script.enabled = true;

        gun1.transform.SetSiblingIndex(0);
        gun2.transform.SetSiblingIndex(1);

        muzzleFlash1 = gun1.transform.Find("MuzzleFlash").gameObject;
        muzzleFlash2 = gun2.transform.Find("MuzzleFlash").gameObject;

        gun1Script.Awake();
        gun2Script.Awake();

        yield return null;

        gun1Script.remainBullet = GameData.equipGunRemainBullets[0];
        gun2Script.remainBullet = GameData.equipGunRemainBullets[1];
    }


    /// <summary>
    /// １フレーム開けてから、控えの武器のデータを読み込んで生成し、残弾もセットするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator StartSetUpHoldGun()
    {
        if (GameData.holdGunNumbers[0] == 0) { yield break; } //控えの武器がないならブレイク

        yield return null;

        int i = 0; 

        //所持中の武器の一覧を表示する
        foreach (int number in GameData.holdGunNumbers)
        {
            if (number == 0) { yield break; } //控えの武器がそれ以上ないならブレイク

            //データを読み込んで銃を生成し、初期設定をする
            GameObject _gun = prehubManager.SetGun(number);
            GameObject gun = Instantiate(_gun);
            gun.transform.SetParent(gameObject.transform);
            gun.SetActive(false);
            gun.transform.SetSiblingIndex(i + 2); //装備中の武器が0番と１番だから、控えの武器は２番から

            Gun gunScript = gun.GetComponent<Gun>();
            gunScript.enabled = true;
            gunScript.Awake();

            gunScript.remainBullet = GameData.holdGunRemainBullets[i];

            i += 1;
        }
    }

    
    /// <summary>
    /// 装備する武器を切り替えるメソッド
    /// </summary>
    void GunChange()
    {
        reloadCircle.gameObject.SetActive(false);
        timeCountUp = 0;
        autoReloadCheck = false;

        //１番の武器を持っているなら２番の武器に切り替え
        if (equipGunNumber == 1)
        {
            equipGunNumber = 2;
            gun2.transform.position = gun1.transform.position;
            gun2.transform.rotation = gun1.transform.rotation;
            gun1.SetActive(false);
            gun2.SetActive(true);
            muzzleFlash1.SetActive(false);
            gun1Script.reloadTimeCountUP = 0;
        }
        //２番の武器を持っているなら１番の武器に切り替え
        else if (equipGunNumber == 2)
        {
            equipGunNumber = 1;
            gun1.transform.position = gun2.transform.position;
            gun1.transform.rotation = gun2.transform.rotation;
            gun1.SetActive(true);
            gun2.SetActive(false);
            muzzleFlash2.SetActive(false);
            gun2Script.reloadTimeCountUP = 0;
        }
    }


    /// <summary>
    /// ３秒たったら控えの武器を自動でリロードするメソッド
    /// </summary>
    void AutoReload()
    {
        timeCountUp += Time.deltaTime;

        if (timeCountUp < 3.0f) { return; }
        
        autoReloadCheck = true;

        if (equipGunNumber == 1)
        {
            float sumBullet = gun2Script.magazineBullet + gun2Script.remainBullet;
            if (sumBullet < gun2Script.magazineSize)
            {
                gun2Script.magazineBullet = sumBullet;
                gun2Script.remainBullet = 0;
            }
            else
            {
                gun2Script.magazineBullet = gun2Script.magazineSize;
                gun2Script.remainBullet -= gun2Script.useBulletCount;
            }
            gun2Script.useBulletCount = 0;
        }
        else if (equipGunNumber == 2)
        {
            float sumBullet = gun1Script.magazineBullet + gun1Script.remainBullet;
            if (sumBullet < gun1Script.magazineSize)
            {
                gun1Script.magazineBullet = sumBullet;
                gun1Script.remainBullet = 0;
            }
            else
            {
                gun1Script.magazineBullet = gun1Script.magazineSize;
                gun1Script.remainBullet -= gun1Script.useBulletCount;
                gun1Script.useBulletCount = 0;
            }
        }
    }
    
    /// <summary>
    /// 武器選択画面で装備中の武器を変えたら装備武器の初期設定をするメソッド
    /// </summary>
    public void GunSetUp()
    {
        foreach (Transform gun in gameObject.transform)
        {
            gun.gameObject.SetActive(false);
        }

        if (equipGunNumber == 1)
        {
            gun1.SetActive(true);
        }
        else
        {
            gun2.SetActive(true);
        }
        GunUIManager gunUIManager = gameManager.gunUIManager.GetComponent<GunUIManager>();
        gunUIManager.GunUISetUp();

        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        gun1.transform.SetSiblingIndex(0);
        gun2.transform.SetSiblingIndex(1);

        muzzleFlash1 = gun1.transform.Find("MuzzleFlash").gameObject;
        muzzleFlash2 = gun2.transform.Find("MuzzleFlash").gameObject;

        reloadCircle.gameObject.SetActive(false);
    }

    /// <summary>
    /// GameDataに現在の銃のデータを保存するメソッド
    /// 主にシーン間のデータ引き継ぎ用
    /// </summary>
    public void SetGunData()
    {
        int[] equipGunNumbers = new int[2];
        int[] holdGunNumbers = new int[20];
        int[] equipGunRemainBullets = new int[2];
        int[] holdGunRemainBullets = new int[20];

        int i = 0;
        int equipNumber = 2; //装備中の武器の数
        int holdNumber = 20; //控えの武器の数の最大値

        foreach (Transform gun in gameObject.transform)
        {
             Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;
            int remainBullet = (int)gunScript.remainBullet;

            //手持ちの武器のデータのセット
            if (i < equipNumber)
            {
                equipGunNumbers[i] = gunClassNumber; //武器の分類番号
                equipGunRemainBullets[i] = remainBullet; //武器の残弾
            }
            //控えの武器のデータのセット
            else if (i < (equipNumber + holdNumber))
            {
                int holdArrayNumber = i - equipNumber;
                holdGunNumbers[holdArrayNumber] = gunClassNumber; //武器の分類番号
                holdGunRemainBullets[holdArrayNumber] = remainBullet; //武器の残弾
            }
            i += 1;
        }

        GameData.equipGunNumbers = equipGunNumbers;
        GameData.equipGunRemainBullets = equipGunRemainBullets;
        GameData.holdGunNumbers = holdGunNumbers;
        GameData.holdGunRemainBullets = holdGunRemainBullets;
    }
}


