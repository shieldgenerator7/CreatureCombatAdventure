using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
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
    public int holdIndex = -1;

    public List<CardHolder> enemyRanks;
    public List<CardHolder> allyRanks;

    public TMP_Text txtPowerEnemy;
    public TMP_Text txtPowerAlly;

    [SerializeField]
    private CardDisplayer heldCardDisplayer = null;
    [SerializeField]
    private Vector2 holdOffset = Vector2.zero;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerControls = new PlayerControls();
        playerActions = playerControls.Player;
        playerActions.AddCallbacks(this);

        card = cardList[cardIndex];
        for(int i =0; i < cardList.Count; i++)
        {
            CardDisplayer card = cardList[i];
            card.cardLayer = i;
            card.updateDisplay();
        }

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

    void updateDisplay()
    {
        int enemyPower = 0;
        int allyPower = 0;

        for (int i = 0; i < 5; i++)
        {
            CreatureCard ally = allyRanks[i].card;
            CreatureCard enemy = enemyRanks[i].card;
            if (!enemy)
            {
                allyPower += ally?.power ?? 0;
            }
            if (!ally)
            {
                enemyPower += enemy?.power ?? 0;
            }
            if (ally && enemy)
            {
                int _ap = ally.power;
                int _ep = enemy.power;
                if(beats(ally.rps, enemy.rps))
                {
                    _ep -= _ap;
                }
                if (beats(enemy.rps, ally.rps))
                {
                    _ap-= _ep;
                }
                allyPower += _ap;
                enemyPower += _ep;
            }
        }

        txtPowerEnemy.text = $"{enemyPower}";
        txtPowerAlly.text = $"{allyPower}";

    }

    //TODO: move to more suitable spot (utility?)
    public bool beats(RockPaperScissors rps1, RockPaperScissors rps2)
    {
        return rps1 == RockPaperScissors.ROCK && rps2 == RockPaperScissors.SCISSORS
            || rps1 == RockPaperScissors.PAPER && rps2 == RockPaperScissors.ROCK
            || rps1 == RockPaperScissors.SCISSORS && rps2 == RockPaperScissors.PAPER;
    }



    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D[] rch2ds = Physics2D.RaycastAll(mousepos, Vector2.zero, 0);
            foreach(RaycastHit2D rch2d in rch2ds) { 
            if (rch2d.collider)
            {
                    CardDisplayer cd = rch2d.collider.gameObject.GetComponent<CardDisplayer>();
                    if (cd)
                    {
                heldCardDisplayer = rch2d.collider.gameObject.GetComponent<CardDisplayer>();
                holdOffset = (Vector2)heldCardDisplayer.transform.position - mousepos;
                    break;
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
                        CardHolder ch = rch2d.collider.gameObject.GetComponent<CardHolder>();
                        if (ch)
                        {
                            if (!ch.card)
                            {
                            ch.acceptDrop(heldCardDisplayer);
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
            }
            heldCardDisplayer = null;

            updateDisplay();
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
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (card.holder)
            {
                card.holder.card = null;
            }
            Vector2 v = context.ReadValue<Vector2>();
            do {
                holdIndex += (int)v.x;
                if (holdIndex >= holders.Count)
                {
                    holdIndex = 0;
                }
                if (holdIndex < 0)
                {
                    holdIndex = holders.Count - 1;
                }
            }
            while (holders[holdIndex].card != null);
            holders[holdIndex].acceptDrop(card);
            updateDisplay();
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
