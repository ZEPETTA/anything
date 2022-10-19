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
        Dropper
    }

    public enum PlacementType
    {
        Floor,
        Object,
        UpperObject
    }

    public ToolType toolType;
    public PlacementType placementType;
    public float floorTileZ;
    public GameObject floorTilePrefab;
    public Texture currClickedTileTexture;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 5f;
    public float scrollSpeed = 0.5f;
    int tileLayerMask;
    Vector2 pastTilePos;
    // Start is called before the first frame update
    void Start()
    {
        toolType = ToolType.Stamp;
        placementType = PlacementType.Floor;
        pastTilePos = Vector2.zero;
        tileLayerMask = 1 << LayerMask.NameToLayer("Tile");
    }

    // Update is called once per frame
    void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += scrollWheel * Time.deltaTime * scrollSpeed;

        if (Camera.main.orthographicSize > maxOrthographicSize)
            Camera.main.orthographicSize = maxOrthographicSize;
        if (Camera.main.orthographicSize < minOrthographicSize)
            Camera.main.orthographicSize = minOrthographicSize;

        switch (toolType)
        {
            case ToolType.Stamp:
                Stamp();
                break;
            case ToolType.Eraser:
                Erase();
                break;
            case ToolType.Dropper:
                Dropper();
                break;
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
                    tile.transform.position = new Vector3(x, y, floorTileZ);
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

    public void OnClickBtnFloorExample()
    {
        currClickedTileTexture= ConvertSpriteToTexture(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite);
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
