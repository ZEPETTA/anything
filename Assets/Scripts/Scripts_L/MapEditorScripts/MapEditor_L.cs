using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapEditor_L : MonoBehaviour
{
    public enum ToolType
    {
        None,
        Eraser,
        Stamp,
        Dropper,
        Arrow
    }

    public enum PlacementType
    {
        Floor,
        Object,
        UpperObject,
        TileEffect
    }

    public enum TileEffectType
    {
        Portal,
        DefinedArea,
        Wall,
    }

    public ToolType toolType;
    public PlacementType placementType;
    public TileEffectType tileEffectType;

    public float floorTileZ;
    public float definedAreaZ;
    public float wallTileZ;
    
    public GameObject floorTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject definedAreaPrefab;

    public Texture currClickedTileTexture;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 5f;
    public float scrollSpeed = 0.5f;
    public float arrowDragSpeed = 10f;
    public int width, height;
    public Transform grid;
    public Transform cursorSquare;
    public Transform tileParent;
    public Transform definedAreaParent;
    public Dropdown definedAreaDropdown;
    public InputField inputFieldDefinedAreaName;
    public GameObject canvas;
    Vector3 gridStartPos;

    int tileLayerMask;
    Vector2 pastTilePos;
    Vector2 pastDefinedAreaPos;
    Vector2 pastWallPos;
    Vector2 mouseClickPos;

/*    public LineRenderer gridLineRenderer_X;
    public LineRenderer gridLineRenderer_Y;*/

    // Start is called before the first frame update
    void Start()
    {
        

        toolType = ToolType.Stamp;
        placementType = PlacementType.Floor;
        pastTilePos = Vector2.zero;
        pastDefinedAreaPos = Vector2.zero;
        tileLayerMask = 1 << LayerMask.NameToLayer("Tile");
        gridStartPos = grid.transform.position;
        gridStartPos.x -= grid.transform.localScale.x * 0.5f;
        gridStartPos.y -= grid.transform.localScale.y * 0.5f;


        /*for (int y = 0; y < height; y++)
        {
            //gridLineRenderer_X.positionCount += 2;
            gridLineRenderer_X.SetPosition(y * 2, new Vector3(gridStartPos.x, gridStartPos.y + y, gridStartPos.z));
            gridLineRenderer_X.SetPosition(y * 2 + 1, new Vector3(gridStartPos.x+width, gridStartPos.y + y, gridStartPos.z));
        }*/

        definedAreaDropdown.onValueChanged.AddListener(delegate
        {
            print(definedAreaDropdown.captionText.text);
        });
    }



    // Update is called once per frame
    void Update()
    {
        /*        for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        //lineRenderer.SetPosition()

                        Debug.DrawLine(new Vector3(gridStartPos.x + x, gridStartPos.y + y, gridStartPos.z),
                            new Vector3(gridStartPos.x + x + 1, gridStartPos.y + y, gridStartPos.z), Color.red);
                        Debug.DrawLine(new Vector3(gridStartPos.x + x, gridStartPos.y + y, gridStartPos.z),
            new Vector3(gridStartPos.x + x, gridStartPos.y + y + 1, gridStartPos.z), Color.red);
                    }
                }
        */

        MoveCursorSquare();

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scrollWheel * Time.deltaTime * scrollSpeed;

        if (Camera.main.orthographicSize > maxOrthographicSize)
            Camera.main.orthographicSize = maxOrthographicSize;
        if (Camera.main.orthographicSize < minOrthographicSize)
            Camera.main.orthographicSize = minOrthographicSize;

        switch (toolType)
        {
            case ToolType.Stamp:
                if (placementType == PlacementType.Floor)
                    Stamp();
                else if (placementType == PlacementType.TileEffect)
                    TileEffect();
                break;
            case ToolType.Eraser:
                Erase();
                break;
            case ToolType.Dropper:
                Dropper();
                break;
            case ToolType.Arrow:
                Arrow();
                break;
        }
        
    }
    void TileEffect()
    {
        switch (tileEffectType)
        {
            case TileEffectType.Portal:

                break;
            case TileEffectType.DefinedArea:
                if (inputFieldDefinedAreaName.text.Length < 1)
                {//지정 영역의 이름이 입력되어 있지 않으면 안 함
                    break;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Transform littleParent2 = definedAreaParent.Find(inputFieldDefinedAreaName.text);
                    if (littleParent2 == null)
                    {
                        littleParent2 = Instantiate(gameObject).transform;
                        littleParent2.SetParent(definedAreaParent);
                        littleParent2.localPosition = Vector3.zero;
                        littleParent2.name = inputFieldDefinedAreaName.text;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Transform littleParent = definedAreaParent.Find(inputFieldDefinedAreaName.text);
                    if (littleParent)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            Debug.Log(hitInfo.transform.tag);

                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastDefinedAreaPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }

                            GameObject tile = Instantiate(definedAreaPrefab);
                            tile.transform.SetParent(littleParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            tile.GetComponentInChildren<Text>().text = inputFieldDefinedAreaName.text;

                            pastDefinedAreaPos.x = x;
                            pastDefinedAreaPos.y = y;


                        }

                    }
                }
                break;
            case TileEffectType.Wall:
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastWallPos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        /*                    if(Physics.OverlapBox(new Vector3(x,y,floorTileZ),new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,tileLayerMask).Length>0){
                                                print("Already Exists");
                                                return;
                                                //이미 타일이 있는 경우
                                            }*/
                        GameObject tile = Instantiate(wallTilePrefab);
                        tile.transform.SetParent(GameObject.Find("WallParent").transform);
                        tile.transform.localPosition = new Vector3(x, y, wallTileZ);
                        pastWallPos.x = x;
                        pastWallPos.y = y;
                    }
                }
                break;
        }
    }

    void MoveCursorSquare()
    {
        Vector2 currMousePos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        currMousePos.x = (int)currMousePos.x;
        currMousePos.y = (int)currMousePos.y;

        cursorSquare.position = currMousePos;
    }

    void Arrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClickPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 dir = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - mouseClickPos);
            Vector3 move = dir * arrowDragSpeed * Time.deltaTime;
            Camera.main.transform.Translate(-move);
        }

    }

    void Dropper()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.tag == "Tile")
                {
                    currClickedTileTexture = hitInfo.transform.GetComponent<MeshRenderer>().material.mainTexture;
                    toolType = ToolType.Stamp;
                }
            }
        }
    }

    void Erase()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.tag == "Tile" || hitInfo.transform.tag == "Object")
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }

    void Stamp()
    {
        if (Input.GetMouseButton(0))
        {
            switch (placementType)
            {
                case PlacementType.Floor:
                    CreateTile();
                    break;

            }

        
        
        }
    }

    void CreateTile()
    {
        if (currClickedTileTexture)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.transform.tag);

                if (hitInfo.transform.tag == "BackGround")
                {
                    int x = (int)hitInfo.point.x;
                    int y = (int)hitInfo.point.y;
                    if (pastTilePos == new Vector2(x, y))
                    {
                        print("already exists");
                        return;
                    }
                    /*                    if(Physics.OverlapBox(new Vector3(x,y,floorTileZ),new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,tileLayerMask).Length>0){
                                            print("Already Exists");
                                            return;
                                            //이미 타일이 있는 경우
                                        }*/
                    GameObject tile = Instantiate(floorTilePrefab);
                    tile.transform.SetParent(tileParent);
                    tile.transform.localPosition = new Vector3(x, y, floorTileZ);
                    tile.GetComponent<MeshRenderer>().material.mainTexture = currClickedTileTexture;
                    pastTilePos.x = x;
                    pastTilePos.y = y;

                }
                else if (hitInfo.transform.tag == "Tile")
                {
                    hitInfo.transform.GetComponent<MeshRenderer>().material.mainTexture = currClickedTileTexture;
                }
                //이미 오브젝트 있는지 확인?
                //이전 위치 저장

            }
        }
    }

    public void OnClickBtnFloor()
    {
        placementType = PlacementType.Floor;
        TurnOnUi(1);
    }


    public void OnClickBtnEraser()
    {
        toolType = ToolType.Eraser;
    }

    public void OnClickBtnStamp()
    {
        toolType = ToolType.Stamp;
    }

    public void OnClickBtnDropper()
    {
        toolType = ToolType.Dropper;
    }

    public void OnClickBtnArrow()
    {
        toolType = ToolType.Arrow;
    }

    public void OnClickBtnFloorExample()
    {
        currClickedTileTexture= ConvertSpriteToTexture(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite);
    }
    
    public void OnClickBtnTileEffect()
    {
        placementType = PlacementType.TileEffect;
        TurnOnUi(0);
    }
    void TurnOnUi(int index)
    {
        for(int i =0; i< canvas.transform.childCount -1; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.transform.GetChild(index).gameObject.SetActive(true);
    }
    public void OnClickBtnPortal()
    {
        tileEffectType = TileEffectType.Portal;
    }

    public void OnClickBtnDefinedArea()
    {
        tileEffectType = TileEffectType.DefinedArea;
    }
    public void OnClickBtnWall()
    {
        tileEffectType = TileEffectType.Wall;
    }

    public Texture ConvertSpriteToTexture(Sprite sprite)
    {
        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                int x = Mathf.FloorToInt(sprite.textureRect.x);
                int y = Mathf.FloorToInt(sprite.textureRect.y);
                int width = Mathf.FloorToInt(sprite.textureRect.width);
                int height = Mathf.FloorToInt(sprite.textureRect.height);

                Texture2D newText = new Texture2D(width, height);
                Color[] newColors = sprite.texture.GetPixels(x, y, width, height);

                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch
        {
            return sprite.texture;
        }
    }
}
