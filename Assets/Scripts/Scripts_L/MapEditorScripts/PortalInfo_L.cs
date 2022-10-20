using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInfo_L
{
    public enum MoveType
    {
        Key,
        Instant
    }

    public enum PlaceType
    {
        OtherMap,
        DefinedArea,
        OtherSpace
    }
    //Key = F키 이동
    //Instant = 즉시 이동

    public MoveType moveType;
    public PlaceType placeType;

    public string definedAreaName;
    //이름으로 mapInfo에서 해당 이름의 definedArea의 배열(또는 리스트)을 찾을 수 있어야 함
    //찾은 area를 Portal_L에서 받아서, 그 중 하나로 이동할 수 있어야 함
}
