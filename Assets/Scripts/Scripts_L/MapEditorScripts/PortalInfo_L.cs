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
    //Key = FŰ �̵�
    //Instant = ��� �̵�

    public MoveType moveType;
    public PlaceType placeType;

    public string definedAreaName;
    //�̸����� mapInfo���� �ش� �̸��� definedArea�� �迭(�Ǵ� ����Ʈ)�� ã�� �� �־�� ��
    //ã�� area�� Portal_L���� �޾Ƽ�, �� �� �ϳ��� �̵��� �� �־�� ��
}
