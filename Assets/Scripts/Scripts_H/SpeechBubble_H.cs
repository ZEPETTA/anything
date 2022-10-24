using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble_H : MonoBehaviour
{
    public float speechTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.parent.parent.position + new Vector3(0, 2f, 0));
    }
    private void OnEnable()
    {
        StartCoroutine(TurnOff());
    }
    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(speechTime);
        gameObject.SetActive(false);
    }
}
