using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackablePlayer : MonoBehaviour
{
    public CharacterBaseController m_CharacterBaseController;
    
    public UnityEvent OnMirrorVision = new UnityEvent();
    public UnityEvent OffMirrorVision = new UnityEvent();

	int timer = 0;
	public int Duration = 3;
    
    private void Awake()
    {
        m_CharacterBaseController = GetComponent<CharacterBaseController>();
    }

    public void CheckIfAttacked()
    {
        if (m_CharacterBaseController.hasBeenHit)
        {
            if (m_CharacterBaseController.mirror)
            {
                OnMirrorVision.Invoke();
                m_CharacterBaseController.mirror = false;
                timer = Duration;
                DungeonBaseController.instance.OnEndTurn.AddListener(Wearoff);
            }
            m_CharacterBaseController.hasBeenHit = false;
            m_CharacterBaseController.OnHit.Invoke(m_CharacterBaseController.damageSource);
		}
    }

	void Wearoff()
	{
		if (timer <= 0)
		{
			OffMirrorVision.Invoke();
			DungeonBaseController.instance.OnEndTurn.RemoveListener(Wearoff);
		}
		timer--;
	}
}
