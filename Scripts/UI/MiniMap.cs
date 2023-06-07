using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// ミニマップを管理するクラス、エリアごと・ロードごとにマップを生成する（エリアの数＋ロードの数＝マップの数）
/// </summary>
public class MiniMap : MonoBehaviour
{
    [SerializeField] Transform areas;
    [SerializeField] Transform roads;
    [SerializeField] Canvas areaMapCanvas;
    [SerializeField] Canvas roadMapCanvas;
    [SerializeField] Image mapImageNone;

    //ミニマップの色
    [SerializeField] Color wallColor;
    [SerializeField] Color groundColor;
    [SerializeField] Color stairsColor;
    [SerializeField] Color noneColor;

    //ミニマップと全体マップ用のカメラ
    [SerializeField] GameObject miniMapCamera;
    [SerializeField] GameObject wholeMapCamera;

    // 余白、余白を入れないとマップが汚くなる
    [SerializeField] int space = 20;

    // マップ用テクスチャ
    Texture2D texture;

    /// <summary>
    /// 外部からnoOpenMapをtrueにすることによってマップを開けないようにする変数
    /// </summary>
    public bool noOpenMap = false;
    bool wholeCameraOn = false;

    void Start()
    {
        CreateMap(areas, areaMapCanvas); //エリアのマップ生成
        CreateMap(roads, roadMapCanvas); //ロードのマップ生成
    }


    /// <summary>
    /// マップを生成するメソッド
    /// </summary>
    /// <param name="maps">作成したいマップが格納されている親オブジェクトのトランスフォーム</param>
    /// <param name="parent">作成したマップイメージを格納する親キャンバス</param>
    void CreateMap(Transform maps, Canvas parent)
    {
        foreach (Transform map in maps)
        {
            //床と壁のタイルマップを読み込む
            Tilemap floorTilemap = map.Find("Floor").GetComponent<Tilemap>();
            Tilemap wallTilemap = map.Find("Wall").GetComponent<Tilemap>();

            //階段があるマップなら階段タイルを読み込む
            Tilemap stairsTilemap = null;
            bool stairsCheck = false;
            if (map.Find("Stairs"))
            {
                stairsCheck = true;
                stairsTilemap = map.Find("Stairs").GetComponent<Tilemap>();
            }

            //テクスチャ作成
            Vector3Int size = new Vector3Int(wallTilemap.size.x + space * 2, wallTilemap.size.y + space * 2, wallTilemap.size.z);
            texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

            //画像をぼやけなくする
            texture.filterMode = FilterMode.Point;

            Vector3Int origin = new Vector3Int(wallTilemap.origin.x - space, wallTilemap.origin.y - space, wallTilemap.origin.z);

            // テクスチャ座標ごとの色を求める
            for (int y = 0; y < size.y; ++y)
            {
                for (int x = 0; x < size.x; ++x)
                {
                    // Tilemapのグリッド座標
                    Vector3Int cellPos = new Vector3Int(origin.x + x, origin.y + y, 0);

                    // 壁タイルが存在する
                    if (wallTilemap.GetTile(cellPos) != null)
                    {
                        texture.SetPixel(x, y, wallColor);
                    }
                    // 地面タイルが存在する
                    else if (floorTilemap.GetTile(cellPos) != null)
                    {
                        texture.SetPixel(x, y, groundColor);
                    }
                    //階段があるなら
                    else if (stairsCheck)
                    {
                        //階段タイルが存在する
                        if (stairsTilemap.GetTile(cellPos) != null)
                        {
                            texture.SetPixel(x, y, stairsColor);
                        }
                        //何もない場所
                        else
                        {
                            texture.SetPixel(x, y, noneColor);
                        }
                    }

                    // なにもない場所
                    else
                    {
                        texture.SetPixel(x, y, noneColor);
                    }
                }
            }

            // テクスチャ確定
            texture.Apply();

            //マップイメージの元となる、空のイメージを作成する
            Image mapImage = Instantiate(mapImageNone);
            mapImage.transform.SetParent(parent.transform);

            // テクスチャをImageに適用
            mapImage.rectTransform.sizeDelta = new Vector2(size.x, size.y);
            mapImage.sprite = Sprite.Create(texture, new Rect(0, 0, size.x, size.y), Vector2.zero);

            // _imageをGridの中心に移動
            Vector2 leftDownWorldPos = wallTilemap.CellToWorld(origin);
            Vector2 rightUpWorldPos = wallTilemap.CellToWorld(origin + size);
            mapImage.transform.position = (leftDownWorldPos + rightUpWorldPos) * 0.5f;

            //読み込んだものがロードなら、読み込んだあとに非アクティブにする
            //そうしないと、ロードの場所がおかしくなる
            if (maps == roads)
            {
                map.gameObject.SetActive(false);
            }

            //宝箱がないエリア（初期エリア等）は、最初からマップを表示し、それ以外はマップを非表示
            StageManager stagemanage = map.gameObject.GetComponent<StageManager>();
            if (stagemanage.noChest) { continue; }
            mapImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ミニマップと全体マップのカメラを切り替えるメソッド
    /// </summary>
    public void ChangeMapCamera()
    {
        if (noOpenMap) { return; }

        //全体マップをミニマップにする
        if (wholeCameraOn)
        {
            wholeCameraOn = false;
            miniMapCamera.SetActive(true);
            wholeMapCamera.SetActive(false);
        }
        //ミニマップを全体マップにする
        else
        {
            wholeCameraOn = true;
            miniMapCamera.SetActive(false);
            wholeMapCamera.SetActive(true);
        }
    }

    /// <summary>
    /// 指定された番号のエリアマップをアクティブにするメソッド
    /// </summary>
    /// <param name="areaNumber"></param>
    public void ActiveArea(int areaNumber)
    {
        GameObject area = areaMapCanvas.transform.GetChild(areaNumber).gameObject;
        area.SetActive(true);
    }

    /// <summary>
    /// 指定された番号のロードマップをアクティブにするメソッド
    /// </summary>
    /// <param name="roadNumber"></param>
    public void ActiveRoad(int roadNumber)
    {
        GameObject road = roadMapCanvas.transform.GetChild(roadNumber - 1).gameObject;
        road.SetActive(true);
    }

    /// <summary>
    /// 一時的にマップを閉じ、開けないようにするメソッド
    /// </summary>
    public void MapOff()
    {
        miniMapCamera.SetActive(false);
        wholeMapCamera.SetActive(false);
        noOpenMap = true;
    }

    /// <summary>
    /// 再びマップを開き、開けるようにするメソッド
    /// </summary>
    public void MapOn()
    {
        miniMapCamera.SetActive(true);
        noOpenMap = false;
    }
}