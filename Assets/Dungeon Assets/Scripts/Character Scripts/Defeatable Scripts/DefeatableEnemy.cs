﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatableEnemy : DefeatableBase
{
    public CharacterBaseController m_CharacterBaseController;

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
        Debug.Log(gameObject.name + " was defeated!");

        DungeonBaseController.instance.m_TurnManager.RemoveFromQueue(m_CharacterBaseController);
        DungeonBaseController.instance.allCharacters.Remove(m_CharacterBaseController);
        DungeonBaseController.instance.enemies.Remove(m_CharacterBaseController);
        //DungeonBaseController.instance.m_TurnManager.RemakeQueue();

        Destroy(gameObject);
    }
}
