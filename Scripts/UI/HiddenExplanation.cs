using UnityEngine;

/// <summary>
/// チュートリアル時に操作を表示するUIを非表示にするクラス
/// </summary>
public class HiddenExplanation : MonoBehaviour
{
    /// <summary>
    /// 非表示にしたいUI
    /// </summary>
    [SerializeField] Canvas explanationUI;
    /// <summary>
    /// 初期に表示するUIの場合はチェックを入れる
    /// </summary>
    [SerializeField] bool activeCheck;

    void OnEnable()
    {
        //起動時に操作を表示するUIを非表示にする
        explanationUI.gameObject.SetActive(false);
    }

    //StartはOnEnableより後に呼ばれる
    //初期に表示するUIにはactiveCheckを入れる
    void Start()
    {
        if (activeCheck)
        {
            //activeCheckがtrueの場合は、初期にそのUIを表示する
            explanationUI.gameObject.SetActive(true);
        }
    }
}
