using UnityEngine;
using Calc;

/// <summary>
/// �A�C�e�����Ǘ�����N���X
/// </summary>
public class Item : MonoBehaviour
{
    GameManager gameManager;
    SEManager SEManager;
    [SerializeField] int itemClassNumber;
    [SerializeField] bool isShopItem;
    [SerializeField] int price;
    [SerializeField] GameObject priceCanvas;

    Transform myTransform;
    SpriteRenderer mySpriteRenderer;

    Player playerScript;
    Transform gunManagerTransform;
    GunManager gunManagerScript;
    Material outline;
    Material defaultMaterial;

    public bool canGet = false;
    bool isInRange;
    float canGetRange = 1.5f; //�A�C�e�����擾�\�Ȕ͈�

    AudioClip itemGetSE;
    AudioClip itemCantGetSE;
    AudioSource audioSource;

    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myTransform = gameObject.transform;
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        playerScript = gameManager.playerScript;
        if (gameManager.gunManager != null)
        {
            gunManagerTransform = gameManager.gunManager.gameObject.transform;
            gunManagerScript = gameManager.gunManager;
        }
        outline = gameManager.outline;
        defaultMaterial = gameManager.defaultMaterial;

        itemGetSE = SEManager.SE_ItemGet;
        
        //�_�C�������h����肵���Ƃ��͐�p�̌��ʉ��ɂ���
        if (itemClassNumber == 10)
        {
            itemGetSE = SEManager.SE_GetDiamond;
        }

