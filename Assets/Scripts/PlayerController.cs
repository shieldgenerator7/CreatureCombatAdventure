using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    public Player player;
    public GameObject cardDisplayerPrefab;
    private List<CardDisplayer> cardDisplayerList = new List<CardDisplayer>();

    PlayerControls playerControls;
    PlayerControls.PlayerActions playerActions;

    public List<CardHolder> holders;

    public List<CardHolder> enemyRanks;
    public List<CardHolder> allyRanks;

    public TMP_Text txtPowerEnemy;
    public TMP_Text txtPowerAlly;

    [SerializeField]
    private CardDisplayer heldCardDisplayer = null;
    [SerializeField]
    private Vector2 holdOffset = Vector2.zero;

    [SerializeField]
    private CardHolder hoverHolder = null;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerControls = new PlayerControls();
        playerActions = playerControls.Player;
        playerActions.AddCallbacks(this);

        createCards();

        for (int i = 0; i < cardDisplayerList.Count; i++)
        {
            CardDisplayer card = cardDisplayerList[i];
            card.cardLayer = i+1;
            card.updateDisplay();
            //TODO: make function to layout cards in hand / other holder
            card.transform.position = new Vector2(-8 + i * 8, -10);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void createCards()
    {
        cardDisplayerList.ForEach(card =>
        {
            Destroy(card.gameObject);
        });
        cardDisplayerList.Clear();
        player.cardList.ForEach(card =>
        {
            GameObject go = Instantiate(cardDisplayerPrefab);
            CardDisplayer cd = go.GetComponent<CardDisplayer>();
            cd.card = card;
            cd.updateDisplay();
            cd.transform.position = new Vector2(0, -5);
            cd.owner = this;

            cardDisplayerList.Add(cd);
        });
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
                if (beats(ally.rps, enemy.rps))
                {
                    _ep -= _ap;
                }
                if (beats(enemy.rps, ally.rps))
                {
                    _ap -= _ep;
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
            foreach (RaycastHit2D rch2d in rch2ds)
            {
                if (rch2d.collider)
                {
                    CardDisplayer cd = rch2d.collider.gameObject.GetComponent<CardDisplayer>();
                    if (cd)
                    {
                        if (cd.owner == this) { 
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
                        CardHolder ch = rch2d.collider.gameObject.GetComponent<CardHolder>();
                        if (ch)
                        {
                            if (!ch.card && ch.owner == this)
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
                if (hoverHolder != null)
                {
                    hoverHolder.acceptMouseHover(false);
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
                    CardHolder ch = rch2d.collider.gameObject.GetComponent<CardHolder>();
                    if (ch)
                    {
                        if ((!ch.card || ch.card == heldCardDisplayer.card) && ch.owner == this)
                        {
                            hoverHolder = ch;
                            hoverHolder.acceptMouseHover(true);
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
}
