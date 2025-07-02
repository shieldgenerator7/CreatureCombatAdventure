using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class ArenaDisplayer : MonoBehaviour
{
    public Color colorNormal = Color.white;
    public Color colorLess = Color.red;
    public Color colorGreater = Color.green;

    public List<CardHolderDisplayer> allyHolders;
    public CardHolderDisplayer allyHand;
    public List<CardHolderDisplayer> enemyHolders;
    public CardHolderDisplayer enemyHand;

    public List<TMP_Text> txtAllyList;
    public List<TMP_Text> txtEnemyList;

    public Match match;
    public Arena arena;

    private List<UIAnimation> scoreAnimations = new List<UIAnimation>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void init(Wrangler ally, Wrangler enemy)
    {

        //TODO: follow DRY

        //holders ally
        allyHand.init(ally.handHolder);
        for (int i = 0; i < arena.allyHolders.Count; i++)
        {
            CardHolder ch = arena.allyHolders[i];
            allyHolders[i].init(ch);
        }

        //holders enemy
        enemyHand.init(enemy.handHolder);
        for (int i = 0; i < arena.enemyHolders.Count; i++)
        {
            CardHolder ch = arena.enemyHolders[i];
            enemyHolders[i].init(ch);
        }

        //lane displayers
        SymbolSetData ssd = GameManager.SymbolSetData;
        for (int i = 0; i < arena.lanes.Count; i++)
        {
            int index = i;
            ArenaLane lane = arena.lanes[i];
            lane.OnPowerChanged += (pap, ap, apr, pep, ep, epr) =>
            {
                if (pap != ap)
                {
                    NumberUIAnimation nuia =
                    NumberUIAnimation.adjustTo(txtAllyList[index], pap, ap, ssd,
                        (number) => $"<color=#{ColorUtility.ToHtmlStringRGB(getColor(ap, apr))}>{ssd.GetSymbolString(lane.AllyRPS)} {ssd.GetSymbolString(number)}"
                        );
                    scoreAnimations.Add(nuia);
                }
                if (pep != ep)
                {
                    NumberUIAnimation nuia =
                    NumberUIAnimation.adjustTo(txtEnemyList[index], pep, ep, ssd,
                        (number) => $"<color=#{ColorUtility.ToHtmlStringRGB(getColor(ep, epr))}>{ssd.GetSymbolString(lane.EnemyRPS)} {ssd.GetSymbolString(number)}"
                        );
                    scoreAnimations.Add(nuia);
                }
            };
        }

        //listen for match score update
        match.OnScoresChanged += (pap, ap, pep, ep) =>
        {
            //remove the anims from the anim queue
            scoreAnimations.ForEach(anim => UIAnimationQueue.Instance.removeAnimation(anim));
            //Make lane score changes all animate together
            SimultaneousUIAnimation.AnimateTogether(scoreAnimations);
            //
            scoreAnimations.Clear();
        };

    }

    private Color getColor(int power, int rawPower)
    {
        if (power == rawPower)
        {
            return colorNormal;
        }
        if (power < rawPower)
        {
            return colorLess;
        }
        if (power > rawPower)
        {
            return colorGreater;
        }
        return Color.white;
    }
}
