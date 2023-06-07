using UnityEngine;
using Calc;

/// <summary>
/// �X�e�[�W�̊e�G���A�𐶐�����N���X
/// </summary>
public class AreaCreateManager : MonoBehaviour
{
    //���\�[�X�t�@�C���̒����痐���őI��Ő�������

    /// <summary>
    /// �X�e�[�W�̔ԍ�
    /// </summary>
    [SerializeField] int stageNumber;
    /// <summary>
    /// �X�e�[�W�̃G���A�i�G�̂���ꏊ�j�̐�
    /// </summary>
    [SerializeField] int areaNumber;
    /// <summary>
    /// �X�e�[�W�̃p�^�[���̐�
    /// </summary>
    [SerializeField] int patternNumber;

    Transform myTransform;

    void Start()
    {
        myTransform = gameObject.transform;
        string stageName = null;

        //�X�e�[�W�i���o�[�ɂ���ēǂݍ��ރt�@�C����ς���
        switch (stageNumber)
        {
            case 1:
                stageName = "Stage1";
                break;

            case 2:
                stageName = "Stage2";
                break;

            case 3:
                stageName = "Stage3";
                break;

        }

        //1�`patternNumber�̃��X�g�����A���̃��X�g���烉���_���ɐ�����I�сA������p�^�[���ɂ���
        int pattern = Calculate.ChooseNumber(Calculate.NumList(1, patternNumber));
        print("�p�^�[���F" + pattern);

        //�X�^�[�g�G���A����
        string loadName = stageName + "/AreaStart/AreaStart-" + pattern;
        CreateArea(loadName);
        
        //�ʏ�G���A����
        //�G���A�i���o�[�̐�������������
        for (int i = 1; i <= areaNumber; i++)
        {
            loadName = stageName + "/Area" + i + "/Area" + i + "-" + pattern;
            CreateArea(loadName);
        }

        //�{�X�G���A����
        loadName = stageName + "/AreaBoss" + "/AreaBoss-" + pattern;
        CreateArea(loadName);
        
        //�S�[���G���A����
        loadName = stageName + "/AreaGoal" + "/AreaGoal-" + pattern;
        CreateArea(loadName);

        //�V���b�v�G���A����
        loadName = stageName + "/AreaShop" + "/AreaShop-" + pattern;
        CreateArea(loadName);
        
        Resources.UnloadUnusedAssets(); //�������̊J��
    }

    /// <summary>
    /// �����̖��O�̃G���A���A���\�[�X�t�@�C������ǂݍ��ݐ������郁�\�b�h
    /// </summary>
    /// <param name="loadName"></param>
    void CreateArea(string loadName)
    {
        GameObject stageObj = (GameObject)Resources.Load(loadName);
        GameObject stage = Instantiate(stageObj);
        stage.transform.parent = myTransform;
    }
}
