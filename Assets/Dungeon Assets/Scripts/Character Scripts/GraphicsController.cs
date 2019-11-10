using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsController : MonoBehaviour
{
    public Animator m_Animator;
    
    public void StartWalking()
    {
        m_Animator.SetBool("Move", true);
    }

    public void StopWalking()
    {
        m_Animator.SetBool("Move", false);
    }

}
