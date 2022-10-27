using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 닉네임의 앞방향 = 카메라 앞방향으로 셋팅
        transform.position = Camera.main.transform.forward;
    }
}
