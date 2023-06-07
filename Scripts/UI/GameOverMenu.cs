using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// �Q�[���I�[�o�[�̃��j���[���Ǘ�����N���X
/// </summary>
public class GameOverMenu : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI killText;
    KeyInputManager keyInputManager;
    Player player;

    SEManager SEManager;
    AudioSource audioSource;
    AudioClip openSE;
    AudioClip clickSE;

    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;
        player = gameManager.playerScript;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GameOverDisplay;
        clickSE = SEManager.SE_GameOverClick;
    }

    /// <summary>
    /// �N�������瓮���Ȃ��悤�ɂ��郁�\�b�h
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE);
        keyInputManager.noControl = true;
        gameManager.DisplayCursor();
        
        //�Q�[���I�[�o�[�ɂȂ����Ƃ��́A���ԁE�R�C���E�X�e�[�W���E���j�����擾
        int[] times = gameManager.ElapsedTime();
        timeText.text = times[0].ToString("0") + ":" + times[1].ToString("00") + ":" + times[2].ToString("00");
        coinText.text = player.holdCoin.ToString();
        stageText.text = gameManager.stageName.text;
        killText.text =  player.killCount.ToString();
    }

    //���X�^�[�g����������
    /// <summary>
    /// �X�e�[�W�P�֑J�ڂ���R���[�`�����Ăяo��
    /// </summary>
    public void Restart()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(gameManager.BlackOutAndTransition(3));
    }

    //�G���g�����X�ւ���������
    /// <summary>
    /// �G���g�����X�փV�[���J�ڂ���R���[�`�����Ăяo��
    /// </summary>
    public void ToEntrance()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(gameManager.BlackOutAndTransition(1));
    }

    //�`���[�g���A���̂�X�^�[�g�̓`���[�g���A������ɂ���
    public void RestartTutorial()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(gameManager.BlackOutAndTransition(2));
    }
}
