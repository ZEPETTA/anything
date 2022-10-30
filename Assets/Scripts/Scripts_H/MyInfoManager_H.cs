using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoManager_H : MonoBehaviour
{
    public GameObject characterEditorImgae;
    public GameObject myFavoriteImgae;
    public Sprite favoriteClick;
    public List<FavoriteInfoButton_H> favoriteInfoButtons;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < myFavoriteImgae.transform.childCount; i++)
        {
            favoriteInfoButtons.Add(myFavoriteImgae.transform.GetChild(0).GetComponent<FavoriteInfoButton_H>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EditAvatar()
    {
        characterEditorImgae.SetActive(true);
    }
    public void XButton()
    {
        characterEditorImgae.SetActive(false);
    }
    public void SaveButton()
    {
        //나중에 제대로 동작하게 수정
        characterEditorImgae.SetActive(false);
    }
    public void MyFavoriteDown()
    {
        myFavoriteImgae.SetActive(true);
    }
    public void MyFavoriteUP()
    {
        myFavoriteImgae.SetActive(false);
    }

}
