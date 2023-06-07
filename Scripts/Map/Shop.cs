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

        StartCoroutine(CreateItem()); //�P�t���[���J���Ă���A�C�e������
        StartCoroutine(CreateGun()); //�P�t���[���J���Ă��畐�퐶��
    }

    /// <summary>
    /// �V���b�v���ɃA�C�e����������R���[�`���A�d���͂��Ȃ��悤��
    /// ���������A�C�e���͈ꎞ�I�ɃV���b�v�A�C�e���̃��X�g���珜��
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

        //�V���b�v�̃A�C�e�����X�g��������
        ItemAndGunData.ShopItemListInitialize();
    }

    /// <summary>
    /// �V���b�v���ɕ��퐶������R���[�`���A�����NB�P�A�����NA�P��
    /// ������������͕���̃f�[�^�̔z�񂩂珜���A�󔠂���łȂ��悤��
    /// </summary>
    IEnumerator CreateGun()
    {
        yield return null;

        int i = 0;
        foreach (Vector2 vec2 in gunPos)
        {
            int gunNum = 0;

            //�������̃����NB�̕��킩�烉���_���ɐ���
            if (i == 0)
            {
                gunNum = Calculate.ChooseNumber(ItemAndGunData.gunsListRankB);
                ItemAndGunData.gunsListRankB.Remove(gunNum);
            }
            //�������̃����NA�̕��킩�烉���_���ɐ���
            else if (i == 1)
            {
                gunNum = Calculate.ChooseNumber(ItemAndGunData.gunsListRankA);
                ItemAndGunData.gunsListRankA.Remove(gunNum);
            }

            GameObject gun = prehubManager.SetShopGun(gunNum);
            //�S�Ă̏e�������Ă���Ȃɂ��\�����Ȃ�
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
