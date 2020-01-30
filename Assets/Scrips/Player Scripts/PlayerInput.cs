using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PlayerController playerController;

    private int horizontal = 0, vertical = 0;

    public enum Axis
    {
        Horizonral,
        Vertical
    }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = 0;
        vertical = 0;

        getKeyboardInput();
        setMovement();
    }
    void getKeyboardInput()
    {

            horizontal = getAxisRaw(Axis.Horizonral);
            vertical = getAxisRaw(Axis.Vertical);

            if(horizontal != 0)
            {
                vertical = 0;
            }
        
    }

    void setMovement()
    {
        if (vertical != 0)
        {
            playerController.setInputDirection((vertical == 1) ? PlayerDirection.UP : PlayerDirection.DOWN);
        }
        else if (horizontal != 0)
        {
            playerController.setInputDirection((horizontal == 1) ? PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

    int getAxisRaw(Axis axis)
    {
        if (axis == Axis.Horizonral)
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);

            if (left)
            {
                return -1;
            }
            if (right)
            {
                return 1;
            }
            return 0;
        }
        else if (axis == Axis.Vertical)
        {
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);

            if (up)
            {
                return 1;
            }
            if (down) {
                return -1;
            }
            return 0;
        }
        return 0;
    }
}
