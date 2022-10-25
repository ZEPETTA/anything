using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjButton_H : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CanDrawObject);
    }
    void CanDrawObject()
    {
        MapEditor_L.instance.objectType = MapEditor_L.ObjectType.Image;
        MapEditor_L.instance.objImage = GetComponent<Image>().sprite.texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
