using UnityEngine;

/// <summary>
/// �`���[�g���A�����ɑ����\������UI���\���ɂ���N���X
/// </summary>
public class HiddenExplanation : MonoBehaviour
{
    /// <summary>
    /// ��\���ɂ�����UI
    /// </summary>
    [SerializeField] Canvas explanationUI;
    /// <summary>
    /// �����ɕ\������UI�̏ꍇ�̓`�F�b�N������
    /// </summary>
    [SerializeField] bool activeCheck;

    void OnEnable()
    {
        //�N�����ɑ����\������UI���\���ɂ���
        explanationUI.gameObject.SetActive(false);
    }

    //Start��OnEnable����ɌĂ΂��
    //�����ɕ\������UI�ɂ�activeCheck������
    void Start()
    {
        if (activeCheck)
        {
            //activeCheck��true�̏ꍇ�́A�����ɂ���UI��\������
            explanationUI.gameObject.SetActive(true);
        }
    }
}
