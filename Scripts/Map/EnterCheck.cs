using UnityEngine;
using UnityEngine.Tilemaps;
using Calc;

/// <summary>
/// �v���C���[���G���A���ɐN���������ǂ����𔻒肵�A�N�������炻�̃G���A���A�N�e�B�u�ɂ��郁�\�b�h
/// </summary>
public class EnterCheck : MonoBehaviour
{
    /// <summary>
    /// ��������������A�k�F�P�C���F�Q�C��F�R�A���F�S
    /// </summary>
    [SerializeField] int directionNumber;
    [SerializeField] int activeRoadNumber;
    [SerializeField] bool bossArea;
    [SerializeField] bool shopArea;
    int activeAreaNumber;
    GameManager gameManager;
    MiniMap miniMap;
    StageManager stageManager;
    SEManager SEManager;

    Transform myTransform;
    bool myEnteredCheck = false;
    bool seCheck = false;
    bool isEnter = false;

    Transform parentArea;
    
    GameObject road;

    AudioClip roomOpenSE;
    AudioSource audioSourceSE;

    //�V���b�v�p
    AudioClip exploreBGM;
    AudioClip shopBGM;
    AudioSource audioSourceBGM;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        Transform roadManagerTransform = gameManager.roadManagerTranform;
        miniMap = gameManager.miniMap;

        SEManager = gameManager.SEManager;
        audioSourceSE = gameManager.audioSourceSE;

        //�A�N�e�B�u���[�h�i���o�[�ɂ���ăA�N�e�B�u�ɂ��郍�[�h�����肷��
        foreach (Transform road in roadManagerTransform)
        {
            RoadNumber roadNumberScript = road.GetComponent<RoadNumber>();
            int roadNumber = roadNumberScript.roadnumber;
            if (activeRoadNumber == roadNumber)
            {
                this.road = road.gameObject;
                break;
            }
        }

        myTransform = gameObject.transform;

        parentArea = myTransform.parent.parent;

        stageManager = parentArea.GetComponent<StageManager>();
        activeAreaNumber = stageManager.areaNumber;

        roomOpenSE = SEManager.SE_RoomOpen;

        if (!shopArea) { return; }

        BGMManager BGMManager = gameManager.BGMManager;
        exploreBGM = BGMManager.BGM_Explore;
        shopBGM = BGMManager.BGM_Shop;
        audioSourceBGM = gameManager.audioSourceBGM;
    }

    /// <summary>
    /// �v���C���[�������ɓ��������ǂ����𔻒肷�郁�\�b�h
    /// </summary>
    void OnTriggerExit2D(Collider2D collision)
    {
        //�P��N��������Q��ȏ�͔��肵�Ȃ��A�V���b�v�͕����񔻒�
        if (myEnteredCheck && !shopArea) { return; }

        //�N�������̂��v���C���[�ȊO�Ȃ烊�^�[��
        if (!collision.CompareTag("Player")) { return; }

        Vector2 vector = Calculate.Vector(myTransform.position, collision.transform.position);
        
        //�v���C���[���w�肵�������ɒʂ蔲�����ꍇ�̓G���A���A�N�e�B�u�A�t�ɒʂ蔲�����ꍇ�̓��[�h���A�N�e�B�u
        switch (directionNumber)
        {
            case 1: //�k
                if (vector.y > 0)
                {
                    AreaActive();
                }
                else if (vector.y < 0)
                {
                    RoadActive();
                }
                break;


            case 2: //��
                if (vector.x > 0)
                {
                    AreaActive();
                }
                else if (vector.x < 0)
                {
                    RoadActive();
                }
                break;

            case 3: //��
                if (vector.y < 0)
                {
                    AreaActive();
                }
                else if (vector.y > 0)
                {
                    RoadActive();
                }
                break;

            case 4: //��
                if (vector.x < 0)
                {
                    AreaActive();
                }
                else if (vector.x > 0)
                {
                    RoadActive();
                }
                break;
        }
    }

    /// <summary>
    /// �G���A���̑S�ẴI�u�W�F�N�g�A�h�A���ǂ��u���b�N���A�N�e�B�u�ɂ��郁�\�b�h
    /// </summary>
    void AreaActive()
    {
        //�V���b�v�Ȃ�BGM��ς��A���ĂȂ��悤�ɂ���
        if (shopArea && !isEnter)
        {
            audioSourceBGM.Stop();
            audioSourceBGM.clip = shopBGM;
            audioSourceBGM.Play();
            gameManager.noShoot = true;
            isEnter = true;
        }

        if (stageManager.enteredCheck) { return; } //���ɉ���ς݂̃G���A�Ȃ甽�����Ȃ��i�h�A�ɗ����Ė߂�ƂQ���������̂�h���j
        if (stageManager.noChest) { return; } //�󔠂��Ȃ������ɂ͔������Ȃ��i�X�^�[�g�G���A���j
        stageManager.enteredCheck = true;
        myEnteredCheck = true;
        RoomOpenSE();

        foreach (Transform childTransform in parentArea)
        {
            childTransform.gameObject.SetActive(true);

            //�X�e�[�W���̑S�Ẵh�A�I�u�W�F�N�g�̎q�I�u�W�F�N�g�Ɋ܂܂��A�h�A���ǂ��u���b�N���A�N�e�B�u
            if (childTransform.CompareTag("Door"))
            {
                Tilemap doorTile = childTransform.GetComponent<Tilemap>();
                doorTile.color = new Color(1, 1, 1, 1);
                Transform doorBlock = childTransform.Find("DoorBlock");
                doorBlock.gameObject.SetActive(true);
            }
        }
        
        //�{�X�G���A�ɐN�������Ƃ��A���o���Đ�����
        if (bossArea)
        {
            StagingManager stagingManager = gameManager.stagingManager;
            Vector2 areaPos = parentArea.localPosition;
            stagingManager.StagingBossArea(areaPos);
        }

        //�~�j�}�b�v�ɊJ�������G���A��\�������A�퓬���̓~�j�}�b�v��\�����Ȃ�
        miniMap.ActiveArea(activeAreaNumber);
        miniMap.MapOff();
    }

    /// <summary>
    /// �����A�N�e�B�u�ɂ��郁�\�b�h
    /// </summary>
    void RoadActive()
    {
        //�V���b�v����o����BGM�����Ƃɖ߂��A���Ă�悤�ɂ���
        if (shopArea && isEnter)
        {
            audioSourceBGM.Stop();
            audioSourceBGM.clip = exploreBGM;
            audioSourceBGM.Play();
            gameManager.noShoot = false;
            isEnter = false;
        }
        road.SetActive(true);
        miniMap.ActiveRoad(activeRoadNumber);
        RoomOpenSE();
        //������myEnteredCheck������ƁA�h�A�ŏo���肷��ƃX�e�[�W���\������Ȃ��Ȃ�
    }

    void RoomOpenSE()
    {
        //BGM�͈�x�����Ȃ�Ȃ��悤��
        //���̂悤�ɂ��Ȃ��ƈ�x�̏o����ő�ʂɌ��ʉ�����
        if (!seCheck)
        {
            audioSourceSE.PlayOneShot(roomOpenSE);
            seCheck = true;
        }
    }
}
