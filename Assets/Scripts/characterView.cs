using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterView : MonoBehaviour
{
    public Vector3 newPosition;
    Animator animator;

    Vector3 currentVelocity;
    float smoothTime = 0.5f;
    float MaxSpeed = 4f;

    

    // Start is called before the first frame update
    void Start()
    {
        newPosition = this.transform.position;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, newPosition, ref currentVelocity, smoothTime, MaxSpeed);
        bool setled = ((Mathf.Abs(this.transform.position.x - newPosition.x) < 0.02f) && (Mathf.Abs(this.transform.position.y - newPosition.y) < 0.02f));
        animator.SetBool("run", !setled);
    }
}
