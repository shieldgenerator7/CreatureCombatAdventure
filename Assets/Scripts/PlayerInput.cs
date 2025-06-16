using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : WranglerInput, PlayerControls.IPlayerActions
{
    PlayerControls playerControls;
    PlayerControls.PlayerActions playerActions;


    private Card heldCard = null;
    private Vector2 holdOffset = Vector2.zero;

    private CardHolder hoverHolder = null;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerControls = new PlayerControls();
        playerActions = playerControls.Player;
        playerActions.AddCallbacks(this);

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



    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
            foreach (RaycastHit2D rch2d in rch2ds)
            {
                if (rch2d.collider)
                {
                    CardDisplayer cd = rch2d.collider.gameObject.GetComponent<CardDisplayer>();
                    if (cd)
                    {
                        Card card = cd.card;
                        if (controller.canPickupCard(card)) {
                        heldCard = card;
                        holdOffset = (Vector2)cd.transform.position - mousepos;
                        break;
                        }
                    }
                }
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (heldCard != null)
            {
                Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
                bool dropped = false;
                foreach (RaycastHit2D rch2d in rch2ds)
                {
                    if (rch2d.collider)
                    {
                        CardHolder ch = rch2d.collider.gameObject.GetComponent<CardHolderDisplayer>()?.CardHolder;
                        if (ch != null)
                        {
                            if (controller.canPlaceCardAt(heldCard, ch))
                            {
                                controller.placeCard(heldCard, ch);
                                dropped = true;
                                break;
                            }
                        }
                    }
                }
                if (!dropped)
                {
                    heldCard.holder?.acceptDrop(heldCard);
                }
                if (hoverHolder != null)
                {
                    //TODO: put back in
                    //hoverHolder.acceptMouseHover(false);
                }
            }
            heldCard = null;
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
        if (heldCard != null)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //TODO: put back in
            //heldCard.transform.position = mousepos + holdOffset;


            //hover holder pos
            if (hoverHolder != null)
            {
                //TODO: put back in
                //hoverHolder.acceptMouseHover(false);
            }
            RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
            foreach (RaycastHit2D rch2d in rch2ds)
            {
                if (rch2d.collider)
                {
                    //TODO: put back in
                    //CardHolderDisplayer ch = rch2d.collider.gameObject.GetComponent<CardHolderDisplayer>();
                    //if (ch != null)
                    //{
                    //    if (controller.canPlaceCardAt(heldCard,ch.cardHolder))
                    //    {
                    //        hoverHolder = ch;
                    //        hoverHolder.acceptMouseHover(true);
                    //        break;
                    //    }
                    //}
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
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

    public void OnExit(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
