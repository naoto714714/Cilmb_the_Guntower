using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

/// <summary>
/// �󔠂̒��g�̐ݒ�ƁA�󔠂��J�����Ƃ��ɃA�C�e�����o��������N���X
/// </summary>
public class Chest : MonoBehaviour
{
    /// <summary>
    /// �󔠂̃^�C�v�ɂ���đI�Ԓ��g��ς���
    /// </summary>
    [SerializeField] int chestTypeNumber;
    /// <summary>
    /// �������X���[�ɂ���
    /// </summary>
    [SerializeField] bool appearSlow;

    GameManager gameManager;
    SEManager SEManager;
    PrehubManager prehubManager;

    Transform myTransform;
    Animator myAnime;
    SpriteRenderer mySpriteRenderer;

    float canOpenRange = 2;//�󔠂��J������͈�
    int itemAppearCount = 1; //�󔠂̒��g�̐�

    Material outline;
    Material defaultMaterial;

    GameObject[] chestContent = new GameObject[2];
    bool openCheck = false;

    AudioClip chestAppearSE;
    AudioClip chestOpenSE;
    AudioSource audioSource;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        prehubManager = gameManager.prehubManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myTransform = gameObject.transform;
        myAnime = gameObject.GetComponent<Animator>();
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        outline = gameManager.outline; //���t�`
        defaultMaterial = gameManager.defaultMaterial;

        chestAppearSE = SEManager.SE_ChestAppear;
        chestOpenSE = SEManager.SE_ChestOpen;
        audioSource.PlayOneShot(chestAppearSE);

