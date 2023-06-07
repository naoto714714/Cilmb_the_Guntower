using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// �~�j�}�b�v���Ǘ�����N���X�A�G���A���ƁE���[�h���ƂɃ}�b�v�𐶐�����i�G���A�̐��{���[�h�̐����}�b�v�̐��j
/// </summary>
public class MiniMap : MonoBehaviour
{
    [SerializeField] Transform areas;
    [SerializeField] Transform roads;
    [SerializeField] Canvas areaMapCanvas;
    [SerializeField] Canvas roadMapCanvas;
    [SerializeField] Image mapImageNone;

    //�~�j�}�b�v�̐F
    [SerializeField] Color wallColor;
    [SerializeField] Color groundColor;
    [SerializeField] Color stairsColor;
    [SerializeField] Color noneColor;

    //�~�j�}�b�v�ƑS�̃}�b�v�p�̃J����
    [SerializeField] GameObject miniMapCamera;
    [SerializeField] GameObject wholeMapCamera;

    // �]���A�]�������Ȃ��ƃ}�b�v�������Ȃ�
    [SerializeField] int space = 20;

    // �}�b�v�p�e�N�X�`��
    Texture2D texture;

    /// <summary>
    /// �O������noOpenMap��true�ɂ��邱�Ƃɂ���ă}�b�v���J���Ȃ��悤�ɂ���ϐ�
    /// </summary>
    public bool noOpenMap = false;
    bool wholeCameraOn = false;

    void Start()
    {
        CreateMap(areas, areaMapCanvas); //�G���A�̃}�b�v����
        CreateMap(roads, roadMapCanvas); //���[�h�̃}�b�v����
    }


