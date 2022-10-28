using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSIntroducer_H : MonoBehaviour
{
    public float txtSpeed = 1f;
    public Text txt;
    public SpeechBubble_H bubble;
    public string[] whatSay;
    bool istalking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && istalking == false)
        {
            UserInfo user = collision.GetComponent<CharacterMove_H>().user;
            bubble.speechTime = whatSay.Length * txtSpeed + txtSpeed;
            bubble.gameObject.SetActive(true);
            txt.text = "æ»≥Á«œººø‰, " + user.name + "¥‘";
            StartCoroutine(Say());

        }
    }
    IEnumerator Say()
    {
        for(int i =0; i< whatSay.Length; i++)
        {
            yield return new WaitForSeconds(txtSpeed);
            txt.text = whatSay[i];
            if(i == whatSay.Length - 1)
            {
                istalking = true;
            }
        }
    }
}
