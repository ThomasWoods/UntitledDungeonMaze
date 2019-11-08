using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardController : CharacterInputBase
{
    public KeyCode primaryForward = KeyCode.W;
    public KeyCode primaryLeft = KeyCode.A;
    public KeyCode primaryRight = KeyCode.D;
    public KeyCode primarySpace = KeyCode.Space;
    public KeyCode primaryInteract = KeyCode.E;

    public KeyCode secondaryForward = KeyCode.UpArrow;
    public KeyCode secondaryLeft = KeyCode.LeftArrow;
    public KeyCode secondaryRight = KeyCode.RightArrow;
    public KeyCode secondaryInteract = KeyCode.Return;

    public override int[] CheckMovementInput()
    {
        int[] inputDir = new int[2];

        if (Input.GetKeyDown(primarySpace))
        {
            inputDir[0] = 2;
        }
        else if (Input.GetKey(primaryForward) || Input.GetKey(secondaryForward))
        {
            inputDir[0] = 1;
        }
        else
        {
            if (Input.GetKey(primaryLeft) || Input.GetKey(secondaryLeft))
            {
                inputDir[1] -= 1;
            }

            if (Input.GetKey(primaryRight) || Input.GetKey(secondaryRight))
            {
                inputDir[1] += 1;
            }
        }

        return inputDir;
    }

    public override bool CheckInteractionInput()
    {
        if (Input.GetKeyDown(primaryInteract) || Input.GetKeyDown(secondaryInteract))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
