using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using SFB;

public class MapEditor_L : MonoBehaviour
{
    public static MapEditor_L instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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
        SpawnPoint
    }
    public enum ObjectType
    {
        Text,
        Image,
    }

    public ToolType toolType;
    public PlacementType placementType;
    public TileEffectType tileEffectType;
    public ObjectType objectType;

    public float floorTileZ;
    public float definedAreaZ;
    public float wallTileZ;
    
    public GameObject floorTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject definedAreaPrefab;
    public GameObject portalPrefab;
    public GameObject spawnPrefab;
    public GameObject objectPrefab;
    public GameObject objectButtonPrefab;

    public Texture currClickedTileTexture;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 5f;
    public float scrollSpeed = 0.5f;
    public float arrowDragSpeed = 10f;
    public int width, height;
    public Transform grid;
    public Transform cursorSquare;
    public Transform tileParent;
    public Transform objectParent;
    public Transform definedAreaParent;
    public Transform portalParent;
    public Transform objectButtonParent;

    public Dropdown definedAreaDropdown;
    public InputField inputFieldDefinedAreaName;

    public InputField inputFieldPortalOtherMapName;
    public InputField inputFieldPortalDefinedAreaName;

    public GameObject canvas;
    public MeshRenderer bg;

    public GameObject panelPortalToDefinedArea;
    public GameObject panelPortalToOtherMap;
    public GameObject checkImageDefinedArea;
    public GameObject checkImageOtherMap;
    public GameObject checkImageKey;
    public GameObject checkImageInstant;
    public GameObject checkImageKey_DefinedArea;
    public GameObject checkImageInstant_DefinedArea;
    public GameObject scrollViewMyObject;
    public GameObject scrollViewMyUpperObject;

    public List<Vector3> spawnPointPosList; //로드 시, 리스트에 위치 넣어두고 랜덤 위치에 캐릭터 옮김
    //UI 클릭 시에는 맵 생성되지 않게
    int layerMaskUI;
    PortalInfo portalInfo;
    Vector3 gridStartPos;

    int tileLayerMask;
    Vector2 pastTilePos;
    Vector2 pastDefinedAreaPos;
    Vector2 pastWallPos;
    Vector2 pastPortalPos;
    Vector2 pastSpawnPointPos;
    Vector2 mouseClickPos;
    public int mapCount;
    public GameObject quadBG;
    /*    public LineRenderer gridLineRenderer_X;
        public LineRenderer gridLineRenderer_Y;*/

    public void BigToggle()
    {
        quadBG.transform.localScale = new Vector3(60, 30, 1);
    }
    public void MideumToggle()
    {
        quadBG.transform.localScale = new Vector3(120, 60, 1);
    }
    public void SmallToggle()
    {
        quadBG.transform.localScale = new Vector3(180, 60, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        #region 배경화면 가져오기
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/" + SpaceInfo.spaceName + ".txt";
        string jsonData = File.ReadAllText(bgpath);
        SpaceInfo spaceInfo = JsonUtility.FromJson<SpaceInfo>(jsonData);
        MapInfo info = spaceInfo.mapList[0];
        //MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        //quadBG.transform.localScale = new Vector3(info.mapWidth / 20, info.mapHeight / 20, 1);
        Texture2D bgTexture = new Texture2D(info.mapWidth, info.mapHeight);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        #endregion
        #region 타일 가져오기
        string tileParent = "TileParent";
        for (int i = 0; i < info.tileList.Count; i++)
        {
            Vector3 tilePos = info.tileList[i].position;
            Texture tileSprite = Resources.Load<Texture>("Resources_L/" + info.tileList[i].imageName);
            GameObject realTile = Instantiate(floorTilePrefab);
            realTile.transform.position = tilePos;
            realTile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tileSprite);
            realTile.transform.parent = GameObject.Find(tileParent).transform;
        }
        #endregion
        #region 포탈 가져오기
        Transform portalParent = GameObject.Find("PortalParent").transform;
        if (portalParent)
        {
            for (int i = 0; i < info.portalList.Count; i++)
            {
                //Vector3 tilePos = info.portalList[i].position;
                GameObject myPortal = Instantiate(portalPrefab);
                Portal2D_L portal2D = myPortal.GetComponent<Portal2D_L>();
                portal2D.portalInfo = new PortalInfo();
                myPortal.transform.parent = portalParent;
                myPortal.transform.localPosition = info.portalList[i].position;

                portal2D.portalInfo.position = myPortal.transform.localPosition;
                portal2D.portalInfo.placeType = info.portalList[i].placeType;
                portal2D.portalInfo.moveType = info.portalList[i].moveType;
                portal2D.portalInfo.definedAreaName = info.portalList[i].definedAreaName;
                portal2D.portalInfo.mapName = info.portalList[i].mapName;
            }
        }
        #endregion
        #region 지정구역 가져오기
        GameObject areaParent = GameObject.Find("DefinedAreaParent");
        Dictionary<string, int> nameDic = new Dictionary<string, int>();
        for (int i = 0; i < info.areaName.Count; i++)
        {
            GameObject child = new GameObject(info.areaName[i]);
            child.transform.parent = areaParent.transform;
            nameDic.Add(info.areaName[i], i);
        }
        for (int i = 0; i < info.definedAreaList.Count; i++)
        {
            GameObject area = Instantiate(definedAreaPrefab);
            area.transform.position = info.definedAreaList[i].positon;
            area.GetComponentInChildren<Text>().text = info.definedAreaList[i].areaName;
            area.transform.parent = areaParent.transform.GetChild(nameDic[info.definedAreaList[i].areaName]);
        }

        #endregion
        #region 벽 가져오기
        for (int i = 0; i < info.wallList.Count; i++)
        {
            Vector3 wallPos = info.wallList[i].positon;
            GameObject wall = Instantiate(wallTilePrefab);
            wall.transform.position = wallPos;
            //wall.GetComponent<MeshRenderer>().material.mainTexture = invisibleTexture;
            wall.transform.parent = GameObject.Find("WallParent").transform;
        }
        #endregion
        #region 스폰 지점 가져오기
        Transform spawnPointParent = GameObject.Find("SpawnPointParent").transform;
        if (spawnPointParent)
        {
            spawnPointPosList = new List<Vector3>();

            for (int i = 0; i < info.spawnPointInfoList.Count; i++)
            {
                GameObject spawnPoint = Instantiate(spawnPrefab);
                spawnPoint.transform.parent = spawnPointParent;
                spawnPoint.transform.localPosition = info.spawnPointInfoList[i].position;
                spawnPointPosList.Add(spawnPoint.transform.localPosition);

            }
        }
        #endregion


        //================
        #region 포탈값 초기화
        portalInfo = new PortalInfo();
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        portalInfo.moveType = PortalInfo.MoveType.Key;
        #endregion

        toolType = ToolType.Stamp;
        placementType = PlacementType.Floor;
        pastTilePos = Vector2.zero;
        pastDefinedAreaPos = Vector2.zero;
        pastPortalPos = Vector2.zero;
        pastSpawnPointPos = Vector2.zero;
        tileLayerMask = 1 << LayerMask.NameToLayer("Tile");
        gridStartPos = grid.transform.position;
        gridStartPos.x -= grid.transform.localScale.x * 0.5f;
        gridStartPos.y -= grid.transform.localScale.y * 0.5f;

        layerMaskUI = (1 << LayerMask.NameToLayer("UI"));

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


    void UiOnOff()
    {
        if (objSkill.value == 0)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
        }
        else if (objSkill.value == 1)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
            skillUI[0].SetActive(true);
        }
        else if (objSkill.value == 2)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
            skillUI[0].SetActive(true);
        }
        else if (objSkill.value == 2)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
            skillUI[1].SetActive(true);
        }
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
        objSkill.onValueChanged.AddListener(delegate { UiOnOff(); });
       

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
                if (placementType == PlacementType.Floor){
                    Stamp();
                }
                else if (placementType == PlacementType.TileEffect) {
                    TileEffect();
                }
                else if(placementType == PlacementType.Object)
                {
                    if(objectType == ObjectType.Text)
                    {
                        StampObjectText();
                    }
                    else if (objectType == ObjectType.Image)
                    {
                        StampObjectImage();
                    }
                }
                else if(placementType == PlacementType.UpperObject)
                {

                }
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
    public void SelectText()
    {
        objectType = ObjectType.Text;
    }
    #region 오브젝트 이미지 가져오기
    public string WriteResult(string[] paths)
    {
        string result = "";
        if (paths.Length == 0)
        {
            return "";
        }
        foreach (string p in paths)
        {
            result += p + "\n";
        }
        return result;
    }
    string path;
    GameObject objButton;
    public void SelectImage()
    {
        objectType = ObjectType.Image;
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        if(path.Length > 0)
        {
            objButton = Instantiate(objectButtonPrefab);
            objButton.transform.parent = objectButtonParent;
            StartCoroutine(GetTexture());
        }
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D texture2D = (Texture2D)myTexture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f,0.5f));
            objButton.GetComponent<Image>().sprite = sprite;
            
        }
    }
    #endregion
    public Texture2D objImage;
    public InputField objName;
    public Dropdown objSkill;
    public GameObject[] skillUI;
    public Toggle objInteraction;
    public InputField objURL;
    public Texture2D objChangeImage;
    void StampObjectImage()
    {
        if(objImage != null)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {

                    if (hitInfo.transform.tag == "Object")
                    {
                        return;
                    }
                    if (hitInfo.transform.tag == "BackGround")
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastTilePos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        pastTilePos = new Vector2(x, y);
                        GameObject obj = Instantiate(objectPrefab);
                        obj.transform.position = new Vector3(x, y, 0);
                        //obj.transform.localScale = new Vector3(objImage.width /1920, objImage.height/1080, 0);
                        obj.transform.parent = objectParent;
                        obj.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (Texture)objImage);
                        ObjectInfo objinfo = obj.transform.GetChild(0).GetComponent<ObjectInfo_H>().objectInfo;
                        objinfo.Position = obj.transform.position;
                        objinfo.image = objImage.EncodeToPNG();
                        objinfo.upperObj = false;
                        objinfo.objType = ObjectInfo.ObjectType.Image;
                        objinfo.objName = objName.text;
                        if (objInteraction.isOn)
                        {
                            objinfo.interactionType = ObjectInfo.InteractionType.pressF;
                        }
                        else
                        {
                            objinfo.interactionType = ObjectInfo.InteractionType.touch;
                        }
                        if (objSkill.value == 0)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.nomalObj;
                        }
                        else if (objSkill.value == 1)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.urlObj;
                            objinfo.urlSkill = objURL.text;
                        }
                        else if (objSkill.value == 2)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.talkingObj;
                            objinfo.talkingSkill = objURL.text;
                        }
                        else if (objSkill.value == 3)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.changeObj;
                        }
                    }
                }
            }
        }
    }
    
    public void ButtonTextObj()
    {
        objTextInputField.gameObject.SetActive(true);
        scrollViewMyObject.SetActive(false);
    }
    public InputField objTextInputField;
    public GameObject textObj;
    void StampObjectText()
    {
        if (objTextInputField.text.Length > 0)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {

                    if (hitInfo.transform.tag == "Object")
                    {
                        return;
                    }
                    if (hitInfo.transform.tag == "BackGround")
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastTilePos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        pastTilePos = new Vector2(x, y);
                        GameObject obj = Instantiate(textObj);
                        obj.transform.position = new Vector3(x, y, 0);
                        obj.GetComponent<TextMesh>().text = objTextInputField.text;
                    }
                }
            }
        }
    }
    void TileEffect()
    {
        switch (tileEffectType)
        {
            case TileEffectType.Portal:
                if (portalInfo.placeType == PortalInfo.PlaceType.DefinedArea)
                {
                    if (inputFieldPortalDefinedAreaName.text.Length < 1)
                    {
                        break;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;
                        /*                        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMaskUI))
                                                {
                                                    break;
                                                }*/

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            Debug.Log(hitInfo.transform.tag);

                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastPortalPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }

                            GameObject tile = Instantiate(portalPrefab);
                            tile.transform.SetParent(portalParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            Portal2D_L portal2D = tile.GetComponent<Portal2D_L>();
                            portal2D.portalInfo = new PortalInfo();
                            portal2D.portalInfo.definedAreaName = inputFieldPortalDefinedAreaName.text;
                            portal2D.portalInfo.moveType = portalInfo.moveType;
                            portal2D.portalInfo.placeType = portalInfo.placeType;
                            portal2D.portalInfo.position = tile.transform.localPosition;

                            pastPortalPos.x = x;
                            pastPortalPos.y = y;
                        }
                    }
                }
                else if(portalInfo.placeType == PortalInfo.PlaceType.OtherMap)
                {
                    if (inputFieldPortalOtherMapName.text.Length < 1)
                    {
                        break;
                    }
                    if (Input.GetMouseButton(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;
/*                        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMaskUI))
                        {
                            break;
                        }*/

                            if (Physics.Raycast(ray, out hitInfo))
                        {
                            Debug.Log(hitInfo.transform.tag);

                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastPortalPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }

                            GameObject tile = Instantiate(portalPrefab);
                            tile.transform.SetParent(portalParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            Portal2D_L portal2D = tile.GetComponent<Portal2D_L>();
                            portal2D.portalInfo = new PortalInfo();
                            portal2D.portalInfo.mapName = inputFieldPortalOtherMapName.text;
                            portal2D.portalInfo.moveType = portalInfo.moveType;
                            portal2D.portalInfo.placeType = portalInfo.placeType;
                            portal2D.portalInfo.position = tile.transform.localPosition;

                            pastPortalPos.x = x;
                            pastPortalPos.y = y;
                        }
                    }
                }
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
                        littleParent2 = new GameObject(inputFieldDefinedAreaName.text).transform;
                        littleParent2.SetParent(definedAreaParent);
                        littleParent2.localPosition = Vector3.zero;
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
            case TileEffectType.SpawnPoint:
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastSpawnPointPos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        /*                    if(Physics.OverlapBox(new Vector3(x,y,floorTileZ),new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,tileLayerMask).Length>0){
                                                print("Already Exists");
                                                return;
                                                //이미 타일이 있는 경우
                                            }*/
                        GameObject tile = Instantiate(spawnPrefab);
                        tile.transform.SetParent(GameObject.Find("SpawnPointParent").transform);
                        tile.transform.localPosition = new Vector3(x, y, wallTileZ);
                        pastSpawnPointPos.x = x;
                        pastSpawnPointPos.y = y;
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

    #region 버튼 OnClick 함수

    public void OnClickBtnFloor()
    {
        placementType = PlacementType.Floor;
        TurnOnUi(1);
        print("On Click Btn Floor");
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
    public void OnClickBtnObject()
    {
        placementType = PlacementType.Object;
        TurnOnUi(2);
    }
    public void OnClickBtnUpperObject()
    {
        placementType = PlacementType.UpperObject;
        TurnOnUi(3);
    }

    public void OnClickBtnMyObject()
    {
        bool b = scrollViewMyObject.activeSelf ? false : true;
        scrollViewMyObject.SetActive(b);
        objTextInputField.gameObject.SetActive(false);
    }

    public void OnClickBtnMyUpperObject()
    {
        bool b = scrollViewMyUpperObject.activeSelf ? false : true;
        scrollViewMyUpperObject.SetActive(b);
    }

    public void OnClickBtnPortalToDefinedArea()
    {
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        panelPortalToDefinedArea.SetActive(true);
        panelPortalToOtherMap.SetActive(false);

        checkImageDefinedArea.SetActive(true);
        checkImageOtherMap.SetActive(false);
    }

    public void OnClickBtnPortalToOtherMap()
    {
        portalInfo.placeType = PortalInfo.PlaceType.OtherMap;
        panelPortalToDefinedArea.SetActive(false);
        panelPortalToOtherMap.SetActive(true);

        checkImageDefinedArea.SetActive(false);
        checkImageOtherMap.SetActive(true);
    }

    public void OnClickBtnPortalMoveKey()
    {
        portalInfo.moveType = PortalInfo.MoveType.Key;
        checkImageKey.SetActive(true);
        checkImageInstant.SetActive(false);
    }

    public void OnClickBtnPortalMoveInstant()
    {
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        checkImageKey.SetActive(false);
        checkImageInstant.SetActive(true);
    }

    public void OnClickBtnPortalDefinedMoveKey()
    {
        portalInfo.moveType = PortalInfo.MoveType.Key;
        checkImageKey_DefinedArea.SetActive(true);
        checkImageInstant_DefinedArea.SetActive(false);
    }

    public void OnClickBtnPortalDefinedMoveInstant()
    {
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        checkImageKey_DefinedArea.SetActive(false);
        checkImageInstant_DefinedArea.SetActive(true);
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
    public void OnClickBtnSpawnPoint()
    {
        tileEffectType = TileEffectType.SpawnPoint;
    }

    #endregion

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
