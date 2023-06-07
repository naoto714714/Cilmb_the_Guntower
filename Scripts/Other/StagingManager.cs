using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Calc;

//�{�X��J�n���ƏI�����̉��o���Ǘ�����N���X
public class StagingManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject bossStagingStart;
    [SerializeField] GameObject bossStaging;
    [SerializeField] string bossName;
    [SerializeField] TextMeshProUGUI bossStagingNameText;

    Transform UIManagerTransform;
    Slider bossHPSlider;
    TextMeshProUGUI bossNameText;

    KeyInputManager keyInputManager;
    UIManager uiManager;

    Transform cameraTransform;

    List<int> activeList = new List<int>();

    bool stagingStart = false;
    bool skip = false;
    bool dead = false;

    BGMManager BGMManager;
    AudioSource audioSourceBGM;
    SEManager SEManager;
    AudioSource audioSourceSE;


    void Start()
    {
        UIManagerTransform = gameManager.UIMGR.transform;
        keyInputManager = gameManager.keyInputManager;
        uiManager = gameManager.uiManager;
        cameraTransform = gameManager.cameraTransform;

        BGMManager = gameManager.BGMManager;
        audioSourceBGM = gameManager.audioSourceBGM;

        SEManager = gameManager.SEManager;
        audioSourceSE = gameManager.audioSourceSE;
    }

    void Update()
    {
        if (!stagingStart) { return; }
        if (skip) { return; }
        
        //�{�X��J�n���̉��o�͍��N���b�N�ŃX�L�b�v�ł���悤�ɂ���
        if (Input.GetMouseButtonDown(0))
        {
            skip = true;
            gameManager.bossAttackAnimeOff = true;
            EndStaging();
        }
    }

    /// <summary>
    /// �{�X��J�n���̉��o�p�̃��\�b�h
    /// </summary>
    /// <param name="areaPos">���o������X�e�[�W�̒��S�̍��W</param>
    public void StagingBossArea(Vector2 areaPos)
    {
        stagingStart = true;
        
        //�v���C���[�𓮂��Ȃ��悤��
        keyInputManager.noControl = true;
        gameManager.noShoot = true;

        bossHPSlider = gameManager.bossHPSlider;
        bossNameText = gameManager.bossNameText;

        audioSourceBGM.Stop(); //BGM���ꎞ��~�i���o�I����Ƀ{�XBGM�ɕύX���ė����j

        bossStagingNameText.text = "VS. " + bossName;
        
        //�S�Ă�UI���ꎞ�I�ɑ��̕ϐ��Ɋi�[���āA��\���ɂ���i���o�I����ɁAUI���Ăѕ\�����邽�߁j
        int i = 0;
        foreach (Transform child in UIManagerTransform)
        {
            if (child.gameObject.activeSelf)
            {
                activeList.Add(i);
                child.gameObject.SetActive(false);
            }
            child.gameObject.SetActive(false);
            i += 1;
        }

        StartCoroutine(MoveCameraToBoss(areaPos, 1)); //�J���������X�Ƀ{�X�̕��ɓ�����
    }


    /// <summary>
    /// �J���������X�Ƀ{�X�̕��ɓ������R���[�`��
    /// </summary>
    /// <param name="areaPos">�J�����𓮂����ړI�n</param>
    /// <param name="type">���o�̃^�C�v</param>
    /// <returns></returns>
    IEnumerator MoveCameraToBoss(Vector2 areaPos, int type)
    {
        if (skip) { yield break; }
        Vector2 cameraPos = cameraTransform.position;

        Vector2 vector = Calculate.Vector(cameraPos, areaPos);

        int addNumber = 100;
        Vector2 addVector = new Vector2(vector.x / addNumber, vector.y / addNumber);

        //�J������ړI�n�̕��֓����悤�ɁA�|�W�V������100���̂P�������Ă���
        for (int i = 0; i < addNumber; i++)
        {
            cameraTransform.position += new Vector3(addVector.x, addVector.y);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.0f);

        //���o�̃^�C�v���P�Ȃ�A�J�������{�X�Ɍ������Ƃ��ɁA�U���A�j�����Đ�����
        if (type == 1)
        {
            if (skip) { yield break; }
            gameManager.bossAttackAnime = true;

            yield return new WaitForSeconds(2.0f);

            StartCoroutine(ActiveBossStagingStart());
        }
    }


    /// <summary>
    /// �p�l���P��\�����A���̃p�l�������X�ɑ傫������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveBossStagingStart()
    {
        if (skip) { yield break; }

        bossStagingStart.SetActive(true);
        bossStagingStart.transform.localScale = new Vector2(1, 1);

        AudioClip stagingSE = SEManager.SE_BossStaging;
        audioSourceSE.PlayOneShot(stagingSE);

        yield return new WaitForSeconds(0.5f);

        //UI�̕\�������X�ɑ傫������
        for (int i = 1; i < 20; i++)
        {
            bossStagingStart.transform.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        bossStagingStart.SetActive(false);
        StartCoroutine(ActiveBossStaging());
    }

    /// <summary>
    /// �p�l���P��\��������A�p�l���Q�����΂炭�\������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveBossStaging()
    {
        if (skip) { yield break; }

        bossStaging.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        EndStaging();
    }

    /// <summary>
    /// �{�X��J�n�̉��o���I���Ƃ��̃��\�b�h
    /// </summary>
    void EndStaging()
    {
        skip = true;
        
        //������悤��
        stagingStart = false;
        keyInputManager.noControl = false;
        
        bossStaging.SetActive(false);
        bossHPSlider.gameObject.SetActive(true);
        bossNameText.gameObject.SetActive(true);

        //��\���ɂ���UI���Ăѕ\������
        int listLength = activeList.Count;
        for (int i = 0; i < listLength; i++)
        {
            GameObject activeObject = UIManagerTransform.GetChild(activeList[i]).gameObject;
            activeObject.SetActive(true);
        }
        uiManager.CallUpdateHeartUI(); //HP��UI�͈�x��A�N�e�B�u�ɂ���Ə���������邩��A������UI���X�V����

        gameManager.bossMoveStart = true;
        Invoke("CanShoot", 0.6f); //���o���I��������ƁA�����̊ԏe�����ĂȂ��悤�Ɂi�X�L�b�v�����Ƃ��ɏe�������Ă��܂��̂�h���j

        AudioClip bossBGM = BGMManager.BGM_Boss;
        audioSourceBGM.clip = bossBGM;
        audioSourceBGM.Play(); //���o���I�������{�XBGM�𗬂�
    }

    /// <summary>
    /// �v���C���[���e�����Ă�ɂ悤�ɂ��郁�\�b�h
    /// </summary>
    void CanShoot()
    {
        gameManager.noShoot = false;
    }

    /// <summary>
    /// �{�X��|�����Ƃ��̉��o�̃R���[�`��
    /// </summary>
    /// <param name="boss">�{�X�̃I�u�W�F�N�g</param>
    /// <param name="explosion">�����̃v���n�u</param>
    /// <returns></returns>
    public IEnumerator DeadBoss(GameObject boss, GameObject explosion)
    {
        if (dead) { yield break; }

        dead = true;
        skip = false;

        //�����Ȃ��悤��
        keyInputManager.noControl = true;
        gameManager.noShoot = true;
        
        audioSourceBGM.Stop(); //BGM���~

        //�{�X�̃|�W�V�������擾���A�J�������{�X�̕��ɏ��X�ɓ�����
        Vector2 bossPos = boss.transform.position;
        StartCoroutine(MoveCameraToBoss(bossPos, 2)); 

        yield return new WaitForSeconds(3.0f);

        //�{�X���\���ɂ��Ĕ���������
        //�����Ń{�X���f�X�g���C����ƁA���̃^�C�~���O�ŕ󔠂��o�����Ă��܂�
        boss.GetComponent<Renderer>().enabled = false;
        explosion.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        //�v���C���[�𓮂���悤�ɂ��āA�{�X���f�X�g���C
        Destroy(boss);
        keyInputManager.noControl = false;
        gameManager.noShoot = false;

        AudioClip exploreBGM = BGMManager.BGM_Explore;
        audioSourceBGM.clip = exploreBGM;
        audioSourceBGM.Play(); //���o���I�������ʏ�BGM�𗬂�
    }
}