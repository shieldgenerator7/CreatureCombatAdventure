using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    public List<CardDisplayer> cardList;
    private CardDisplayer card;
    private int cardIndex = 0;

    PlayerControls playerControls;
    PlayerControls.PlayerActions playerActions;

    public List<CardHolder> holders;
    public int holdIndex = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerControls = new PlayerControls();
        playerActions = playerControls.Player;
        playerActions.AddCallbacks(this);

        card = cardList[cardIndex];
        holders[holdIndex].acceptDrop(card);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        playerActions.Enable();
    }
    private void OnDisable()
    {
        playerActions.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            cardIndex++;
            if (cardIndex < cardList.Count)
            {
                card = cardList[cardIndex];
                holdIndex++;
                if (holdIndex >= holders.Count)
                {
                    holdIndex = 0;
                }
                if (holdIndex < 0)
                {
                    holdIndex = holders.Count - 1;
                }
                holders[holdIndex].acceptDrop(card);
            }
            else
            {

            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 v = context.ReadValue<Vector2>();
            holdIndex += (int)v.x;
            if (holdIndex >= holders.Count)
            {
                holdIndex = 0;
            }
            if (holdIndex < 0)
            {
                holdIndex = holders.Count - 1;
            }
            holders[holdIndex].acceptDrop(card);
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
