using UnityEngine;
using System.Collections;

//武器選択画面の銃ごとの画像を管理するクラス
public class GunSelectUIArrow : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int type;

    Animator myAnime;
    Collider2D myCollider;

    GunSelectUIManager gunSelectUIManagerScript;

    AudioClip cursorSE;
    AudioSource audioSource;


    void Awake()
    {
        gunSelectUIManagerScript = gameManager.gunSelectUIManager;

        myAnime = gameObject.GetComponent<Animator>();
        myCollider = gameObject.GetComponent<Collider2D>();

        cursorSE = gameManager.SEManager.SE_GunSelectCursor;
        audioSource = gameManager.audioSourceSE;
    }


    void OnMouseOver()
    {
        if (!myCollider.enabled) { return; }
        if (gunSelectUIManagerScript.isDrag) { return; }
        myAnime.SetTrigger("Over");

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(cursorSE);
            switch (type)
            {
                case 1:
                    gunSelectUIManagerScript.Page1();
                    break;

                case 2:
                    gunSelectUIManagerScript.Page2();
                    break;
            }
        }
    }

    void OnMouseExit()
    {
        if (!myCollider.enabled) { return; }
        myAnime.SetTrigger("Active");
    }

    /// <summary>
    /// 判定を無効にし、１フレーム開けてから無効のアニメを再生するメソッド
    /// １フレーム開けてからじゃないと、MouseOverのアニメの方が優先される
    /// </summary>
    public void NormalArrow()
    {
        myCollider.enabled = false;
        StartCoroutine(NormalAnime());
    }

    /// <summary>
    /// １フレーム開けてから判定を有効にし、有効のアニメを再生するメソッド
    /// １フレーム開けてからじゃないと、GetComponentが間に合わない
    /// </summary>
    public void ActiveArrow()
    {
        StartCoroutine(ActiveCollider());
    }

    //ボタン無効のアニメを１フレーム開けてからオンにするコルーチン
    IEnumerator NormalAnime()
    {
        yield return null;
        myAnime.SetTrigger("Normal");
    }

    //１フレーム開けてからボタンを有効にするコルーチン
    IEnumerator ActiveCollider()
    {
        yield return null;
        myCollider.enabled = true;
        myAnime.SetTrigger("Active");
    }
}
