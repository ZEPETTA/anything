using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove_H : MonoBehaviour
{
    public bool myCharacter = true;
    public float speed;
    public float runSpeed;
    float applyRunSpeed;
    bool applyRunFlag = false;
    private Vector3 vector;
    public int walkCount;
    int currentWalkCount;
    bool canMove = true;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!myCharacter)
        {
            return;
        }
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StopAllCoroutines();
                StartCoroutine(MoveCoroutine());
            }
        }

    }
    IEnumerator MoveCoroutine()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            applyRunSpeed = runSpeed;
            applyRunFlag = true;
        }
        else
        {
            applyRunSpeed = 0;
            applyRunFlag = false;
        }
        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);
        int count = 0;
        animator.SetFloat("DirX", vector.x);
        animator.SetFloat("DirY", vector.y);
        animator.SetBool("Walking", true);
        while (currentWalkCount < walkCount)
        {
            count++;
            transform.position += vector.normalized * (speed + applyRunSpeed) * 0.1f;
            if (applyRunFlag)
            {
                currentWalkCount++;
            }
            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }
        animator.SetBool("Walking", false);
        currentWalkCount = 0;
        canMove = true;
    }
}
