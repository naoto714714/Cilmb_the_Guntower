using UnityEngine;
using UnityEngine.UI;

//武器選択画面の銃ごとの画像を管理するクラス
public class GunSelectUIImage : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int gunSelectUIAnimeNumber;

    Image myImage;
    RectTransform myRect;
    Vector2 myPos;
    Collider2D myCollider;

    GunSelectUIManager gunSelectUIManagerScript;
    Canvas UICanvas;
    RectTransform UICanvasRect;

    bool isMyDrag;


    void OnEnable()
    {
        gunSelectUIManagerScript = gameManager.gunSelectUIManager;

        UICanvas = gameManager.UICanvas;
        UICanvasRect = gameManager.gunSelectUIRect;

        myImage = gameObject.GetComponent<Image>();
        myRect = gameObject.GetComponent<RectTransform>();

        myCollider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isMyDrag) { return; } //自分の銃の画像をドラッグ中なら続ける

        //マウスのローカル座標を取得し、自分の中の画像をマウスの位置に合わせ続ける
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvasRect, Input.mousePosition, UICanvas.worldCamera, out Vector2 mousePos);
        myRect.anchoredPosition = new Vector2(mousePos.x, mousePos.y);

        //銃の画像のドラッグをやめたら、銃の画像をもとのポジションに戻す
        if (Input.GetMouseButtonUp(0))
        {
            myCollider.enabled = true; //画像のコライダーをtrueに
            gameObject.transform.position = myPos; //画像をもとの位置に戻す
            gunSelectUIManagerScript.TextNone(); //武器選択画面の、武器の名前・武器の残弾・武器の説明文の表示をオフに

            gunSelectUIManagerScript.isDrag = false;
            gunSelectUIManagerScript.dragGunNumber = 0;
            isMyDrag = false;
        }
    }

    void OnMouseOver()
    {

        //自分の銃の画像の上で左クリックを押したら、その時の画像の位置を保存し、ドラッグしている判定にする
        if (Input.GetMouseButtonDown(0))
        {
            myCollider.enabled = false;
            myPos = gameObject.transform.position;
            gunSelectUIManagerScript.dragGunNumber = gunSelectUIAnimeNumber;
            gunSelectUIManagerScript.isDrag = true;
            isMyDrag = true;
        }

        //画像のドラッグをやめたとき、マウスの位置が装備武器切り替えの位置なら、装備中の武器をドラッグした武器に切り替える
        if (Input.GetMouseButtonUp(0))
        {
            if (gunSelectUIAnimeNumber > 0) { return; }
            if (gunSelectUIManagerScript.dragGunNumber == 0) { return; }
            gunSelectUIManagerScript.EquipGunChange(gunSelectUIAnimeNumber, gunSelectUIManagerScript.dragGunNumber);
        }

        if (gunSelectUIManagerScript.isDrag && gunSelectUIAnimeNumber > 0) { return; } //すでにドラッグ中かつ所持武器にカーソルが合ったらリターン
        //武器の画像が空欄じゃなく、ドラッグ中または所持武器にカーソルがあっていたら、その武器の欄を光らせる
        gunSelectUIManagerScript.animeNumber = gunSelectUIAnimeNumber; 

        if (gunSelectUIManagerScript.isDrag) { return; }
        //ドラッグ中は武器の説明文を切り替えない
        gunSelectUIManagerScript.TextChange();
    }

    void OnMouseExit()
    {
        if (myImage.sprite.name == "None") { return; }
        gunSelectUIManagerScript.animeNumber = 0; //自分の銃の画像のマウスオーバーをやめたら、武器の欄を光らせるのをやめる

        if (gunSelectUIManagerScript.isDrag) { return; }
        gunSelectUIManagerScript.TextNone(); //自分の銃の画像のマウスオーバーをやめたら、説明文等の表示をオフに
    }

}
