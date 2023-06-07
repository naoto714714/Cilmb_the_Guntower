using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// �G���A���̓G��|���I�������A�h�A�̃u���b�N�������A�󔠂��o��������N���X
/// </summary>
public class StageManager : MonoBehaviour
{
    PrehubManager prehubManager;
    Player playerScript;
    MiniMap minimapScript;

    Transform myTransform;

    GameObject chest;
    GameObject smoke;
    bool chestOpen = false;

    public int areaNumber;
    public bool noChest;
    public bool enteredCheck = false;
    /// <summary>
    /// �o��������󔠂̐�
    /// </summary>
    [SerializeField] int chestNumber;

    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        GameManager gameManager = gameMGR.GetComponent<GameManager>();
        playerScript = gameManager.playerScript;
        minimapScript = gameManager.miniMap;

        prehubManager = gameManager.prehubManager;

        myTransform = gameObject.transform;

        smoke = prehubManager.Chest_Smoke;
    }

    void Update()
    {
        if (noChest) { return; } //�󔠂Ȃ��Ȃ烊�^�[���i�X�^�[�g�G���A���j
        if (!enteredCheck) { return; } //�v���C���[���N�����ĂȂ��Ȃ烊�^�[��
        if (chestOpen) { return; } //�󔠂����ɏo�������Ȃ烊�^�[��

        //�퓬���́A�v���C���[�̈ړ����x��ʏ��
        playerScript.walkSpeedAddition = 1.0f;

        //�X�e�[�W���ɓG����̂ł�����Ȃ烊�^�[��
        foreach (Transform childTransform in myTransform)
        {
            if (childTransform.CompareTag("Enemy"))
            {
                return;
            }
        }

        //StageManager�̎q�I�u�W�F�N�g���̒��ɁA"Enemy"�̃^�O���t�����̂������Ȃ�����󔠏o��
        //chestNumber�̐������A�Ԋu�T�����J���ĕ󔠏o��
        //addX = -5, 10, -15, 20, -25, �c
        //posX = -5, 5, -10, 10, -15, �c
        float posX = 0;
        float addX = 0;
        float chestInterval = -5;
        
        //chestNumber�̐������󔠏o��
        for (int i = 0; i < chestNumber; i++)
        {
            SetChest();
            Vector3 appearPos = new Vector3(myTransform.position.x + posX, myTransform.position.y, myTransform.position.z);
            GameObject chestClone = Instantiate(chest, appearPos, Quaternion.identity);
            GameObject smokeClone = Instantiate(smoke, appearPos, Quaternion.identity);
            chestClone.transform.parent = myTransform;
            smokeClone.transform.parent = myTransform;

            addX *= -1;
            addX += chestInterval;
            chestInterval *= -1;
            posX += addX;
        }

        chestOpen = true;

        //�퓬���I�������A�v���C���[�̈ړ����x�A�b�v�i�`���[�g���A���łȂ��Ȃ�j
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            playerScript.walkSpeedAddition = 1.3f;
        }

        //�퓬���I�������A�~�j�}�b�v�I��
        minimapScript.MapOn();

        foreach (Transform childTransform in myTransform)
        {
            //�S�Ẵh�A�̏o��������J����
            if (childTransform.CompareTag("Door"))
            {
                Transform doorBlock = childTransform.Find("DoorBlock");
                doorBlock.gameObject.SetActive(false);
            }

            //�����ɓ��������ǂ����̔���𖳂���
            if (childTransform.CompareTag("EnterCheck"))
            {
                childTransform.gameObject.SetActive(false);
            }
        }
    }

    //�o��������󔠂������_���Ɍ��肷�郁�\�b�h
    void SetChest()
    {
        int randDecideChestType = Random.Range(1, 101);

        //59���Ŗ؂̕󔠁i�A�C�e���P�j
        if (randDecideChestType <= 59)
        {
            chest = prehubManager.Chest1_Wood_ItemSingle;
        }
        //20%�Ŗ؂̕󔠁i�A�C�e���Q�j
        else if (randDecideChestType <= 79)
        {
            chest = prehubManager.Chest2_Wood_ItemDouble;
        }
        //10%�Ő��󔠁i�����NB�̏e�j
        else if (randDecideChestType <= 89)
        {
            chest = prehubManager.Chest3_Blue;
        }
        //8%�ŐԂ��󔠁i�����NA�̏e�j
        else if (randDecideChestType <= 97)
        {
            chest = prehubManager.Chest4_Red;
        }
        //3%�ō����󔠁i�����NS�̏e�j�@
        else
        {
            chest = prehubManager.Chest5_Black;
        }
    }
}
