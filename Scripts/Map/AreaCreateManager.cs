using UnityEngine;
using Calc;

/// <summary>
/// ステージの各エリアを生成するクラス
/// </summary>
public class AreaCreateManager : MonoBehaviour
{
    //リソースファイルの中から乱数で選んで生成する

    /// <summary>
    /// ステージの番号
    /// </summary>
    [SerializeField] int stageNumber;
    /// <summary>
    /// ステージのエリア（敵のいる場所）の数
    /// </summary>
    [SerializeField] int areaNumber;
    /// <summary>
    /// ステージのパターンの数
    /// </summary>
    [SerializeField] int patternNumber;

    Transform myTransform;

    void Start()
    {
        myTransform = gameObject.transform;
        string stageName = null;

        //ステージナンバーによって読み込むファイルを変える
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

        //1～patternNumberのリストを作り、そのリストからランダムに数字を選び、それをパターンにする
        int pattern = Calculate.ChooseNumber(Calculate.NumList(1, patternNumber));
        print("パターン：" + pattern);

        //スタートエリア生成
        string loadName = stageName + "/AreaStart/AreaStart-" + pattern;
        CreateArea(loadName);
        
        //通常エリア生成
        //エリアナンバーの数だけ生成する
        for (int i = 1; i <= areaNumber; i++)
        {
            loadName = stageName + "/Area" + i + "/Area" + i + "-" + pattern;
            CreateArea(loadName);
        }

        //ボスエリア生成
        loadName = stageName + "/AreaBoss" + "/AreaBoss-" + pattern;
        CreateArea(loadName);
        
        //ゴールエリア生成
        loadName = stageName + "/AreaGoal" + "/AreaGoal-" + pattern;
        CreateArea(loadName);

        //ショップエリア生成
        loadName = stageName + "/AreaShop" + "/AreaShop-" + pattern;
        CreateArea(loadName);
        
        Resources.UnloadUnusedAssets(); //メモリの開放
    }

    /// <summary>
    /// 引数の名前のエリアを、リソースファイルから読み込み生成するメソッド
    /// </summary>
    /// <param name="loadName"></param>
    void CreateArea(string loadName)
    {
        GameObject stageObj = (GameObject)Resources.Load(loadName);
        GameObject stage = Instantiate(stageObj);
        stage.transform.parent = myTransform;
    }
}
