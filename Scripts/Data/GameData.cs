/// <summary>
/// �V�[���Ԃ̃Q�[���f�[�^�������p�����߁A�ꎞ�I�Ƀf�[�^��ۑ����Ă����N���X
/// </summary>
public static class GameData
{
    public static int playerHP = 10;
    public static int holdCoin = 0;
    public static int killCount = 0;
    public static int hour = 0;
    public static int minite = 0;
    public static int seconds = 0;

    public static int[] equipGunNumbers = new int[2] { 1, 2 }; //�莝���̕����2�A���������1��2�i�n���h�K���ƃ\�[�h�I�t�V���b�g�K���j
    public static int[] holdGunNumbers = new int[20];//�T���̕���͂Ƃ肠����20�܂�

    public static int[] equipGunRemainBullets = new int[2] { 100000, 150 };//�莝���̕���̎c�e�A�����̎c�e��10����150�i�n���h�K���ƃ\�[�h�I�t�V���b�g�K���j
    public static int[] holdGunRemainBullets = new int[20]; //�T���̕���̎c�e
}