        //�`�F�X�g�^�C�v�i���o�[�ɂ���ĕ󔠂̒��g��ς���
        switch (chestTypeNumber)
        {
            //1,2�̓A�C�e�����o��󔠁A2�̓A�C�e�����Q�o��
            case 1:
            case 2:
                if (chestTypeNumber == 2)
                {
                    itemAppearCount = 2;
                }

                chestContent = SetChestContentItem(itemAppearCount);
                break;

            //3��B�����N�̕���
            case 3:
                {
                    chestContent[0] = SetChestContentGun(ref ItemAndGunData.gunsListRankB);
                    break;
                }

            //4��A�����N�̕���
            case 4:
                {
                    chestContent[0] = SetChestContentGun(ref ItemAndGunData.gunsListRankA);
                    break;
                }

            //5��S�����N�̕���
            case 5:
                {
                    chestContent[0] = SetChestContentGun(ref ItemAndGunData.gunsListRankS);
                    break;
                }

            case 100: //�S�[���p�@�_�C�������h
                {
                    chestContent[0] = prehubManager.Item10_Diamond;
                    canOpenRange = 3; //�_�C�������h�̕󔠂͑傫������J������͈͂��L������
                    break;
                }

            case 101: //�`���[�g���A���p�@�O���l�[�h�K��
                {
                    chestContent[0] = prehubManager.Gun_RankA_1_GrenadeGun;
                    break;
                }

            case 102: //�`���[�g���A���p�@�{���Q��
                {
                    itemAppearCount = 2;
                    chestContent[0] = prehubManager.Item3_Bomb;
                    chestContent[1] = prehubManager.Item3_Bomb;
                    break;
                }

            case 103: //�`���[�g���A���p�@�n�[�g�Q��
                {
                    itemAppearCount = 2;
                    chestContent[0] = prehubManager.Item1_Heart_Full;
                    chestContent[1] = prehubManager.Item1_Heart_Full;
                    break;
                }
        }
    }


    void Update()
    {
        if (openCheck) { return; } //�󔠂��J������Update���\�b�h��~

        Vector3 myPos = myTransform.position;
        Vector3 playerPos = gameManager.playerPos;

        mySpriteRenderer.material = defaultMaterial;

        //�v���C���[��canOpenRange���ɂ��邩���肵�A�����甒�t�`������
        if (Calculate.Distance(myPos, playerPos) > canOpenRange) { return; }

        mySpriteRenderer.material = outline;

        //�󔠂̋߂��ɂ���Ƃ��AE�L�[�������ꂽ��󔠂��J���ăA�C�e���o��
        if (Input.GetKey(KeyCode.E))
        {
            ChestOpen();
        }
    }

    /// <summary>
    /// �A�C�e���̏o���m�����l�����A�󔠂̒��g�̃A�C�e�������߂郁�\�b�h
    /// </summary>
    /// <param name="itemAppearCount">�A�C�e���̌�</param>
    /// <returns>�󔠂̒��g�̃A�C�e��</returns>
    GameObject[] SetChestContentItem(int itemAppearCount)
    {
        GameObject[] chestContents = new GameObject[] { null, null };

        for (int i = 0; i < itemAppearCount; i++)
        {
            float sumProbability = 0; //�ݐϊm��
            int rand = Random.Range(1, 101); //�P�`�P�O�O�̊Ԃ̗���

            //�������ݐϊm���ȉ��ɂȂ����Ƃ��A���̎��̃Q�[���I�u�W�F�N�g��chestContent�ɓ������
            foreach (KeyValuePair<int, int> item in ItemAndGunData.itemDict)
            {
                sumProbability += item.Value;

                if (rand > sumProbability) { continue; }

                chestContents[i] = prehubManager.SetItem(item.Key);
                break;
            }
        }

        return chestContents;
    }

    /// <summary>
    /// �e�e�̏o���m���͓��m���ŁA�󔠂̒��g�̏e�����߂郁�\�b�h
    /// </summary>
    /// <param name="gunsList">�e�̕��ޔԍ����܂Ƃ߂����X�g</param>
    /// <returns>�󔠂̒��g�̏e</returns>
    GameObject SetChestContentGun(ref List<int> gunsList)
    {
        GameObject chestContent = null;
        float sumProbability = 0; //�ݐϊm��
        int rand = Random.Range(1,101); //�P�`�P�O�O�̊Ԃ̗���

        float eachProbablity = 100.0f / gunsList.Count; //�e�e�̏o���m���i���m���j

        //�������ݐϊm���ȉ��ɂȂ����Ƃ��A���̎��̃Q�[���I�u�W�F�N�g��chestContent�ɓ������
        foreach (int gunClassNumber in gunsList)
        {
            sumProbability += eachProbablity;

            if (rand > sumProbability) { continue; }

            chestContent = prehubManager.SetGun(gunClassNumber);
            gunsList.Remove(gunClassNumber);

            break;
        }

        //���ɑS��ނ̏e�������Ă���ꍇ�A�R�C���ɒu��������
        if (gunsList.Count == 0)
        {
            chestContent = prehubManager.Item8_Coin_Silver;
        }

        return chestContent;
    }


    /// <summary>
    /// �󔠂��J���ꂽ�Ƃ��A�󔠂��J�����A�j���[�V�����ɕύX���A�A�C�e�����o�������郁�\�b�h
    /// </summary>
    void ChestOpen()
    {
        //�A�j�����J�����A�j���ɁA���t�`���Ȃ����A���ʉ��Đ�
        myAnime.SetTrigger("Open");
        mySpriteRenderer.material = defaultMaterial;
        audioSource.PlayOneShot(chestOpenSE);

        openCheck = true;

        int directionX = 0; //�A�C�e�����o����������i�O�͐^��j
        for (int i = 0; i < itemAppearCount; i++)
        {
            GameObject itemClone = Instantiate(chestContent[i], myTransform.position, Quaternion.identity);
            itemClone.transform.parent = myTransform;

            //�A�C�e�����Q�o������Ȃ�A�P�ڂ��E���ɁA�Q�ڂ������ɏo��������
            if (itemAppearCount == 2 && i == 0)
            {
                directionX = 1;
            }
            else if (itemAppearCount == 2 && i == 1)
            {
                directionX = -1;
            }
            StartCoroutine(ItemAppear(itemClone.transform, directionX));
        }
    }
    
    /// <summary>
    /// �󔠂���A�C�e������яo���悤�ɏo��������R���[�`���A�A�C�e���o������͎擾�s��
    /// </summary>
    /// <param name="itemTransform">�I�΂ꂽ�󔠂̒��g�̃g�����X�t�H�[��</param>
    /// <param name="directionX">��яo�������i�O�͐^��j</param>
    IEnumerator ItemAppear(Transform itemTransform, int directionX)
    {
        const float itemAppearMoveX = 1.5f; //�A�C�e�����ړ�����X���W
        const float itemFallMoveY = 2.0f; //�A�C�e�����ړ�����Y���W
        float itemAppearUpSpeed = 2.0f; //�A�C�e���㏸���x
        float itemStopTime = 0.3f; //�󒆂ł̒�~����
        float itemFallDownSpeed = 1.6f; //�A�C�e�����~���x
        float elapsedTime = 0; //�o�ߎ���

        if (appearSlow)
        {
            itemAppearUpSpeed = 0.8f;
            itemStopTime = 1.8f;
            itemFallDownSpeed = 0.4f;
        }

        //directionX�̕����ɁA�A�C�e�����R�Ȃ�Ɉړ�������
        //�󔠂̈ʒu����(directionX, 1.5)��ڎw���A���X�Ɉړ�����
        for (float i = 0; i <= itemAppearMoveX;)
        {
            elapsedTime += Time.deltaTime;
            i = itemAppearMoveX * itemAppearUpSpeed * elapsedTime;
            //y = -2/3 * (x - 3/2)^2 + 1  �� ���_��(1.5, 1)�Ƃ��A(0, 0)��ʂ�񎟊֐�
            double PosY = (-2.0 / 3.0) * (i - 3.0 / 2.0) * (i - 3.0 / 2.0) + 1;
            itemTransform.localPosition = new Vector3(i * directionX, (float)PosY, -1);
            yield return null;
        }

        //(directionX, 1.5)�Ɉړ�������A������~���A�擾�\�ɂ���
        yield return new WaitForSeconds(itemStopTime);

        Item itemScript = itemTransform.gameObject.GetComponent<Item>();
        itemScript.canGet = true;
        float x = itemTransform.localPosition.x;
        float y = itemTransform.localPosition.y;
        elapsedTime = 0;

        //������~��A���̏ꂩ�珙�X�ɗ�������i�󔠂�y���W-1�܂Łj
        for (float i = 0; i <= itemFallMoveY;)
        {
            elapsedTime += Time.deltaTime;
            i = itemAppearMoveX * itemFallDownSpeed * elapsedTime;
            itemTransform.localPosition = new Vector3(x, y - i, -1);
            yield return null;
        }

        //�������g���폜�i�X�N���v�g�̂݁j
        Destroy(gameObject.GetComponent<Chest>());
    }

}
