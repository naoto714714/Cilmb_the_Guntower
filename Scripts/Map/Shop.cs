using UnityEngine;
using System.Collections;
using Calc;

public class Shop : MonoBehaviour
{
    GameManager gameManager;
    PrehubManager prehubManager;

    [SerializeField] Vector2[] itemPos;
    [SerializeField] Vector2[] gunPos;

    Transform myTransform;

    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        prehubManager = gameManager.prehubManager;

        myTransform = gameObject.transform;

        StartCoroutine(CreateItem()); //１フレーム開けてからアイテム生成
        StartCoroutine(CreateGun()); //１フレーム開けてから武器生成
    }

    /// <summary>
    /// ショップ内にアイテム生成するコルーチン、重複はしないように
    /// 生成したアイテムは一時的にショップアイテムのリストから除く
    /// </summary>
    IEnumerator CreateItem()
    {
        yield return null;
        
        int i = 0;
        foreach (Vector2 vec2 in itemPos)
        {
            int itemNum = Calculate.ChooseNumber(ItemAndGunData.shopItemList);
            ItemAndGunData.shopItemList.Remove(itemNum);

            GameObject item = prehubManager.SetShopItem(itemNum);
            GameObject obj = Instantiate(item, Vector3.zero, Quaternion.identity, myTransform);
            obj.transform.localPosition = vec2;
            obj.SetActive(false);
            i += 1;
        }

        //ショップのアイテムリストを初期化
        ItemAndGunData.ShopItemListInitialize();
    }

    /// <summary>
    /// ショップ内に武器生成するコルーチン、ランクB１つ、ランクA１つ
    /// 生成した武器は武器のデータの配列から除き、宝箱からでないように
    /// </summary>
    IEnumerator CreateGun()
    {
        yield return null;

        int i = 0;
        foreach (Vector2 vec2 in gunPos)
        {
            int gunNum = 0;

            //未所持のランクBの武器からランダムに生成
            if (i == 0)
            {
                gunNum = Calculate.ChooseNumber(ItemAndGunData.gunsListRankB);
                ItemAndGunData.gunsListRankB.Remove(gunNum);
            }
            //未所持のランクAの武器からランダムに生成
            else if (i == 1)
            {
                gunNum = Calculate.ChooseNumber(ItemAndGunData.gunsListRankA);
                ItemAndGunData.gunsListRankA.Remove(gunNum);
            }

            GameObject gun = prehubManager.SetShopGun(gunNum);
            //全ての銃を持ってたらなにも表示しない
            if (gun != null)
            {
                GameObject obj = Instantiate(gun, Vector3.zero, Quaternion.identity, myTransform);
                obj.transform.localPosition = vec2;
                obj.SetActive(false);
            }

            i += 1;
        }
    }
}
