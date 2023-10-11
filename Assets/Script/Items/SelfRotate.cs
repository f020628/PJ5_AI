using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate: MonoBehaviour
{
    

    public Animator animator;
    
    private void Start()
    {
        animator.Play("SelfRotate");
    }
  

 

}
