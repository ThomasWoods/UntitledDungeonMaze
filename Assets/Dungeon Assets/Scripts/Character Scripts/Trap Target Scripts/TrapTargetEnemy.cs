using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTargetEnemy : TrapTargetBase
{
    public CharacterBaseController m_CharacterBaseController;

    private void Awake()
    {
        m_CharacterBaseController = GetComponent<CharacterBaseController>();
    }

    public override void OnTrapped(int trapStr, StaticEnums.StatusEffect effect)
    {
        m_CharacterBaseController.TakeDamage(effect, trapStr);
    }
}
