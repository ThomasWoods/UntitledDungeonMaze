using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackablePlayer : MonoBehaviour
{
    public CharacterBaseController m_CharacterBaseController;
    
    public UnityEvent OnMirrorVision = new UnityEvent();
    
    private void Awake()
    {
        m_CharacterBaseController = GetComponent<CharacterBaseController>();
    }

    public void CheckIfAttacked()
    {
        if (m_CharacterBaseController.hasBeenHit)
        {
            if (m_CharacterBaseController.damageSource.Contains("Medusa")) OnMirrorVision.Invoke();
            m_CharacterBaseController.hasBeenHit = false;
            m_CharacterBaseController.OnHit.Invoke();
        }
    }

}
