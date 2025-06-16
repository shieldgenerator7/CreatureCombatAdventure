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


    private CardDisplayer heldCardDisplayer = null;
    private Vector2 holdOffset = Vector2.zero;

    private CardHolder<Card> hoverHolder = null;



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
                        if (controller.canPickupCard(cd)) { 
                        heldCardDisplayer = rch2d.collider.gameObject.GetComponent<CardDisplayer>();
                        holdOffset = (Vector2)heldCardDisplayer.transform.position - mousepos;
                        break;
                        }
                    }
                }
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (heldCardDisplayer != null)
            {
                Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
                bool dropped = false;
                foreach (RaycastHit2D rch2d in rch2ds)
                {
                    if (rch2d.collider)
                    {
                        CardHolder<Card> ch = rch2d.collider.gameObject.GetComponent<CardHolderDisplayer>()?.cardHolder;
                        if (ch != null)
                        {
                            if (controller.canPlaceCardAt(heldCardDisplayer, ch))
                            {
                                controller.placeCard(heldCardDisplayer, ch);
                                dropped = true;
                                break;
                            }
                        }
                    }
                }
                if (!dropped)
                {
                    heldCardDisplayer.holder?.acceptDrop(heldCardDisplayer);
                }
                if (hoverHolder != null)
                {
                    //TODO: put this back in
                    //hoverHolder.acceptMouseHover(false);
                }
            }
            heldCardDisplayer = null;
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
        if (heldCardDisplayer != null)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            heldCardDisplayer.transform.position = mousepos + holdOffset;


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
                    CardHolder<Card> ch = rch2d.collider.gameObject.GetComponent<CardHolderDisplayer>()?.cardHolder;
                    if (ch != null)
                    {
                        if (controller.canPlaceCardAt(heldCardDisplayer,ch))
                        {
                            hoverHolder = ch;
                            //TODO: put back in
                            //hoverHolder.acceptMouseHover(true);
                            break;
                        }
                    }
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
