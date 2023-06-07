using UnityEngine;
using System.Collections;

//����I����ʂ̏e���Ƃ̉摜���Ǘ�����N���X
public class GunSelectUIArrow : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int type;

    Animator myAnime;
    Collider2D myCollider;

    GunSelectUIManager gunSelectUIManagerScript;

    AudioClip cursorSE;
    AudioSource audioSource;


    void Awake()
    {
        gunSelectUIManagerScript = gameManager.gunSelectUIManager;

        myAnime = gameObject.GetComponent<Animator>();
        myCollider = gameObject.GetComponent<Collider2D>();

        cursorSE = gameManager.SEManager.SE_GunSelectCursor;
        audioSource = gameManager.audioSourceSE;
    }


    void OnMouseOver()
    {
        if (!myCollider.enabled) { return; }
        if (gunSelectUIManagerScript.isDrag) { return; }
        myAnime.SetTrigger("Over");

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(cursorSE);
            switch (type)
            {
                case 1:
                    gunSelectUIManagerScript.Page1();
                    break;

                case 2:
                    gunSelectUIManagerScript.Page2();
                    break;
            }
        }
    }

    void OnMouseExit()
    {
        if (!myCollider.enabled) { return; }
        myAnime.SetTrigger("Active");
    }

    /// <summary>
    /// ����𖳌��ɂ��A�P�t���[���J���Ă��疳���̃A�j�����Đ����郁�\�b�h
    /// �P�t���[���J���Ă��炶��Ȃ��ƁAMouseOver�̃A�j���̕����D�悳���
    /// </summary>
    public void NormalArrow()
    {
        myCollider.enabled = false;
        StartCoroutine(NormalAnime());
    }

    /// <summary>
    /// �P�t���[���J���Ă��画���L���ɂ��A�L���̃A�j�����Đ����郁�\�b�h
    /// �P�t���[���J���Ă��炶��Ȃ��ƁAGetComponent���Ԃɍ���Ȃ�
    /// </summary>
    public void ActiveArrow()
    {
        StartCoroutine(ActiveCollider());
    }

    //�{�^�������̃A�j�����P�t���[���J���Ă���I���ɂ���R���[�`��
    IEnumerator NormalAnime()
    {
        yield return null;
        myAnime.SetTrigger("Normal");
    }

    //�P�t���[���J���Ă���{�^����L���ɂ���R���[�`��
    IEnumerator ActiveCollider()
    {
        yield return null;
        myCollider.enabled = true;
        myAnime.SetTrigger("Active");
    }
}
