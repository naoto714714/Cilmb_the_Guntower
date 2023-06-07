using UnityEngine;

/// <summary>
/// �Q�[�����I�����邩�ǂ����̃��j���[�̃N���X
/// </summary>
public class EndGameSelect : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    KeyInputManager keyInputManager;

    SEManager SEManager;
    AudioSource audioSource;
    AudioClip openSE;
    AudioClip closeSE;

    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GunSelectOpen;
        closeSE = SEManager.SE_GunSelectClose;
    }

    /// <summary>
    /// �N����������ʉ���炵�A�����Ȃ��悤�ɂ��郁�\�b�h
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE);
        keyInputManager.noControl = true;
        Time.timeScale = 0;
        gameManager.DisplayCursor(); //�Ə�����J�[�\���ɕύX
    }

    //�͂�����������
    /// <summary>
    /// �Q�[�����I�����郁�\�b�h���Ăяo�����\�b�h
    /// </summary>
    public void EndGameYes()
    {
        gameManager.EndGame();
    }

    //����������������
    /// <summary>
    /// �v���C���[��(0,0)�Ɉړ����A�Ăѓ�����悤�ɂ��郁�\�b�h
    /// </summary>
    public void EndGameNo()
    {
        gameManager.DisplayAimImage();

        audioSource.PlayOneShot(closeSE);

        gameManager.playerTransform.position = new Vector2(0, 0);
        keyInputManager.noControl = false;

        gameObject.SetActive(false);
        Time.timeScale = 1;
        gameManager.noShoot = false;
    }
}
