using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Calc;

/// <summary>
/// プレイヤーが通り抜けたとき、画面を遷移させるクラス
/// </summary>
public class StageTransition : MonoBehaviour
{
    /// <summary>
    /// 部屋がある方向、北：１，東：２，南：３、西：４
    /// </summary>
    [SerializeField] int directionNumber;
    /// <summary>
    /// 画面遷移先を指定
    /// </summary>
    [SerializeField] int transitionStageNumber;

    GameManager gameManager;
    SEManager SEManager;

    Transform myTransform;

    AudioClip stairsSE;
    AudioSource audioSourceSE;
    AudioSource audioSourceBGM;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        SEManager = gameManager.SEManager;
        audioSourceSE = gameManager.audioSourceSE;
        audioSourceBGM = gameManager.audioSourceBGM;

        myTransform = gameObject.transform;

        stairsSE = SEManager.SE_Stairs;
    }

    /// <summary>
    /// プレイヤーが通り抜けたら画面遷移するメソッドを呼ぶメソッド
    /// </summary>
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Vector2 vector = Calculate.Vector(myTransform.position, collision.transform.position);

        //プレイヤーが指定した方向に通り抜けた場合にBlackOutメソッドを呼ぶ
        switch (directionNumber)
        {
            case 1: //北
                if (vector.y > 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;


            case 2: //東
                if (vector.x > 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;

            case 3: //南
                if (vector.y < 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;

            case 4: //西
                if (vector.x < 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;
        }
    }


    /// <summary>
    /// 徐々に画面を暗転し、画面を遷移するメソッドを呼ぶコルーチン
    /// </summary>
    IEnumerator BlackOut()
    {
        //ゲーム終了メニューを表示するなら、暗転せずにメニューを表示
        if (transitionStageNumber == 0)
        {
            gameManager.TransitionStage(transitionStageNumber);
            yield break;
        }

        //階段を登るSEを鳴らし、BGMをフェードアウトする
        audioSourceSE.PlayOneShot(stairsSE);
        StartCoroutine(VolumeDown());

        //プレイヤーを動けなくする
        gameManager.keyInputManager.noControl = true;
        Time.timeScale = 0;

        //ミニマップ非表示
        if (gameManager.miniMap != null) { gameManager.miniMap.MapOff(); }

        //黒パネルを表示し、透明にする
        GameObject blackPanelObj = gameManager.blackPanel.gameObject;
        blackPanelObj.SetActive(true);

        Image blackPanel = gameManager.blackPanel;
        
        blackPanel.color = new Color(1, 1, 1, 0);

        //黒パネルの透明度を徐々に下げる
        for (float i = 0; i < 1; i += 0.008f)
        {
            blackPanel.color = new Color(1, 1, 1, i);
            yield return null;
        }

        blackPanel.color = new Color(1, 1, 1, 1);
       
        //コルーチンを動かすため時間を戻す
        Time.timeScale = 1;

        yield return new WaitForSeconds(3.0f);

        //少し停止してから画面を遷移する
        gameManager.TransitionStage(transitionStageNumber);

        //動けるようにする
        gameManager.keyInputManager.noControl = false;
    }

    /// <summary>
    /// 徐々にBGMのボリュームを下げるコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator VolumeDown()
    {
        while (audioSourceBGM.volume > 0)
        {
            audioSourceBGM.volume -= 0.0015f;
            yield return null;
        }
    }
}
