using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : WranglerInput, PlayerControls.IPlayerActions
{
    public int pickupLayer = 1000;

    PlayerControls playerControls;
    PlayerControls.PlayerActions playerActions;


    private CardDisplayer heldCard = null;
    private Vector2 holdOffset = Vector2.zero;

    private CardHolderDisplayer hoverHolder = null;

    private CardDisplayer mousedOverCard = null;



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
                CardDisplayer cd = getMousedOverCard();
            if (cd != null) {
                heldCard = cd;
                cd.cardLayer = pickupLayer;
                cd.updateDisplay();
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
                            if (controller.canPlaceCardAt(heldCard.card, ch))
                            {
                                controller.placeCard(heldCard.card, ch);
                                dropped = true;
                                break;
                            }
                        }
                    }
                }
                if (!dropped)
                {
                    heldCard.card.holder?.acceptDrop(heldCard.card);
                }
                if (hoverHolder != null)
                {
                    hoverHolder.acceptMouseHover(false);
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
            heldCard.transform.position = mousepos + holdOffset;


            //hover holder pos
            if (hoverHolder != null)
            {
                hoverHolder.acceptMouseHover(false);
            }
            RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
            foreach (RaycastHit2D rch2d in rch2ds)
            {
                if (rch2d.collider)
                {
                    CardHolderDisplayer ch = rch2d.collider.gameObject.GetComponent<CardHolderDisplayer>();
                    if (ch != null)
                    {
                        if (controller.canPlaceCardAt(heldCard.card, ch.CardHolder))
                        {
                            hoverHolder = ch;
                            hoverHolder.acceptMouseHover(true);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            CardDisplayer cd = getMousedOverCard();
            if (cd)
            { 
                if (cd != mousedOverCard && mousedOverCard)
                {
                    mousedOverCard.MousedOver = false;
                }
                mousedOverCard = cd;
                mousedOverCard.MousedOver = true;
            }
            else
            {
                if (mousedOverCard != null)
                {
                    mousedOverCard.MousedOver = false;
                }
                mousedOverCard = null;
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

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) { 
        FindAnyObjectByType<GameManager>().Reset();
        }
    }
    public void OnNextMatch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
        FindAnyObjectByType<GameManager>().NextMatch();
        }
    }
    public void OnPrevMatch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
        FindAnyObjectByType<GameManager>().PrevMatch();
        }
    }

    private CardDisplayer getMousedOverCard()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
        List<CardDisplayer> cards = new List<CardDisplayer>();
        foreach (RaycastHit2D rch2d in rch2ds)
        {
            if (rch2d.collider)
            {
                CardDisplayer cd = rch2d.collider.gameObject.GetComponent<CardDisplayer>();
                if (cd)
                {
                    Card card = cd.card;
                    if (controller.canPickupCard(card))
                    {
                        cards.Add(cd);
                    }
                }
            }
        }
        if (cards.Count > 0)
        {
            int max = cards.Max(cd => cd.cardLayer);
            CardDisplayer cd = cards.Find(c => c.cardLayer == max);
            holdOffset = (Vector2)cd.transform.position - mousepos;
            return cd;
        }
        return null;
    }
}
