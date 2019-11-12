﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatableEnemy : DefeatableBase
{
    public CharacterBaseController m_CharacterBaseController;

    private bool hasBeenDefeated = false;

    private void Awake()
    {
        m_CharacterBaseController = GetComponent<CharacterBaseController>();
    }

    public override void Defeated()
    {
        RemoveCharacter();
    }

    private void RemoveCharacter()
    {
        DungeonBaseController.instance.allCharacters.Remove(m_CharacterBaseController);
        DungeonBaseController.instance.enemies.Remove(m_CharacterBaseController);

        Destroy(gameObject);
    }
}
