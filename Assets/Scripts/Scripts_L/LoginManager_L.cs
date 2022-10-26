using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager_L : MonoBehaviour
{
    public InputField inputFieldId;
    public InputField inputFieldPW;
    public Button btnLogin;
    public GameObject signUP;
    // Start is called before the first frame update
    void Start()
    {
        inputFieldId.onValueChanged.AddListener(OnIdValueChanged);
        inputFieldPW.onValueChanged.AddListener(OnPWValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnIdValueChanged(string s)
    {
        btnLogin.interactable = s.Length > 0 && inputFieldPW.text.Length > 0;
    }

    public void OnPWValueChanged(string s)
    {
        btnLogin.interactable = s.Length > 0 && inputFieldId.text.Length > 0;
    }

    public void OnClickBtnLogin()
    {
        SceneManager.LoadScene("LobbyScene_L");
    }

    public void OnClickBtnSignup()
    {
       signUP.SetActive(true);
    }
}
