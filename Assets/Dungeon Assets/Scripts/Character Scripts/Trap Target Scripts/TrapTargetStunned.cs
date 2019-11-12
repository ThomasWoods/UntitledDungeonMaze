using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTargetStunned : TrapTargetBase
{
	public int StunDuration = 1;
    public CharacterBaseController m_CharacterController;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterBaseController>();
    }

    public override void OnTrapped(int trapStr)
    {
		m_CharacterController.Stun(StunDuration);
        Debug.Log(gameObject.name + " stepped on a trap and got stunned!");
    }
}
