using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// �قƂ�ǑS�ẴI�u�W�F�N�g�ɃA�N�Z�X�ł��A�ėp�I�ȃ��\�b�h���܂Ƃ߂��N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    //���������������������V���A���C�Y����ϐ���������������������

    //�[�[�[�[�[�[�[�[�[�[�Q�[���I�u�W�F�N�g�[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �I�u�W�F�N�g�@�v���C���[
    /// </summary>
    public GameObject player;
    /// <summary>
    /// �I�u�W�F�N�g�@�������̕����UI
    /// </summary>
    public GameObject gunUIManager;
    /// <summary>
    /// �I�u�W�F�N�g�@����I�����
    /// </summary>
    public GameObject gunSelectUIMGR;
    /// <summary>
    /// �I�u�W�F�N�g�@�Q�[�����I�����܂����H�̃��j���[
    /// </summary>
    public GameObject endGameSelectUI;
    /// <summary>
    /// �I�u�W�F�N�g�@�Q�[���N���A�̃��j���[
    /// </summary>
    public GameObject GameClear;
    /// <summary>
    /// �I�u�W�F�N�g�@�Q�[���I�[�o�[�̃��j���[
    /// </summary>
    public GameObject gameOverMenu;
    /// <summary>
    /// �I�u�W�F�N�g�@����m�F���
    /// </summary>
    public GameObject operationsConfirm;
    /// <summary>
    /// �I�u�W�F�N�g�@UI���܂Ƃ߂��}�l�[�W���[
    /// </summary>
    public GameObject UIMGR;
    /// <summary>
    /// �I�u�W�F�N�g�@�}�b�v�ɕ\������A�v���C���[�̈ʒu��\������_
    /// </summary>
    public GameObject mapPoint;
    /// <summary>
    /// �I�u�W�F�N�g�@�Ŕ�ǂ񂾂Ƃ��ɕ\������A�����̔w�i
    /// </summary>
    public GameObject signboardTextBackground;
    /// <summary>
    /// �I�u�W�F�N�g�@�v���n�u�}�l�[�W���[
    /// </summary>
    public GameObject prehubMGR;
    /// <summary>
    /// �I�u�W�F�N�g�@SE�}�l�[�W���[
    /// </summary>
    public GameObject SEMGR;
    /// <summary>
    /// �I�u�W�F�N�g�@BGM�}�l�[�W���[
    /// </summary>
    public GameObject BGMMGR;

    //�[�[�[�[�[�[�[�[�[�[�g�����X�t�H�[���[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �g�����X�t�H�[���@���C���J����
    /// </summary>
    public Transform cameraTransform;
    /// <summary>
    /// �g�����X�t�H�[���@�G�̒e�̐e�I�u�W�F�N�g
    /// </summary>
    public Transform enemyBulletManagerTransform;
    /// <summary>
    /// �g�����X�t�H�[���@���[�h�̐e�I�u�W�F�N�g
    /// </summary>
    public Transform roadManagerTranform;
    /// <summary>
    /// ���N�g�g�����X�t�H�[���@�n�[�g��UI
    /// </summary>
    public RectTransform heartUITransform;
    /// <summary>
    /// ���N�g�g�����X�t�H�[���@�{����UI
    /// </summary>
    public RectTransform bombUITransform;

    //�[�[�[�[�[�[�[�[�[�[�L�����o�X�[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �L�����o�X�@UI���܂Ƃ߂��L�����o�X
    /// </summary>
    public Canvas UICanvas;

    //�[�[�[�[�[�[�[�[�[�[�C���[�W�[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �C���[�W�@�Ə�
    /// </summary>
    public Image aimImage;
    /// <summary>
    /// �C���[�W�@�t���b�V���i��ʂ����ς��̔��p�l���j
    /// </summary>
    public Image flash;
    /// <summary>
    /// �C���[�W�@�u���b�N�p�l���i��ʂ����ς��̍��p�l���j
    /// </summary>
    public Image blackPanel;
    /// <summary>
    /// �C���[�W�@�����[�h�̓x�����������T�[�N��
    /// </summary>
    public Image reloadCircle;

    //�[�[�[�[�[�[�[�[�[�[�}�e���A���[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �}�e���A���@�摜�ɔ��t�`������
    /// </summary>
    public Material outline;
    /// <summary>
    /// �}�e���A���@�f�t�H���g
    /// </summary>
    public Material defaultMaterial;

    //�[�[�[�[�[�[�[�[�[�[�X���C�_�[�[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �X���C�_�[�@�{�X��HP�o�[
    /// </summary>
    public Slider bossHPSlider;

    //�[�[�[�[�[�[�[�[�[�[�e�L�X�g�[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �e�L�X�g�@�{�X�̖��O
    /// </summary>
    public TextMeshProUGUI bossNameText;
    /// <summary>
    /// �e�L�X�g�@�Ŕ̕���
    /// </summary>
    public TextMeshProUGUI signboardText;
    /// <summary>
    /// �e�L�X�g�@�X�e�[�W�̖��O
    /// </summary>
    public TextMeshProUGUI stageName;
    /// <summary>
    /// �e�L�X�g�@�R�C���̖���
    /// </summary>
    public TextMeshProUGUI coin;

    //�[�[�[�[�[�[�[�[�[�[�X�N���v�g�[�[�[�[�[�[�[�[�[�[
    /// <summary>
    /// �X�N���v�g�@�����Ă��镐����Ǘ�����N���X
    /// </summary>
    public GunManager gunManager;
    /// <summary>
    /// �X�N���v�g�@�~�j�}�b�v���Ǘ�����N���X
    /// </summary>
    public MiniMap miniMap;
    /// <summary>
    /// �X�N���v�g�@�L�[���͂��Ǘ�����N���X
    /// </summary>
    public KeyInputManager keyInputManager;
    /// <summary>
    /// �X�N���v�g�@���o���Ǘ�����N���X
    /// </summary>
    public StagingManager stagingManager;
    /// <summary>
    ///  �G�̃��\�b�h���܂Ƃ߂��N���X
    /// </summary>
    public EnemyMethod enemyMethod;
    /// <summary>
    /// �X�N���v�g�@UI���Ǘ�����N���X
    /// </summary>
    public UIManager uiManager;
    /// <summary>
    /// �X�N���v�g�@����ؑ։�ʂ̃N���X
    /// </summary>
    public GunSelectUIManager gunSelectUIManager;


    //��������������������Awake�ő������ϐ���������������������
    /// <summary>
    /// �g�����X�t�H�[���@�v���C���[
    /// </summary>
    public Transform playerTransform;
    /// <summary>
    /// �X�N���v�g�@�v���C���[���Ǘ�����N���X
    /// </summary>
    public Player playerScript;
    /// <summary>
    /// �g�����X�t�H�[���@�Ə��̃g�����X�t�H�[��
    /// </summary>
    public Transform aimImageTransform;
    /// <summary>
    /// ���N�g�g�����X�t�H�[���@�Ə��̃��N�g�g�����X�t�H�[��
    /// </summary>
    public RectTransform aimImageRect;
    /// <summary>
    ///���N�g�g�����X�t�H�[���@����ؑ։�ʂ̃��N�g�g�����X�t�H�[��
    /// </summary>
    public RectTransform gunSelectUIRect;
    /// <summary>
    /// �X�N���v�g�@�v���n�u���Ǘ�����N���X
    /// </summary>
    public PrehubManager prehubManager;
    /// <summary>
    /// �X�N���v�g�@BGM���Ǘ�����N���X
    /// </summary>
    public BGMManager BGMManager;
    /// <summary>
    /// �I�[�f�B�I�\�[�X�@BGM�̃I�[�f�B�I�\�[�X
    /// </summary>
    public AudioSource audioSourceBGM;
    /// <summary>
    /// �X�N���v�g�@SE���Ǘ�����N���X
    /// </summary>
    public SEManager SEManager;
    /// <summary>
    /// �I�[�f�B�I�\�[�X�@SE�̃I�[�f�B�I�\�[�X
    /// </summary>
    public AudioSource audioSourceSE;


    //����������������������ɍX�V�����ϐ���������������������
    /// <summary>
    /// �R�����x�N�g���@�v���C���[�̃|�W�V����
    /// </summary>
    public Vector3 playerPos;
    /// <summary>
    /// �R�����x�N�g���@�Ə��̃|�W�V����
    /// </summary>
    public Vector3 aimImagePos;


    //���������������������O������؂�ւ���ϐ���������������������
    /// <summary>
    /// �u�[���@�{�X�𓮂����J�n���邩�ǂ���
    /// </summary>
    public bool bossMoveStart = false;
    /// <summary>
    /// �u�[���@�{�X���U���A�j�����Đ����邩�ǂ���
    /// </summary>
    public bool bossAttackAnime = false;
    /// <summary>
    /// �u�[���@�{�X���U���A�j�����~���邩�ǂ���
    /// </summary>
    public bool bossAttackAnimeOff = false;
    /// <summary>
    /// �u�[���@�v���C���[�̑�����\�ɂ��邩�ǂ���
    /// </summary>
    public bool noShoot = false;
    /// <summary>
    /// �u�[���@�X�e�[�W�̓G���g�����X���ǂ���
    /// </summary>
    public bool entrance = false;
    /// <summary>
    /// �`���[�g���A�����ǂ���
    /// </summary>
    public bool tutorial = false;


    //���������������������o�ߎ��Ԃ��i�[����ϐ���������������������
    /// <summary>
    /// ����
    /// </summary>
    int hour;
    /// <summary>
    /// ��
    /// </summary>
    int minite;
    /// <summary>
    /// �b
    /// </summary>
    float seconds;



    void Awake()
    {
        hour = GameData.hour;
        minite = GameData.minite;
        seconds = GameData.seconds;

        playerTransform = player.transform;
        playerScript = player.GetComponent<Player>();
        aimImageTransform = aimImage.transform;
        aimImageRect = aimImage.rectTransform;
        if (gunSelectUIMGR != null)
        {
            gunSelectUIRect = gunSelectUIMGR.GetComponent<RectTransform>();
            gunSelectUIManager = gunSelectUIMGR.GetComponent<GunSelectUIManager>();
        }
        uiManager = UIMGR.GetComponent<UIManager>();
        prehubManager = prehubMGR.GetComponent<PrehubManager>();
        BGMManager = BGMMGR.GetComponent<BGMManager>();
        audioSourceBGM = BGMMGR.GetComponent<AudioSource>();
        SEManager = SEMGR.GetComponent<SEManager>();
        audioSourceSE = SEMGR.GetComponent<AudioSource>();

        StartCoroutine(BlackIn()); //�Q�[���J�n���ɍ���ʂ���t�F�[�h�C������
        StartCoroutine(StageNameDisplay()); //�Q�[���J�n���ɃX�e�[�W����\������
    }

    void Update()
    {
        playerPos = playerTransform.position;
        aimImagePos = aimImageTransform.position;

        TimeCount(); //��Ɍo�ߎ��Ԃ��J�E���g����
    }


    /// <summary>
    /// �o�ߎ��Ԃ��J�E���g���郁�\�b�h
    /// </summary>
    void TimeCount()
    {
        seconds += Time.deltaTime;
        //60�b��������P���ɒ���
        if (seconds >= 60f)
        {
            minite++;
            seconds -= 60;
        }
        //60����������1���Ԃɒ���
        if (minite >= 60)
        {
            hour++;
            minite -= 60;
        }
    }

    /// <summary>
    /// ���݂̌o�ߎ��Ԃ�Ԃ����\�b�h
    /// </summary>
    /// <returns>�o�ߎ��Ԃ̔z��A[0]���ԁA[1]���A[2]�b</returns>
    public int[] ElapsedTime()
    {
        int[] times = new int[3];
        times[0] = hour;
        times[1] = minite;
        times[2] = (int)seconds;
        return times;
    }

    /// <summary>
    /// ����ʂ���t�F�[�h�C������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator BlackIn()
    {
        GameObject blackPanelObj = blackPanel.gameObject;

        //���p�l����\��
        blackPanelObj.SetActive(true);
        blackPanel.color = new Color(1, 1, 1, 1);

        //���p�l���̓����x�����X�ɏグ��
        for (float i = 0; i < 1; i += 0.008f)
        {
            blackPanel.color = new Color(1, 1, 1, 1 - i);
            yield return null;
        }

        //���p�l�����\��
        blackPanel.color = new Color(1, 1, 1, 0);
        blackPanelObj.SetActive(false);
    }

    /// <summary>
    /// �X�e�[�W�J�n���ɃX�e�[�W�̖��O��\������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator StageNameDisplay()
    {
        //�X�e�[�W���𓧖���
        stageName.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(1.0f);

        //�����҂��Ă���X�e�[�W���̓����x�����X�ɉ�����
        for (float i = 0; i < 1; i += 0.008f)
        {
            stageName.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);�@//�X�e�[�W����\�������܂܏�����~

        //�X�e�[�W���̓����x�����X�ɏグ��
        for (float i = 0; i < 1; i += 0.008f)
        {
            stageName.color = new Color(1, 1, 1, 1 - i);
            yield return null;
        }
        //�X�e�[�W���𓧖���
        stageName.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// �h��̎��ԂƑ傫���������Ƃ��A�J������h�炷�R���[�`�����Ăяo�����\�b�h
    /// </summary>
    /// <param name="shakeTime">�h��̎���</param>
    /// <param name="shakeSize">�h��̑傫��</param>
    public void CameraShake(float shakeTime, float shakeSize)
    {
        StartCoroutine(DoShake(shakeTime, shakeSize));
    }

    /// <summary>
    /// �h��̎��ԂƑ傫���������Ƃ��A�J������h�炷�R���[�`��
    /// </summary>
    /// <param name="shakeTime">�h��̎���</param>
    /// <param name="shakeSize">�h��̑傫��</param>
    /// <returns></returns>
    IEnumerator DoShake(float shakeTime, float shakeSize)
    {
        Vector3 cameraPos = cameraTransform.position; //�h��J�n���̃J�����ʒu��ۑ�      
        float time = 0.0f;

        //�w�肵�����Ԃ��o�߂���܂ŗh�炵������
        while (shakeTime > time)
        {
            //�J�����������_���ɗh�炷
            float x = cameraPos.x + Random.Range(-1.0f, 1.0f) * shakeSize;
            float y = cameraPos.y + Random.Range(-1.0f, 1.0f) * shakeSize;
            cameraTransform.position = new Vector3(x, y, cameraPos.z);

            time += Time.deltaTime;

            if (Time.timeScale == 0) { yield break; } //�J�����h�ꒆ�Ɏ��Ԃ��~�߂�Ɨh�ꑱ����̂�h��

            yield return null;
        }
        //�J�����������ʒu�ɖ߂�
        cameraTransform.position = cameraPos;
    }


    /// <summary>
    /// ��ʂ��t���b�V������R���[�`�����Ăяo�����\�b�h
    /// </summary>
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    /// <summary>
    /// ��ʂ��t���b�V������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator DoFlash()
    {
        float maxAlpha = 0.6f; //��ʂ�^�����ɂ���ƖڂɈ������߁A������x�̓����x����n�߂�

        //���p�l����\��
        GameObject flashObj = flash.gameObject;
        flashObj.SetActive(true);
        flash.color = new Color(1, 1, 1, maxAlpha);

        //���X�ɓ����x��������
        for (float i = 0; i < maxAlpha; i += 0.01f)
        {
            flash.color = new Color(1, 1, 1, maxAlpha - i);
            yield return null;
        }

        //���p�l�����\��
        flash.color = new Color(1, 1, 1, 0);
        flashObj.SetActive(false);
    }


    /// <summary>
    /// �Ə��𓧖��ɂ��ăJ�[�\����\�����郁�\�b�h
    /// </summary>
    public void DisplayCursor()
    {
        aimImage.color = new Color(1, 1, 1, 0);
        Cursor.visible = true;
    }

    /// <summary>
    /// �Ə���s�����ɂ��ăJ�[�\�����\���ɂ��郁�\�b�h
    /// </summary>
    public void DisplayAimImage()
    {
        aimImage.color = new Color(1, 1, 1, 1);
        Cursor.visible = false;
    }


    /// <summary>
    /// �w�肳�ꂽ���Ԃ̌�I�u�W�F�N�g��j�󂷂�R���[�`���A���Ăяo�����\�b�h
    /// </summary>
    /// <param name="obj">�j�󂷂�I�u�W�F�N�g</param>
    /// <param name="destroyTime">�j��܂ł̎���</param>
    public void DestroyObj(GameObject obj, float destroyTime)
    {
        StartCoroutine(DestObj(obj, destroyTime));
    }

    /// <summary>
    /// �w�肳�ꂽ���Ԃ̌�A�I�u�W�F�N�g��j�󂷂�R���[�`��
    /// </summary>
    /// <param name="obj">�j�󂷂�I�u�W�F�N�g</param>
    /// <param name="destroyTime">�j��܂ł̎���</param>
    /// <returns></returns>
    IEnumerator DestObj(GameObject obj, float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(obj);
    }

    /// <summary>
    /// �G�̒e��S�ď������\�b�h
    /// </summary>
    public void DestroyEnemyBullet()
    {
        foreach (Transform childTransform in enemyBulletManagerTransform) //�S�Ă̓G�̒e�I�u�W�F�N�g��Destroy
        {
            Destroy(childTransform.gameObject);
        }
    }

    /// <summary>
    /// �v���C���[�̃L�����𑝂₷���\�b�h
    /// </summary>
    public void KillCountUp()
    {
        playerScript.killCount += 1;
    }

    /// <summary>
    /// �Ó]������A�����҂��Ă���V�[���J�ڂ���R���[�`��
    /// </summary>
    /// <param name="transitionNumber"></param>
    /// <returns></returns>
    public IEnumerator BlackOutAndTransition(int transitionNumber)
    {
        //���p�l����\��
        GameObject blackPanelObj = this.blackPanel.gameObject;
        blackPanelObj.SetActive(true);
        Image blackPanel = this.blackPanel;

        //���p�l���𓧖���
        blackPanel.color = new Color(1, 1, 1, 0);

        //���p�l���̓����x�����X�ɉ�����
        for (float i = 0; i < 1; i += 0.008f)
        {
            blackPanel.color = new Color(1, 1, 1, i);
            yield return null;
        }

        //���p�l����s������
        blackPanel.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2.0f);

        //�����҂��Ă����ʑJ��
        TransitionStage(transitionNumber);
    }

    /// <summary>
    /// ��ʑJ�ڂ��郁�\�b�h
    /// </summary>
    public void TransitionStage(int transitionStageNumber)
    {
        //�����ɂ���đJ�ڐ��ω�
        switch (transitionStageNumber)
        {
            //�Q�[���I�����܂����H�̃��j���[
            case 0:
                endGameSelectUI.SetActive(true);
                break;

            //�G���g�����X�֑J�ڂ��A�f�[�^��������
            case 1:
                SceneManager.LoadScene("Entrance");
                GameDataInitialize();
                ItemAndGunData.GunsListInitialize();
                break;

            //�`���[�g���A���֑J�ڂ��A�f�[�^��������
            case 2:
                SceneManager.LoadScene("Tutorial");
                GameDataInitialize();
                ItemAndGunData.GunsListInitialize();
                break;

            //�X�e�[�W�P�֑J�ڂ��A�f�[�^��������
            case 3:
                SceneManager.LoadScene("Stage1");
                GameDataInitialize();
                ItemAndGunData.GunsListInitialize();
                break;

            //�X�e�[�W�Q�֑J�ڂ��A�f�[�^���X�V
            case 4:
                SceneManager.LoadScene("Stage2");
                GameDataUpdate();
                break;

            //�X�e�[�W�R�֑J�ڂ��A�f�[�^���X�V
            case 5:
                SceneManager.LoadScene("Stage3");
                GameDataUpdate();
                break;

            case 6: //�S�[���֑J�ڂ��A�f�[�^���X�V
                SceneManager.LoadScene("Goal");
                GameDataUpdate();
                break;
        }
    }

    /// <summary>
    /// �Q�[���f�[�^�̏��������s�����\�b�h
    /// </summary>
    void GameDataInitialize()
    {
        GameData.playerHP = 20; //�`���[�g���A���ɍ��킹�č����ݒ�i�ő�l�𒴂����玩���I�ɍő�l�ɂȂ�j
        GameData.holdCoin = 0;
        GameData.killCount = 0;
        GameData.hour = 0;
        GameData.minite = 0;
        GameData.seconds = 0;

        GameData.equipGunNumbers = new int[2] { 1, 2 }; //�莝���̕����2�A���������1��2�i�n���h�K���ƃ\�[�h�I�t�V���b�g�K���j
        GameData.holdGunNumbers = new int[20]; //�T���̕���͂Ƃ肠����20�܂�

        GameData.equipGunRemainBullets = new int[2] { 100000, 150 };//�莝���̕���̎c�e�A�����̎c�e��10����150�i�n���h�K���ƃ\�[�h�I�t�V���b�g�K���j
        GameData.holdGunRemainBullets = new int[20]; //�T���̕���̎c�e
    }

    /// <summary>
    /// �Q�[���f�[�^�̍X�V���s�����\�b�h
    /// </summary>
    void GameDataUpdate()
    {
        GameData.playerHP = playerScript.playerHP;
        GameData.holdCoin = playerScript.holdCoin;
        GameData.killCount = playerScript.killCount;
        GameData.hour = ElapsedTime()[0];
        GameData.minite = ElapsedTime()[1];
        GameData.seconds = ElapsedTime()[2];

        gunManager.SetGunData();
    }

    /// <summary>
    /// �Q�[���N���A���̃R���[�`�����Ăԃ��\�b�h
    /// </summary>
    public void CallClearGame()
    {
        StartCoroutine(ClearGame());
    }

    /// <summary>
    /// �Q�[���N���A���̃R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator ClearGame()
    {
        keyInputManager.noControl = true;
        yield return new WaitForSeconds(2.0f);

        GameObject blackPanelObj = blackPanel.gameObject;

        //���p�l����\�����ē�����
        blackPanelObj.SetActive(true);
        blackPanel.color = new Color(1, 1, 1, 0);

        //���p�l���̓����x�����X�ɉ�����
        for (float i = 0; i < 1; i += 0.002f)
        {
            blackPanel.color = new Color(1, 1, 1, i);
            yield return null;
        }

        //���p�l����\��
        blackPanel.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2.0f);

        GameClear.SetActive(true);
    }


    /// <summary>
    /// �Q�[�����I�����郁�\�b�h
    /// </summary>
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }

}