        //�V���b�v�A�C�e���̏ꍇ�͎擾�\�͈͂��L���A���ʉ���ς���
        if (isShopItem)
        {
            itemGetSE = SEManager.SE_Buy;
            itemCantGetSE = SEManager.SE_CantBuy;
            canGetRange = 1.8f;
        }
    }


    void Update()
    {
        Vector3 myPos = myTransform.position;
        Vector3 playerPos = gameManager.playerPos;

        if (!canGet) { return; }�@//�擾�s�\�ȏꍇ�̓��^�[��

        if (Calculate.Distance(myPos, playerPos) > canGetRange)//�v���C���[���擾�\�͈͊O�̏ꍇ�̓��^�[��
        {
            if (isInRange)
            {
                isInRange = false;
                mySpriteRenderer.material = defaultMaterial;
                if (isShopItem) { priceCanvas.SetActive(false); }
            }
            return;
        }

        //�v���C���[���擾�\�͈͂ɂ���ꍇ
        //�v���C���[���擾�\�͈͓��ɂ���Ƃ��A�A�C�e���ɔ��t�`��t���AE�L�[�Ŏ擾�\�ɂ���
        if (!isInRange)
        {
            isInRange = true;
            mySpriteRenderer.material = outline;
            if (isShopItem) { priceCanvas.SetActive(true); } //�V���b�v�A�C�e���Ȃ��������\��
        }
        
        //E�L�[�ŃA�C�e���擾
        if (Input.GetKeyDown(KeyCode.E))//�V���b�v�A�C�e���Ȃ珊���R�C�����牿�i�������A���������\��
        {
            if (isShopItem)
            {
                if (playerScript.holdCoin < price)
                {
                    audioSource.PlayOneShot(itemCantGetSE);
                    return;
                }
                playerScript.holdCoin -= price;
                priceCanvas.SetActive(false);
            }
            audioSource.PlayOneShot(itemGetSE); //�擾�̌��ʉ���炷

            //�A�C�e�����e�Ȃ�A���̏e��gunManager�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɂ���
            if (gameObject.CompareTag("Gun"))
            {
                Gun gunScript = gameObject.GetComponent<Gun>();
                gunScript.enabled = true;
                Item itemScript = gameObject.GetComponent<Item>();
                itemScript.enabled = false;

                myTransform.parent = gameManager.gunManager.transform;
                mySpriteRenderer.material = defaultMaterial;
                gameObject.SetActive(false);
            }
            //�e�ȊO�Ȃ�A�A�C�e���N���X�i���o�[�ɂ���ď�����ς���
            else
            {
                switch (itemClassNumber)
                {
                    //1�Ȃ�HP���Q�񕜁A2�Ȃ�HP��1��
                    case 1:
                    case 2:
                        GetHeart(itemClassNumber);
                        break;

                    //3�Ȃ�{�����P���₷
                    case 3:
                        GetBomb();
                        break;
                    
                    //4�Ȃ�A�������̑S�Ă̕���̎c�e��������
                    //5�Ȃ�A�������̕���̎c�e��������
                    //6�Ȃ�A�������̕���̎c�e�����S�ɉ�
                    case 4:
                    case 5:
                    case 6:
                        GetBulletBox(itemClassNumber);
                        break;

                    //7�Ȃ�A�R�C���������l��
                    //8�Ȃ�A�R�C�����܂��܂��l��
                    //9�Ȃ�A�R�C�����ʂɊl��
                    case 7:
                    case 8:
                    case 9:
                        GetCoin(itemClassNumber);
                        break;
                    //10�Ȃ�A�Q�[���N���A���o
                    case 10:
                        gameManager.CallClearGame();
                        break;
                }
                gameObject.SetActive(false);
                Destroy(gameObject,2.0f); //�A�C�e���������Ɏ擾����ƃG���[�ɂȂ邩��A���ԍ��Ŕj�󂷂�
            }
        }
    }


    /// <summary>
    /// �n�[�g���擾�����Ƃ��̃��\�b�h
    /// </summary>
    /// <param name="itemClassNumber">�A�C�e���ԍ�</param>
    void GetHeart(int itemClassNumber)
    {
        switch (itemClassNumber)
        {
            //�A�C�e���ԍ����P�Ȃ�A�Q��
            case 1:
                playerScript.playerHP += 2;
                break;

            //�A�C�e���ԍ����Q�Ȃ�A�P��
            case 2:
                playerScript.playerHP += 1;
                break;
        }
    }


    /// <summary>
    /// �{�����擾�����Ƃ��̃��\�b�h
    /// </summary>
    void GetBomb()
    {
        //�v���C���[�̃{�����P���₷
        playerScript.remainBomb += 1;
    }


    /// <summary>
    /// �e�򔠂��擾�����Ƃ��̃��\�b�h
    /// </summary>
    /// <param name="itemClassNumber">�A�C�e���ԍ�</param>
    void GetBulletBox(int itemClassNumber)
    {
        Gun gunScript = null;
        const float bitBulletRatio = 0.3f; //�ő�̎c�e*0.3�̒e���擾

        //���ݑ������Ă���e�̃X�N���v�g���擾
        switch (gunManagerScript.equipGunNumber)
        {
            case 1:
                gunScript = gunManagerScript.gun1.GetComponent<Gun>();
                break;

            case 2:
                gunScript = gunManagerScript.gun2.GetComponent<Gun>();
                break;
        }

        //�A�C�e���ԍ��ɂ���ď�����ς���
        switch (itemClassNumber)
        {   
            //4�Ȃ�A�������̑S�Ă̏e�̎c�e��������
            case 4:
                foreach (Transform guns in gunManagerTransform)
                {
                    gunScript = guns.GetComponent<Gun>();
                    gunScript.remainBullet += (int)(gunScript.maxBullet * bitBulletRatio);
                    if (gunScript.remainBullet > gunScript.maxBullet)
                    {
                        gunScript.remainBullet = gunScript.maxBullet;
                    }
                }
                break;

            //5�Ȃ�A�������̕���̎c�e��������
            case 5:
                gunScript.remainBullet += (int)(gunScript.maxBullet * bitBulletRatio);
                break;

            //6�Ȃ�A�������̕���̎c�e�����S�ɉ�
            case 6:
                gunScript.remainBullet = gunScript.maxBullet;
                break;
        }

        //�c�e������𒴂��Ȃ��悤�ɂ���
        if(gunScript.remainBullet > gunScript.maxBullet)
        {
            gunScript.remainBullet = gunScript.maxBullet;
        }
    }


    /// <summary>
    /// �R�C�����擾�����Ƃ��̃��\�b�h
    /// </summary>
    /// <param name="itemClassNumber">�A�C�e���ԍ�</param>
    void GetCoin(int itemClassNumber)
    {
        const int bronzeCoinNumber = 3; //�u�����Y�R�C���Ȃ�3���l��
        const int silverCoinNumber = 10; //�V���o�[�R�C���Ȃ�10���l��
        const int goldCoinNumber = 30; //�S�[���h�R�C���Ȃ�30���l��
        switch (itemClassNumber)
        {
            case 7:
                playerScript.holdCoin += bronzeCoinNumber;
                break;

            case 8:
                playerScript.holdCoin += silverCoinNumber;
                break;

            case 9:
                playerScript.holdCoin += goldCoinNumber;
                break;
        }
    }
}

