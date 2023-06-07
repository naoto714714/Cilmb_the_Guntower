using UnityEngine;

/// <summary>
/// ���ʉ����Ǘ�����N���X
/// </summary>
public class SEManager : MonoBehaviour
{
    /// <summary>
    /// �ˌ���_���ʁi�s�X�g�����j
    /// </summary>
    public AudioClip SE_ShotNormal;
    /// <summary>
    /// �ˌ���_�y�i�T�u�}�V���K�����j
    /// </summary>
    public AudioClip SE_ShotLight;
    /// <summary>
    /// �ˌ���_�d�i�V���b�g�K�����j
    /// </summary>
    public AudioClip SE_ShotHeavy;

    /// <summary>
    /// �{���g�p�����Ƃ���SE
    /// </summary>
    public AudioClip SE_Bomb;

    /// <summary>
    /// �_���[�W�󂯂��Ƃ���SE
    /// </summary>
    public AudioClip SE_Damage;
    
    /// <summary>
    /// �����[�h�����Ƃ���SE
    /// </summary>
    public AudioClip SE_Reload;

    /// <summary>
    /// ���[�����O�����Ƃ���SE
    /// </summary>
    public AudioClip SE_Rolling;

    /// <summary>
    /// �w�������Ƃ���SE
    /// </summary>
    public AudioClip SE_Buy;
    public AudioClip SE_CantBuy;


    /// <summary>
    /// �e�I����ʂ��J�����Ƃ���SE
    /// </summary>
    public AudioClip SE_GunSelectOpen;
    /// <summary>
    /// �e�I����ʂ�����Ƃ���SE
    /// </summary>
    public AudioClip SE_GunSelectClose;
    /// <summary>
    /// �e�I����ʂŃJ�[�\�������킹���Ƃ���SE
    /// </summary>
    public AudioClip SE_GunSelectCursor;
    /// <summary>
    /// �e�I����ʂŌ��肵���Ƃ���SE
    /// </summary>
    public AudioClip SE_GunSelectDecide;

    /// <summary>
    /// �Ŕ��J�����Ƃ���SE
    /// </summary>
    public AudioClip SE_SignboardOpen;
    /// <summary>
    /// �Ŕ�����Ƃ���SE
    /// </summary>
    public AudioClip SE_SignboardClose;

    /// <summary>
    /// �_�C�������h���Q�b�g�����Ƃ���SE
    /// </summary>
    public AudioClip SE_GetDiamond;
    /// <summary>
    /// �Q�[���N���A���j���[���J�����Ƃ���SE
    /// </summary>
    public AudioClip SE_GameClearDisplay;

    /// <summary>
    /// �Q�[���I�[�o�[�̃��j���[���J�����Ƃ���SE
    /// </summary>
    public AudioClip SE_GameOverDisplay;
    /// <summary>
    /// �Q�[���I�[�o�[�̃��j���[�ŃN���b�N�����Ƃ���SE
    /// </summary>
    public AudioClip SE_GameOverClick;

    /// <summary>
    /// �󔠏o������SE
    /// </summary>
    public AudioClip SE_ChestAppear;
    /// <summary>
    /// �󔠂��J�����Ƃ���SE
    /// </summary>
    public AudioClip SE_ChestOpen;

    /// <summary>
    /// �A�C�e�����擾�����Ƃ���SE
    /// </summary>
    public AudioClip SE_ItemGet;

    /// <summary>
    /// �G��|���ĕ������J�����Ƃ���SE
    /// </summary>
    public AudioClip SE_RoomOpen;

    /// <summary>
    /// �K�i�ŃV�[���J�ڂ���Ƃ���SE
    /// </summary>
    public AudioClip SE_Stairs;

    /// <summary>
    /// ��������SE
    /// </summary>
    public AudioClip SE_Explosion;

    /// <summary>
    /// �G���U�������Ƃ���SE
    /// </summary>
    public AudioClip SE_EnemyShot;
    /// <summary>
    /// �G��|�����Ƃ���SE
    /// </summary>
    public AudioClip SE_EnemyDown;

    /// <summary>
    /// �{�X��J�n���̉��o�p��SE�i�V���L�[���j
    /// </summary>
    public AudioClip SE_BossStaging;
    /// <summary>
    /// �X�J���L���O���U�������Ƃ���SE1�i���΁j
    /// </summary>
    public AudioClip SE_BossSkullAttack1;
    /// <summary>
    /// �X�J���L���O���U�������Ƃ���SE2(�U���j
    /// </summary>
    public AudioClip SE_BossSkullAttack2;
    /// <summary>
    /// ���K���g���b�N���U�������Ƃ���SE
    /// </summary>
    public AudioClip SE_BossRock;
    /// <summary>
    /// �J�[�X�o�j�[�̉��o�p��SE
    /// </summary>
    public AudioClip SE_BossBunny;
}