    /// <summary>
    /// �}�b�v�𐶐����郁�\�b�h
    /// </summary>
    /// <param name="maps">�쐬�������}�b�v���i�[����Ă���e�I�u�W�F�N�g�̃g�����X�t�H�[��</param>
    /// <param name="parent">�쐬�����}�b�v�C���[�W���i�[����e�L�����o�X</param>
    void CreateMap(Transform maps, Canvas parent)
    {
        foreach (Transform map in maps)
        {
            //���ƕǂ̃^�C���}�b�v��ǂݍ���
            Tilemap floorTilemap = map.Find("Floor").GetComponent<Tilemap>();
            Tilemap wallTilemap = map.Find("Wall").GetComponent<Tilemap>();

            //�K�i������}�b�v�Ȃ�K�i�^�C����ǂݍ���
            Tilemap stairsTilemap = null;
            bool stairsCheck = false;
            if (map.Find("Stairs"))
            {
                stairsCheck = true;
                stairsTilemap = map.Find("Stairs").GetComponent<Tilemap>();
            }

            //�e�N�X�`���쐬
            Vector3Int size = new Vector3Int(wallTilemap.size.x + space * 2, wallTilemap.size.y + space * 2, wallTilemap.size.z);
            texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

            //�摜���ڂ₯�Ȃ�����
            texture.filterMode = FilterMode.Point;

            Vector3Int origin = new Vector3Int(wallTilemap.origin.x - space, wallTilemap.origin.y - space, wallTilemap.origin.z);

            // �e�N�X�`�����W���Ƃ̐F�����߂�
            for (int y = 0; y < size.y; ++y)
            {
                for (int x = 0; x < size.x; ++x)
                {
                    // Tilemap�̃O���b�h���W
                    Vector3Int cellPos = new Vector3Int(origin.x + x, origin.y + y, 0);

                    // �ǃ^�C�������݂���
                    if (wallTilemap.GetTile(cellPos) != null)
                    {
                        texture.SetPixel(x, y, wallColor);
                    }
                    // �n�ʃ^�C�������݂���
                    else if (floorTilemap.GetTile(cellPos) != null)
                    {
                        texture.SetPixel(x, y, groundColor);
                    }
                    //�K�i������Ȃ�
                    else if (stairsCheck)
                    {
                        //�K�i�^�C�������݂���
                        if (stairsTilemap.GetTile(cellPos) != null)
                        {
                            texture.SetPixel(x, y, stairsColor);
                        }
                        //�����Ȃ��ꏊ
                        else
                        {
                            texture.SetPixel(x, y, noneColor);
                        }
                    }

                    // �Ȃɂ��Ȃ��ꏊ
                    else
                    {
                        texture.SetPixel(x, y, noneColor);
                    }
                }
            }

            // �e�N�X�`���m��
            texture.Apply();

            //�}�b�v�C���[�W�̌��ƂȂ�A��̃C���[�W���쐬����
            Image mapImage = Instantiate(mapImageNone);
            mapImage.transform.SetParent(parent.transform);

            // �e�N�X�`����Image�ɓK�p
            mapImage.rectTransform.sizeDelta = new Vector2(size.x, size.y);
            mapImage.sprite = Sprite.Create(texture, new Rect(0, 0, size.x, size.y), Vector2.zero);

            // _image��Grid�̒��S�Ɉړ�
            Vector2 leftDownWorldPos = wallTilemap.CellToWorld(origin);
            Vector2 rightUpWorldPos = wallTilemap.CellToWorld(origin + size);
            mapImage.transform.position = (leftDownWorldPos + rightUpWorldPos) * 0.5f;

            //�ǂݍ��񂾂��̂����[�h�Ȃ�A�ǂݍ��񂾂��Ƃɔ�A�N�e�B�u�ɂ���
            //�������Ȃ��ƁA���[�h�̏ꏊ�����������Ȃ�
            if (maps == roads)
            {
                map.gameObject.SetActive(false);
            }

            //�󔠂��Ȃ��G���A�i�����G���A���j�́A�ŏ�����}�b�v��\�����A����ȊO�̓}�b�v���\��
            StageManager stagemanage = map.gameObject.GetComponent<StageManager>();
            if (stagemanage.noChest) { continue; }
            mapImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �~�j�}�b�v�ƑS�̃}�b�v�̃J������؂�ւ��郁�\�b�h
    /// </summary>
    public void ChangeMapCamera()
    {
        if (noOpenMap) { return; }

        //�S�̃}�b�v���~�j�}�b�v�ɂ���
        if (wholeCameraOn)
        {
            wholeCameraOn = false;
            miniMapCamera.SetActive(true);
            wholeMapCamera.SetActive(false);
        }
        //�~�j�}�b�v��S�̃}�b�v�ɂ���
        else
        {
            wholeCameraOn = true;
            miniMapCamera.SetActive(false);
            wholeMapCamera.SetActive(true);
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�ԍ��̃G���A�}�b�v���A�N�e�B�u�ɂ��郁�\�b�h
    /// </summary>
    /// <param name="areaNumber"></param>
    public void ActiveArea(int areaNumber)
    {
        GameObject area = areaMapCanvas.transform.GetChild(areaNumber).gameObject;
        area.SetActive(true);
    }

    /// <summary>
    /// �w�肳�ꂽ�ԍ��̃��[�h�}�b�v���A�N�e�B�u�ɂ��郁�\�b�h
    /// </summary>
    /// <param name="roadNumber"></param>
    public void ActiveRoad(int roadNumber)
    {
        GameObject road = roadMapCanvas.transform.GetChild(roadNumber - 1).gameObject;
        road.SetActive(true);
    }

    /// <summary>
    /// �ꎞ�I�Ƀ}�b�v����A�J���Ȃ��悤�ɂ��郁�\�b�h
    /// </summary>
    public void MapOff()
    {
        miniMapCamera.SetActive(false);
        wholeMapCamera.SetActive(false);
        noOpenMap = true;
    }

    /// <summary>
    /// �Ăу}�b�v���J���A�J����悤�ɂ��郁�\�b�h
    /// </summary>
    public void MapOn()
    {
        miniMapCamera.SetActive(true);
        noOpenMap = false;
    }
}