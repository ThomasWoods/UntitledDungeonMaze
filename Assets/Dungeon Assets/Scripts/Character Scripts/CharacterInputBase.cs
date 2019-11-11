﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputBase : MonoBehaviour
{
    public CharacterBaseController m_CharacterBaseController;

	virtual protected void Awake()
	{
		m_CharacterBaseController = GetComponent<CharacterBaseController>();
	}

	public virtual CharacterAction CheckMovementInput()
    {
		CharacterAction backupDir = CharacterAction.Wait;
        Debug.LogWarning(gameObject.name + " doesn't override the base movement input method!");
        return backupDir;
    }

    public virtual bool CheckInteractionInput()
    {
        Debug.LogWarning(gameObject.name + " doesn't override the base interaction input method!");
        return false;
    }
}
