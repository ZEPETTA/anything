using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeTextSize_L : Text
{
    //크기가 변경되었을때 호출되는 함수를 가지는 변수
    public Action onChangedSize;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (onChangedSize != null)
        {
            onChangedSize();
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputVertical();
        if (onChangedSize != null)
        {
            onChangedSize();
        }
    }
}
