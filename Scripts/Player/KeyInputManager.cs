using UnityEngine;

/// <summary>
/// キー入力を管理するクラス
/// </summary>
public class KeyInputManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    Player player;
    MiniMap miniMap;

    /// <summary>
    /// 外部からnoControlをtrueにすることによって操作できなくする変数
    /// </summary>
    public bool noControl = false;
    bool entrance;

    void Start()
    {
        player = gameManager.playerScript;
        miniMap = gameManager.miniMap;
        entrance = gameManager.entrance;
    }

    //Eキーのアイテム取得はItemスクリプト、RキーのリロードはGunスクリプトのまま
    //アイテムと銃はいくつもスクリプトがあって取得が大変だから
    void Update()
    {
        if (noControl)
        {
            player.StopPlayer(); //noControlがtrueならプレイヤーを停止、これがないと一方向に移動し続ける
        }

        if (noControl) { return; } //noControlがtrueなら操作できないように

        player.MovePlayer(); //プレイヤーを移動するメソッド
        player.MoveCamera(); //カメラを移動するメソッド

        //スペースキーかレフトシフトキーが押されたらローリング
        //自分のPCだと A,W,Space と A,S,Spaceの組み合わせが押せなかった
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            player.RollingPlayer(); //ローリングするメソッド
        }

        //エントランスなら、マップ、ボム、武器選択、操作確認を無効
        if (entrance) { return; }

        //Mで全体マップを開く
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.ChangeMapCamera(); //全体マップとミニマップを切り替えるメソッド
        }

        //ホイールクリックでボム使用
        if (Input.GetMouseButtonDown(2))
        {
            player.UseBomb(); //ボムを使用するメソッド
        }

        //タブクリックで武器選択画面を開く
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            player.ActiveGunSelectUI(); //武器選択画面を開く/閉じるメソッド
        }

        //エスケープクリックで操作確認画面を開く
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.ActiveOperationsConfirm();　//操作確認画面を開く/閉じるメソッド
        }
    }
}
