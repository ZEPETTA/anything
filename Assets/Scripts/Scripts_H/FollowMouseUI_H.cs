using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouseUI_H : MonoBehaviour
{
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = Input.mousePosition+ new Vector3(80,35,0);
    }
}
