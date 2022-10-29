using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardManager_L : MonoBehaviour
{
    public GameObject noticeBoardWriteView;

    public InputField inputPostit;

    public List<Toggle> toggleList;
    public GameObject postitPrefab;
    public List<Transform> scrollViewContentList;

    public enum CategoryType
    {
        Chat,
        QnA,
        Team,
        PartTime
    }

    public CategoryType categoryType;

    private void Start()
    {
        categoryType = CategoryType.Chat;

        for(int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].onValueChanged.AddListener(delegate { OnToggleSwitch(); });
        }
    }

    public void OnClickSubmitPostit()
    {
        GameObject postit = Instantiate(postitPrefab, scrollViewContentList[(int)categoryType]);

        postit.GetComponentInChildren<Text>().text = inputPostit.text;
        inputPostit.text = "";
        noticeBoardWriteView.SetActive(false);
    }

    public void OnToggleSwitch()
    {
        for(int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                categoryType = (CategoryType)i;
                //int를 enum형으로 변환
                //toggleList 순서와 categoryType 순서가 일치해야 함
            }
        }
    }
}
