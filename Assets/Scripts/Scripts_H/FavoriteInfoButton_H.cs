using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FavoriteInfoButton_H : MonoBehaviour
{
    public bool IsMyFavorite = false;
    // Start is called before the first frame update
    public void OnClickFavoriteButton()
    {
        IsMyFavorite = !IsMyFavorite;
        if(IsMyFavorite == true)
        {
            Color color = new Color();
            color = GetComponent<Image>().color;
            color.a = 0;
            GetComponent<Image>().color = color;
        }
        else
        {
            Color color = new Color();
            color = GetComponent<Image>().color;
            color.a = 255;
            GetComponent<Image>().color = color;
        }
    }
}
