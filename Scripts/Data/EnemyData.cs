using UnityEngine;

namespace Data
{
    //�G�̃f�[�^���Ǘ�����f�[�^
    class EnemyData : MonoBehaviour
    {
        static float[] enemyData;

        /// <summary>
        /// �G�l�~�[�i���o�[���ƂɃZ�b�g����G�̃f�[�^��ς��郁�\�b�h
        /// </summary>
        /// <param name="classNumber">�G���Ƃ̕��ޔԍ�</param>
        public static float[] SetEnemyData(int enemyClassNumber)
        {
            switch (enemyClassNumber)
            {
                //�G��HP�A�������x�A�������ԁA�����I����Ď~�܂鎞�ԁA
                //�U�����Ă���͈́A�U�����Ă���Ԋu�A�U���񐔁A��x�̔��ː��A�U�����̔��ˊԊu
                //�e�ۂ̑��x�A�e�̃_���[�W�A�ʂ̑傫���A
                //�ő�u���p�x�A���b��ɒe�������i�A�j���[�V�����ɍ��킹�邽�߁j�A
                //�����_���Ɉړ����邩�A�U�����ɕK���ړ�����悤�ɂ��邩�A�U�����ɕK����~����悤�ɂ��邩
                case 1: //�g�����N
                    enemyData = new float[]
                    { 100, 1.5f, 2.0f, 2.0f, 
                        8, 3.0f, 1, 1, 0,
                        5.0f, 1, 1, 
                        0, 0.6f, 
                        1, 0, 0};
                    break;

                case 2: //�s�b�O
                    enemyData = new float[]
                    { 120, 3.5f, 0.3f, 0, 
                        0, 0, 0, 0, 0, 
                        0, 0, 0, 
                        0, 0, 
                        0, 0, 0};
                    break;

                case 3: //�v�����g
                    enemyData = new float[]
                    { 70, 0.0f, 0.0f, 0.0f, 
                        100, 3.0f, 3, 6, 1.0f,
                        4.0f, 1, 1,
                        30, 1.2f, 
                        0, 0, 0};
                    break;

                case 4: //���C�m
                    enemyData = new float[]
                    { 150, 6.0f, 2.0f, 2.0f,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0, 
                        0, 0, 0};
                    break;

                case 5: //�r�[
                    enemyData = new float[]
                    { 70, 1.0f, 2.0f, 3.0f,
                        10, 4.0f, 1, 1, 0,
                        12.0f, 1, 1,
                        0, 1.3f, 
                        1, 0, 0 };
                    break;


                case 6: //�^�[�g��
                    enemyData = new float[]
                    { 140, 0.0f, 0.0f, 0.0f,
                        100, 5.0f, 2, 60, 1.5f,
                        4.0f, 1, 1,
                        180, 1.2f, 
                        0, 0, 0};
                    break;

                case 7: //�u���[�o�[�h
                    enemyData = new float[]
                    { 100, 1.0f, 0.3f, 0.0f,
                        8, 2.0f, 1, 1, 0,
                        5.0f, 1, 1,
                        10, 0.0f, 
                        0, 0, 0};
                    break;

                case 8: //�}�b�V�����[��
                    enemyData = new float[]
                    { 120, 3.0f, 0.8f, 3.0f,
                        20, 5.0f, 1, 60, 0,
                        2.0f, 1, 1,
                        180, 0.8f, 
                        1, 0, 0};
                    break;

                case 9: //�X�l�C��
                    enemyData = new float[]
                    { 160, 0.8f, 0.3f, 0.0f,
                        10, 4.0f, 10, 1, 0.2f,
                        6.0f, 1, 1,
                        25, 1.0f,
                        0, 0, 1};
                    break;

                case 10: //�o�b�g
                    enemyData = new float[]
                    { 80, 2.0f, 90f, 1.0f,
                        10, 4.0f, 4, 1, 0.6f,
                        6.0f, 1, 1,
                        5, 1.0f,
                        0, 1, 0};
                    break;

                case 11: //�r�b�O�X���C��
                    enemyData = new float[]
                    { 300,1.0f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 12: //�X���C��
                    enemyData = new float[]
                    { 150, 2.5f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 13: //�~�j�X���C��
                    enemyData = new float[]
                    { 20, 3.5f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 14: //���f�B�b�V��
                    enemyData = new float[]
                    { 150, 1.5f, 2.0f, 2.0f,
                        12, 2.0f, 1, 8, 0,
                        5.0f, 1, 1,
                        20, 0.8f,
                        1, 0, 0};
                    break;

                case 15: //�S�[�X�g
                    enemyData = new float[]
                    { 150, 4.5f, 0.8f, 1.5f,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 16: //�t�@�b�g�o�[�h
                    enemyData = new float[]
                    { 250, 0.0f, 0.0f, 0.0f,
                        15, 1.2f, 1, 1, 0.0f,
                        2.0f, 1, 2.5f,
                        0, 0.0f,
                        0, 0, 0};
                    break;

                case 17: //�`�L��
                    enemyData = new float[]
                    { 150, 6.0f, 2.0f, 2.0f,
                        10, 3, 3, 1, 0.3f,
                        5, 1, 1,
                        10, 0,
                        0, 0, 0};
                    break;

                case 18: //�_�b�N
                    enemyData = new float[]
                    { 100, 6.0f, 2.0f, 2.0f,
                        10, 3, 1, 1, 0,
                        5, 1, 1,
                        10, 0,
                        0, 0, 0};
                    break;

                case 19: //�J�����I��
                    enemyData = new float[]
                    { 500, 6.0f, 1.5f, 2.0f,
                        30, 3, 3, 3, 0.3f,
                        7, 1, 1.2f,
                        15, 0,
                        1, 0, 0};
                    break;

                case 20: //���b�N
                    enemyData = new float[]
                    { 250,1.0f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 21: //�~�j���b�N
                    enemyData = new float[]
                    { 50, 3.5f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 101: //�`���[�g���A���p
                    enemyData = new float[]
                    { 100, 1.5f, 2.0f, 2.0f,
                        0, 3.0f, 1, 1, 0,
                        5.0f, 1, 1,
                        0, 0.6f, 
                        1, 0, 0};
                    break;
            }
            return enemyData;
        }
    }
}

