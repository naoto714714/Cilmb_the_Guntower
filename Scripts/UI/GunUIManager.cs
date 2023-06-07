using UnityEngine;
using TMPro;
using Data;

//装備中の武器を表示するUIを管理するクラス
public class GunUIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] SpriteRenderer gunImage;
    [SerializeField] TextMeshProUGUI magazineBulletText;
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] TextMeshProUGUI gunName1Text;
    [SerializeField] TextMeshProUGUI gunName2Text;

    Animator myAnime;

    //ゲームマネージャから読み込む変数
    GunManager gunManager;

    //ガンマネージャから読み込む変数
    int equipGunNumber;
    float magazineBullet;
    float remainBullet;
    int currentEquipGunNumber;
    float currentMagazineBullet;
    float currentRemainBullet;

    //gun1, gun2のコンポーネントを入れる変数
    Sprite gun1Sprite;
    Sprite gun2Sprite;
    Gun gun1Script;
    Gun gun2Script;


    void Start()
    {
        myAnime = gameObject.GetComponent<Animator>();

        gunManager = gameManager.gunManager;

        GameObject gun1 = gunManager.gun1;
        GameObject gun2 = gunManager.gun2;
        gun1Sprite = gun1.GetComponent<SpriteRenderer>().sprite;
        gun2Sprite = gun2.GetComponent<SpriteRenderer>().sprite;
        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        equipGunNumber = gunManager.equipGunNumber;

        gunName1Text.text = gun1Script.gunName;
        gunName2Text.text = gun2Script.gunName;

        gunImage.sprite = gun1Sprite;

        myAnime.SetBool("GunAnimeChange", false);
    }


    void Update()
    {  
        //現在の装備中の武器番号を常に取得
        currentEquipGunNumber = gunManager.equipGunNumber;

        //装備中の武器の番号によってマガジンの弾数と残弾数の変数を切り替える
        //UI上で左の武器が１番、右の武器が２番
        SetCurrentBullet();

        //装備中の武器の番号が変わったら、UIのアニメ、画像、マガジンの弾数、残弾数を切り替え
        if (currentEquipGunNumber != equipGunNumber)
        {
            EquipGunNumberChange();
        }

        //マガジンの弾数が変わったらUIのマガジンの弾数テキスト変更
        if (currentMagazineBullet != magazineBullet)
        {
            MagazineBulletTextChange();
        }

        //残弾数が変わったらUIの残弾数テキスト変更
        //残弾数2000以上は∞扱い
        if (currentRemainBullet != remainBullet)
        {
            RemainBulletTextChange();
        }

        //ここにアニメ切り替えを入れないと、2を装備中にGunSelectUIManagerで武器を変えたときアニメがおかしくなる
        if (equipGunNumber == 1)
        {
            myAnime.SetBool("GunAnimeChange", false);
        }
        else if (equipGunNumber == 2)
        {
            myAnime.SetBool("GunAnimeChange", true);
        }
    }


    /// <summary>
    /// 装備中の武器の番号によってマガジンの弾数と残弾数の変数を切り替えるメソッド
    /// </summary>
    void SetCurrentBullet()
    {
        //１番を装備中なら、１番の武器のマガジンの弾数と残弾数を表示
        if (equipGunNumber == 1)
        {
            currentMagazineBullet = gun1Script.magazineBullet;
            currentRemainBullet = gun1Script.remainBullet;
        }
        //２番を装備中なら、２番の武器のマガジンの弾数と残弾数を表示
        else if (equipGunNumber == 2)
        {
            currentMagazineBullet = gun2Script.magazineBullet;
            currentRemainBullet = gun2Script.remainBullet;
        }
    }


    /// <summary>
    /// 装備中の武器の番号が変わったら、画像、マガジンの弾数、残弾数を切り替えるメソッド
    /// </summary>
    void EquipGunNumberChange()
    {
        equipGunNumber = currentEquipGunNumber;
        
        //装備中の武器が１なら１の武器の画像、マガジンの弾数、残弾数を表示
        if (equipGunNumber == 1)
        {
            gunImage.sprite = gun1Sprite;
            magazineBullet = gun1Script.magazineBullet;
            remainBullet = gun1Script.remainBullet;
        }
        //装備中の武器が２なら２の武器の画像、マガジンの弾数、残弾数を表示
        else if (equipGunNumber == 2)
        {
            gunImage.sprite = gun2Sprite;
            magazineBullet = gun2Script.magazineBullet;
            remainBullet = gun2Script.remainBullet;
        }
    }


    /// <summary>
    /// マガジンの弾数が変わったらUIのマガジンの弾数テキスト変更するメソッド
    /// </summary>
    void MagazineBulletTextChange()
    {
        magazineBullet = currentMagazineBullet;
        magazineBulletText.text = magazineBullet.ToString();
    }


    /// <summary>
    /// 残弾数が変わったらUIの残弾数テキスト変更するメソッド
    /// </summary>
    void RemainBulletTextChange()
    {
        remainBullet = currentRemainBullet;
        //残弾が2000以上の場合は∞と表示
        if (remainBullet > 2000)
        {
            remainBulletText.text = "∞";
        }
        else
        {
            remainBulletText.text = remainBullet.ToString();
        }
    }

    /// <summary>
    /// 武器のUIの切り替えをまとめて行うメソッド（外部からの呼び出し用）
    /// </summary>
    public void GunUISetUp()
    {
        GameObject gun1 = gunManager.gun1;
        GameObject gun2 = gunManager.gun2;
        gun1Sprite = gun1.GetComponent<SpriteRenderer>().sprite;
        gun2Sprite = gun2.GetComponent<SpriteRenderer>().sprite;
        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        currentEquipGunNumber = gunManager.equipGunNumber;

        gunName1Text.text = gun1Script.gunName;
        gunName2Text.text = gun2Script.gunName;

        EquipGunNumberChange();
        MagazineBulletTextChange();
        RemainBulletTextChange();
    }

}
