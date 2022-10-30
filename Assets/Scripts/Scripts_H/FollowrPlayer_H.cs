using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowrPlayer_H : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name.Contains("PanelNoticeBoard"))
        {
            gameObject.transform.position = player.transform.position - new Vector3(-3,0,5);
        }
    }
}
