using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

/// <summary>
/// 武器選択画面を管理するクラス
/// </summary>
public class GunSelectUIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    KeyInputManager keyInputManager;

    const int equipGunNumber = 2;
    const int holdGunNumber = 10;

    [SerializeField] Image[] equipGunImages = new Image[equipGunNumber];
    [SerializeField] Image[] holdGunImages = new Image[holdGunNumber];
    [SerializeField] TextMeshProUGUI gunNameText;
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] TextMeshProUGUI gunExplanation;
    [SerializeField] GunSelectUIArrow arrowUp;
    [SerializeField] GunSelectUIArrow arrowDown;

    GameObject[] holdGunsObj = new GameObject[holdGunNumber]; 

    Animator myAnime;
    public int animeNumber = 0;
    public int dragGunNumber = 0;
    public bool isDrag = false;
    bool wasNoControl = false;
    bool wasNoShoot = false;

    //ゲームマネージャから読み込む変数
    GunManager gunManager;

    //ガンマネージャから読み込む変数
    GameObject[] equipGuns = new GameObject[equipGunNumber];
    GameObject[] holdGuns = new GameObject[holdGunNumber];

    string[] equipGunNames = new string[equipGunNumber];
    string[] holdGunNames = new string[holdGunNumber];

    float[] equipRemainBullets = new float[equipGunNumber];
    float[] holdRemainBullets = new float[holdGunNumber];

    string[] equipGunExplanations = new string[equipGunNumber];
    string[] holdGunExplanations = new string[holdGunNumber];

    GameObject gunUIManager;
    GameObject miniMap;
    GameObject mapPoint;

    SEManager SEManager;
    AudioClip openSE;
    AudioClip closeSE;
    AudioClip cursorSE;
    AudioClip decideSE;
    AudioSource audioSource;
    bool SECheck;


    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GunSelectOpen;
        closeSE = SEManager.SE_GunSelectClose;
        cursorSE = SEManager.SE_GunSelectCursor;
        decideSE = SEManager.SE_GunSelectDecide;

        gunUIManager = gameManager.gunUIManager;
        miniMap = gameManager.miniMap.gameObject;
        mapPoint = gameManager.mapPoint;

        int i = 0;
        foreach (Image holdGun in holdGunImages)
        {
            holdGunsObj[i] = holdGun.gameObject;
            holdGunsObj[i].SetActive(false);
            i += 1;
        }
    }


    /// <summary>
    /// 武器選択画面を起動時のメソッド
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE); //銃選択画面を開いたときのSE

        wasNoControl = false;
        wasNoShoot = false;

        //動けないように
        Time.timeScale = 0;
        //既に動けない状態なら解除後も動けないように
        if (keyInputManager.noControl) { wasNoControl = true; }
        else { keyInputManager.noControl = true; }
        //既に撃てない状態なら解除後も撃てないように
        if (gameManager.noShoot) { wasNoShoot = true; }
        else { gameManager.noShoot = true; }

        //UIをいくつか非表示
        gunUIManager.SetActive(false);
        miniMap.SetActive(false);
        mapPoint.SetActive(false);

        gameObject.transform.SetAsLastSibling();
        gameManager.DisplayCursor(); //照準を消してカーソルを表示

        myAnime = gameObject.GetComponent<Animator>();

        gunManager = gameManager.gunManager;

        gunManager.muzzleFlash1.SetActive(false);
        gunManager.muzzleFlash2.SetActive(false);

        int i = 0;

        //所持中の武器の一覧を表示する
        foreach (Transform gun in gunManager.transform)
        {
            SpriteRenderer gunImage = gun.GetComponent<SpriteRenderer>();
            Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;

            //始めの２つは装備中の武器の欄に表示
            if (i < equipGunNumber)
            {
                equipGuns[i] = gun.gameObject;
                equipGunImages[i].sprite = gunImage.sprite;
                equipGunNames[i] = gunScript.gunName;
                equipRemainBullets[i] = gunScript.remainBullet;
                equipGunExplanations[i] = GunData.SetGunExplanation(gunClassNumber);
            }
            //始めの２つ以降は所持中の武器の欄に表示する
            else if(i < (equipGunNumber + holdGunNumber))
            {
                int holdArrayNumber = i - equipGunNumber;
                holdGuns[holdArrayNumber] = gun.gameObject;
                holdGunImages[holdArrayNumber].sprite = gunImage.sprite;
                holdGunNames[holdArrayNumber] = gunScript.gunName;
                holdRemainBullets[holdArrayNumber] = gunScript.remainBullet;
                holdGunExplanations[holdArrayNumber] = GunData.SetGunExplanation(gunClassNumber);
                holdGunsObj[holdArrayNumber].SetActive(true);
            }
            //所持武器が１０個を超えたら、矢印をオンにし、２ページ目にいけるようにする
            else
            {
                arrowDown.ActiveArrow();
                break;
            }
            i += 1;
        }
    }


    void Update()
    {
        myAnime.SetInteger("SelectUIStatus", animeNumber);
        
        if (isDrag) { return; } //ドラッグ中は閉じれないように
        //Tabキーを押されたら武器選択画面を非表示に
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameManager.DisplayAimImage(); //カーソルを消して照準を表示
            audioSource.PlayOneShot(closeSE); //銃選択画面を閉じたときの音

            //装備中の武器の欄にある武器を、装備するように切り替え、初期設定を行う
            gunManager.gun1 = equipGuns[0];
            gunManager.gun2 = equipGuns[1];
            gunManager.GunSetUp();

            //非表示にしたUIを表示
            gunUIManager.SetActive(true);
            miniMap.SetActive(true);
            mapPoint.SetActive(true);

            //武器選択画面を非表示にし、動けるようにする
            gameObject.SetActive(false);
            Time.timeScale = 1;
            if (!wasNoControl) { keyInputManager.noControl = false; }
            if (!wasNoShoot) { gameManager.noShoot = false; }
        }
    }

    /// <summary>
    /// 武器にカーソルを合わせたら、その武器の名前・残弾・説明文を表示するメソッド
    /// </summary>
    public void TextChange()
    {   
        //カーソルのSE、なにもないところにカーソルを合わせたらSECheckをfalseにする
        if (!SECheck)
        {
            audioSource.PlayOneShot(cursorSE);
            SECheck = true;
        }

        float remainBullet = 0;
        //装備中の武器にカーソルを合わせたら、その武器名と説明文を表示
        if (animeNumber > 0)
        {
            int arrayNumber = animeNumber - 1;
            gunNameText.text = holdGunNames[arrayNumber];
            remainBullet = holdRemainBullets[arrayNumber];
            gunExplanation.text = holdGunExplanations[arrayNumber];

        }
        //所持中の武器にカーソルが合ったら、その武器名と説明文を表示
        else if (animeNumber < 0)
        {
            int arrayNumber = (animeNumber * -1) - 1;
            gunNameText.text = equipGunNames[arrayNumber];
            remainBullet = equipRemainBullets[arrayNumber];
            gunExplanation.text = equipGunExplanations[arrayNumber];
        }

        //残弾の表示
        //残弾が2000発以上なら、∞と表示する
        if (remainBullet > 2000)
        {
            remainBulletText.text = "（残：∞ 発）";
        }
        //2000発未満なら残弾を表示
        else
        {
            remainBulletText.text = "（残：" + remainBullet + " 発）";
        }
    }

    /// <summary>
    /// 武器名、残弾、説明文を非表示にするメソッド
    /// </summary>
    public void TextNone()
    {
        SECheck = false;
        gunNameText.text = null;
        remainBulletText.text = null;
        gunExplanation.text = null;
    }

    /// <summary>
    /// 装備中の武器を切り替えるメソッド
    /// </summary>
    /// <param name="equipGunNumber">現在装備している武器の番号</param>
    /// <param name="holdGunNumber">装備したい武器の番号</param>
    public void EquipGunChange(int equipGunNumber, int holdGunNumber)
    {
        audioSource.PlayOneShot(decideSE); //武器を入れ替えたときのSE

        int equipArrayNumber = equipGunNumber * -1 - 1;

        //現在装備している武器の設定を一時的に保存（入れ替えるため）
        GameObject tmpObject = equipGuns[equipArrayNumber];
        Sprite tmpSprite = equipGunImages[equipArrayNumber].sprite;
        string tmpGunNameText = equipGunNames[equipArrayNumber];
        float tmpRemainBulletText = equipRemainBullets[equipArrayNumber];
        string tmpGunExplanationText = equipGunExplanations[equipArrayNumber];

        //装備中の武器１と装備中の武器２の入れ替えを行う場合
        if (holdGunNumber < 0)
        {
            int holdArrayNumber = holdGunNumber * -1 - 1;
            //オブジェクトの判定を入れ替え
            equipGuns[equipArrayNumber] = equipGuns[holdArrayNumber];
            equipGuns[holdArrayNumber] = tmpObject;

            //武器の画像を入れ替え
            equipGunImages[equipArrayNumber].sprite = equipGunImages[holdArrayNumber].sprite;
            equipGunImages[holdArrayNumber].sprite = tmpSprite;

            //残弾を入れ替え
            equipRemainBullets[equipArrayNumber] = equipRemainBullets[holdArrayNumber];
            equipRemainBullets[holdArrayNumber] = tmpRemainBulletText;

            //武器名を入れ替え
            equipGunNames[equipArrayNumber] = equipGunNames[holdArrayNumber];
            equipGunNames[holdArrayNumber] = tmpGunNameText;

            //説明文を入れ替え
            equipGunExplanations[equipArrayNumber] = equipGunExplanations[holdArrayNumber];
            equipGunExplanations[holdArrayNumber] = tmpGunExplanationText;
        }
        //所持中の武器と装備中の武器の入れ替えを行う場合
        else
        {
            int holdArrayNumber = holdGunNumber - 1;
            //オブジェクトの判定を入れ替え
            equipGuns[equipArrayNumber] = holdGuns[holdArrayNumber];
            holdGuns[holdArrayNumber] = tmpObject;

            //武器の画像を入れ替え
            equipGunImages[equipArrayNumber].sprite = holdGunImages[holdArrayNumber].sprite;
            holdGunImages[holdArrayNumber].sprite = tmpSprite;

            //武器の残弾を入れ替え
            equipRemainBullets[equipArrayNumber] = holdRemainBullets[holdArrayNumber];
            holdRemainBullets[holdArrayNumber] = tmpRemainBulletText;

            //武器名を入れ替え
            equipGunNames[equipArrayNumber] = holdGunNames[holdArrayNumber];
            holdGunNames[holdArrayNumber] = tmpGunNameText;

            //説明文を入れ替え
            equipGunExplanations[equipArrayNumber] = holdGunExplanations[holdArrayNumber];
            holdGunExplanations[holdArrayNumber] = tmpGunExplanationText;
        }
    }

    void OffHoldGuns()
    {
        foreach (GameObject obj in holdGunsObj)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < holdGunNumber; i++)
        {
            holdGuns[i] = null;
            holdGunImages[i].sprite = null;
            holdGunNames[i] = null;
            holdRemainBullets[i] = 0;
            holdGunExplanations[i] = null;
        }
    }

    public void Page1()
    {
        arrowUp.NormalArrow();
        arrowDown.ActiveArrow();
        OffHoldGuns();

        int i = 0;

        //所持中の武器の一覧を表示する
        foreach (Transform gun in gunManager.transform)
        {
            if (i < equipGunNumber)
            {
                i += 1;
                continue;
            }
            SpriteRenderer gunImage = gun.GetComponent<SpriteRenderer>();
            Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;

            //1ページ目に収まらない武器は２ページ目に
            if (i < (equipGunNumber + holdGunNumber))
            {
                int holdArrayNumber = i - equipGunNumber;
                holdGuns[holdArrayNumber] = gun.gameObject;
                holdGunImages[holdArrayNumber].sprite = gunImage.sprite;
                holdGunNames[holdArrayNumber] = gunScript.gunName;
                holdRemainBullets[holdArrayNumber] = gunScript.remainBullet;
                holdGunExplanations[holdArrayNumber] = GunData.SetGunExplanation(gunClassNumber);
                holdGunsObj[holdArrayNumber].SetActive(true);
            }
            i += 1;
        }
    }

    public void Page2()
    {
        arrowUp.ActiveArrow();
        arrowDown.NormalArrow();
        OffHoldGuns();

        int i = 0;

        //所持中の武器の一覧を表示する
        foreach (Transform gun in gunManager.transform)
        {
            if (i < equipGunNumber + holdGunNumber)
            {
                i += 1;
                continue;
            }
            SpriteRenderer gunImage = gun.GetComponent<SpriteRenderer>();
            Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;

            //1ページ目に収まらない武器は２ページ目に
            if (i < (equipGunNumber + holdGunNumber + holdGunNumber))
            {
                int holdArrayNumber = i - (equipGunNumber + holdGunNumber);
                holdGuns[holdArrayNumber] = gun.gameObject;
                holdGunImages[holdArrayNumber].sprite = gunImage.sprite;
                holdGunNames[holdArrayNumber] = gunScript.gunName;
                holdRemainBullets[holdArrayNumber] = gunScript.remainBullet;
                holdGunExplanations[holdArrayNumber] = GunData.SetGunExplanation(gunClassNumber);
                holdGunsObj[holdArrayNumber].SetActive(true);
            }
            i += 1;
        }
    }
}
