using UnityEngine;

/// <summary>
/// プレイヤーが触れたらダメージを受けるようなトラップのクラス
/// </summary>
public class Trap : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        //衝突したのがプレイヤーなら、プレイヤーにダメージを与える
        //無敵のプレイヤーにはダメージを与えないように、レイヤーで判断する
        int layerNumber = (LayerMask.NameToLayer("Player"));
        if (collision.gameObject.layer == layerNumber)
        {
            Player playerScript = collision.gameObject.GetComponent<Player>();
            playerScript.Damaged(1);
        }
    }
}
