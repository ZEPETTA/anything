using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentItem_L : MonoBehaviour
{
    public ChangeTextSize_L changeTextSize;

    RectTransform rt;

    float preferredHeight;

    // Start is called before the first frame update
    void Start()
    {
        changeTextSize = GetComponent<ChangeTextSize_L>();
        changeTextSize.onChangedSize = OnChangedTextSize;
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string s)
    {
        changeTextSize.text = s;
    }

    void OnChangedTextSize()
    {
        if(preferredHeight != changeTextSize.preferredHeight)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, changeTextSize.preferredHeight);

            preferredHeight = changeTextSize.preferredHeight;
        }
    }
}
