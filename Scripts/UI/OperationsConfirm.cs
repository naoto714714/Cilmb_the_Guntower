using UnityEngine;

/// <summary>
/// ����m�F��ʂ��Ǘ�����N���X
/// </summary>
public class OperationsConfirm : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    KeyInputManager keyInputManager;

    GameObject gunUIManager;
    GameObject miniMap;
    GameObject mapPoint;

    SEManager SEManager;
    AudioClip openSE;
    AudioClip closeSE;
    AudioSource audioSource;

    bool wasNoControl = false;
    bool wasNoShoot = false;


    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GunSelectOpen;
        closeSE = SEManager.SE_GunSelectClose;

        gunUIManager = gameManager.gunUIManager;
        miniMap = gameManager.miniMap.gameObject;
        mapPoint = gameManager.mapPoint;
    }


    /// <summary>
    /// ����m�F��ʂ��J�����Ƃ��̃��\�b�h
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE); //����m�F��ʂ��J�����Ƃ���SE

        //�����Ȃ��悤��
        Time.timeScale = 0;
        //���ɓ����Ȃ���ԂȂ������������Ȃ��悤��
        if (keyInputManager.noControl) { wasNoControl = true; }
        else { keyInputManager.noControl = true; }
        //���Ɍ��ĂȂ���ԂȂ����������ĂȂ��悤��
        if (gameManager.noShoot) { wasNoShoot = true; }
        else { gameManager.noShoot = true; }

        //��������UI���\���ɂ���
        gunUIManager.SetActive(false);
        miniMap.SetActive(false);
        mapPoint.SetActive(false);

        //�Ə����\���ɂ��A�J�[�\����\��
        gameManager.DisplayCursor();
    }


    void Update()
    {
        //�G�X�P�[�v�������ꂽ�瑀��m�F��ʂ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.DisplayAimImage(); //�J�[�\�����\���ɂ��A�Ə���\��
            audioSource.PlayOneShot(closeSE); //����m�F��ʂ�����Ƃ��̉�
                                              //
            //��\���ɂ���UI��\��
            gunUIManager.SetActive(true);
            miniMap.SetActive(true);
            mapPoint.SetActive(true);

            //����m�F��ʂ����\���ɂ��A������悤�ɂ���
            gameObject.SetActive(false);
            Time.timeScale = 1;
            if (!wasNoControl) { keyInputManager.noControl = false; }
            if (!wasNoShoot) { gameManager.noShoot = false; }
        }
    }
}
