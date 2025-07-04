using NUnit.Framework;
using System;
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



    void Awake()
    {
        playerControls = new PlayerControls();
        playerActions = playerControls.Player;
        playerActions.AddCallbacks(this);

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
            CardDisplayer cd;
            (cd, holdOffset) = getMousedOverCard();
            if (cd != null)
            {
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
                            if (controller.Wrangler.canPlaceCardAt(heldCard.card, ch))
                            {
                                controller.Wrangler.placeCard(heldCard.card, ch);
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
                        if (controller.Wrangler.canPlaceCardAt(heldCard.card, ch.CardHolder))
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
            CardDisplayer cd;
            (cd, holdOffset) = getMousedOverCard();
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
        if (context.phase == InputActionPhase.Started)
        {
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

    private (CardDisplayer, Vector2) getMousedOverCard()
    {
        return getMousedOverObject<CardDisplayer>(
            cd => controller.Wrangler.canPickupCard(cd.card), 
            cd => cd.cardLayer
            );
    }

    private (T, Vector2) getMousedOverObject<T>(Func<T,bool> filterFunc = null, Func<T,int> sortValueFunc = null) where T : MonoBehaviour
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
        List<T> cards = new List<T>();
        foreach (RaycastHit2D rch2d in rch2ds)
        {
            if (rch2d.collider)
            {
                T cd = rch2d.collider.gameObject.GetComponent<T>();
                if (cd)
                {
                    if (filterFunc == null || filterFunc(cd))
                    {
                        cards.Add(cd);
                    }
                }
            }
        }
        if (cards.Count > 0)
        {
            T cd;
            if (sortValueFunc != null)
            {
            int max = cards.Max(sortValueFunc);
            cd = cards.Find(c => sortValueFunc(c) == max);
            }
            else
            {
                cd = cards.First();
            }
            Vector2 offset = (Vector2)cd.transform.position - mousepos;
            return (cd, offset);
        }
        return (null, Vector2.zero);
    }
}
