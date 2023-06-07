using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Calc;

//ボス戦開始時と終了時の演出を管理するクラス
public class StagingManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject bossStagingStart;
    [SerializeField] GameObject bossStaging;
    [SerializeField] string bossName;
    [SerializeField] TextMeshProUGUI bossStagingNameText;

    Transform UIManagerTransform;
    Slider bossHPSlider;
    TextMeshProUGUI bossNameText;

    KeyInputManager keyInputManager;
    UIManager uiManager;

    Transform cameraTransform;

    List<int> activeList = new List<int>();

    bool stagingStart = false;
    bool skip = false;
    bool dead = false;

    BGMManager BGMManager;
    AudioSource audioSourceBGM;
    SEManager SEManager;
    AudioSource audioSourceSE;


    void Start()
    {
        UIManagerTransform = gameManager.UIMGR.transform;
        keyInputManager = gameManager.keyInputManager;
        uiManager = gameManager.uiManager;
        cameraTransform = gameManager.cameraTransform;

        BGMManager = gameManager.BGMManager;
        audioSourceBGM = gameManager.audioSourceBGM;

        SEManager = gameManager.SEManager;
        audioSourceSE = gameManager.audioSourceSE;
    }

    void Update()
    {
        if (!stagingStart) { return; }
        if (skip) { return; }
        
        //ボス戦開始時の演出は左クリックでスキップできるようにする
        if (Input.GetMouseButtonDown(0))
        {
            skip = true;
            gameManager.bossAttackAnimeOff = true;
            EndStaging();
        }
    }

    /// <summary>
    /// ボス戦開始時の演出用のメソッド
    /// </summary>
    /// <param name="areaPos">演出をするステージの中心の座標</param>
    public void StagingBossArea(Vector2 areaPos)
    {
        stagingStart = true;
        
        //プレイヤーを動けないように
        keyInputManager.noControl = true;
        gameManager.noShoot = true;

        bossHPSlider = gameManager.bossHPSlider;
        bossNameText = gameManager.bossNameText;

        audioSourceBGM.Stop(); //BGMを一時停止（演出終了後にボスBGMに変更して流す）

        bossStagingNameText.text = "VS. " + bossName;
        
        //全てのUIを一時的に他の変数に格納して、非表示にする（演出終了後に、UIを再び表示するため）
        int i = 0;
        foreach (Transform child in UIManagerTransform)
        {
            if (child.gameObject.activeSelf)
            {
                activeList.Add(i);
                child.gameObject.SetActive(false);
            }
            child.gameObject.SetActive(false);
            i += 1;
        }

        StartCoroutine(MoveCameraToBoss(areaPos, 1)); //カメラを徐々にボスの方に動かす
    }


    /// <summary>
    /// カメラを徐々にボスの方に動かすコルーチン
    /// </summary>
    /// <param name="areaPos">カメラを動かす目的地</param>
    /// <param name="type">演出のタイプ</param>
    /// <returns></returns>
    IEnumerator MoveCameraToBoss(Vector2 areaPos, int type)
    {
        if (skip) { yield break; }
        Vector2 cameraPos = cameraTransform.position;

        Vector2 vector = Calculate.Vector(cameraPos, areaPos);

        int addNumber = 100;
        Vector2 addVector = new Vector2(vector.x / addNumber, vector.y / addNumber);

        //カメラを目的地の方へ動くように、ポジションを100分の１ずつ足していく
        for (int i = 0; i < addNumber; i++)
        {
            cameraTransform.position += new Vector3(addVector.x, addVector.y);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.0f);

        //演出のタイプが１なら、カメラをボスに向けたときに、攻撃アニメを再生する
        if (type == 1)
        {
            if (skip) { yield break; }
            gameManager.bossAttackAnime = true;

            yield return new WaitForSeconds(2.0f);

            StartCoroutine(ActiveBossStagingStart());
        }
    }


    /// <summary>
    /// パネル１を表示し、そのパネルを徐々に大きくするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveBossStagingStart()
    {
        if (skip) { yield break; }

        bossStagingStart.SetActive(true);
        bossStagingStart.transform.localScale = new Vector2(1, 1);

        AudioClip stagingSE = SEManager.SE_BossStaging;
        audioSourceSE.PlayOneShot(stagingSE);

        yield return new WaitForSeconds(0.5f);

        //UIの表示を徐々に大きくする
        for (int i = 1; i < 20; i++)
        {
            bossStagingStart.transform.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        bossStagingStart.SetActive(false);
        StartCoroutine(ActiveBossStaging());
    }

    /// <summary>
    /// パネル１を表示した後、パネル２をしばらく表示するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveBossStaging()
    {
        if (skip) { yield break; }

        bossStaging.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        EndStaging();
    }

    /// <summary>
    /// ボス戦開始の演出を終わるときのメソッド
    /// </summary>
    void EndStaging()
    {
        skip = true;
        
        //動けるように
        stagingStart = false;
        keyInputManager.noControl = false;
        
        bossStaging.SetActive(false);
        bossHPSlider.gameObject.SetActive(true);
        bossNameText.gameObject.SetActive(true);

        //非表示にしたUIを再び表示する
        int listLength = activeList.Count;
        for (int i = 0; i < listLength; i++)
        {
            GameObject activeObject = UIManagerTransform.GetChild(activeList[i]).gameObject;
            activeObject.SetActive(true);
        }
        uiManager.CallUpdateHeartUI(); //HPのUIは一度非アクティブにすると初期化されるから、ここでUIを更新する

        gameManager.bossMoveStart = true;
        Invoke("CanShoot", 0.6f); //演出が終わったあと、少しの間銃を撃てないように（スキップしたときに銃を撃ってしまうのを防ぐ）

        AudioClip bossBGM = BGMManager.BGM_Boss;
        audioSourceBGM.clip = bossBGM;
        audioSourceBGM.Play(); //演出が終わったらボスBGMを流す
    }

    /// <summary>
    /// プレイヤーが銃を撃てるにようにするメソッド
    /// </summary>
    void CanShoot()
    {
        gameManager.noShoot = false;
    }

    /// <summary>
    /// ボスを倒したときの演出のコルーチン
    /// </summary>
    /// <param name="boss">ボスのオブジェクト</param>
    /// <param name="explosion">爆発のプレハブ</param>
    /// <returns></returns>
    public IEnumerator DeadBoss(GameObject boss, GameObject explosion)
    {
        if (dead) { yield break; }

        dead = true;
        skip = false;

        //動けないように
        keyInputManager.noControl = true;
        gameManager.noShoot = true;
        
        audioSourceBGM.Stop(); //BGMを停止

        //ボスのポジションを取得し、カメラをボスの方に徐々に動かす
        Vector2 bossPos = boss.transform.position;
        StartCoroutine(MoveCameraToBoss(bossPos, 2)); 

        yield return new WaitForSeconds(3.0f);

        //ボスを非表示にして爆発させる
        //ここでボスをデストロイすると、このタイミングで宝箱が出現してしまう
        boss.GetComponent<Renderer>().enabled = false;
        explosion.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        //プレイヤーを動けるようにして、ボスをデストロイ
        Destroy(boss);
        keyInputManager.noControl = false;
        gameManager.noShoot = false;

        AudioClip exploreBGM = BGMManager.BGM_Explore;
        audioSourceBGM.clip = exploreBGM;
        audioSourceBGM.Play(); //演出が終わったら通常BGMを流す
    }
}