using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class ArenaDisplayer : MonoBehaviour
{
    public Color colorEmpty = new Color(1, 1, 1, 0.5f);
    public Color colorNormal = Color.white;
    public Color colorLess = Color.red;
    public Color colorGreater = Color.green;

    public List<CardHolderDisplayer> allyHolders;
    public CardHolderDisplayer allyHand;
    public List<CardHolderDisplayer> enemyHolders;
    public CardHolderDisplayer enemyHand;

    public List<TMP_Text> txtAllyList;
    public List<TMP_Text> txtEnemyList;
    public List<SpriteRenderer> srRPSAllyList;
    public List<SpriteRenderer> srRPSEnemyList;

    public Match match;
    public Arena arena;

    private List<UIAnimation> scoreAnimations = new List<UIAnimation>();


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
                //TODO: handle changing of RPS better
                if (true || pap != ap)
                {
                    Color color = getColor(ap, apr);

                    NumberUIAnimation nuia =
                    NumberUIAnimation.adjustTo(txtAllyList[index], pap, ap, color, ssd);
                    scoreAnimations.Add(nuia);

                    SpriteUIAnimation suia =
                    SpriteUIAnimation.ChangeSprite(srRPSAllyList[index], ssd.GetSymbolSprite(lane.AllyRPS), color);
                    scoreAnimations.Add(suia);
                }
                //TODO: handle changing of RPS better
                if (true || pep != ep)
                {
                    Color color = getColor(ep, epr);

                    NumberUIAnimation nuia =
                    NumberUIAnimation.adjustTo(txtEnemyList[index], pep, ep, color, ssd);
                    scoreAnimations.Add(nuia);

                    SpriteUIAnimation suia =
                    SpriteUIAnimation.ChangeSprite(srRPSEnemyList[index], ssd.GetSymbolSprite(lane.EnemyRPS), color);
                    scoreAnimations.Add(suia);
                }
            };
            updateLaneNow(i);
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

    private void updateLaneNow(int laneIndex)
    {
        ArenaLane lane = arena.lanes[laneIndex];
        SymbolSetData ssd = GameManager.SymbolSetData;

        Color allyColor = getColor(lane.allyPower, lane.AllyPowerRaw);
        TMP_Text txtNumberAlly = txtAllyList[laneIndex];
        txtNumberAlly.text = ssd.GetSymbolString(lane.allyPower);
        txtNumberAlly.color = allyColor;
        SpriteRenderer srAlly = srRPSAllyList[laneIndex];
        srAlly.sprite = ssd.GetSymbolSprite(lane.AllyRPS);
        srAlly.color = allyColor;

        Color enemyColor = getColor(lane.enemyPower, lane.EnemyPowerRaw);
        TMP_Text txtNumberEnemy = txtEnemyList[laneIndex];
        txtNumberEnemy.text = ssd.GetSymbolString(lane.enemyPower);
        txtNumberEnemy.color = getColor(lane.enemyPower, lane.EnemyPowerRaw);
        txtNumberEnemy.color = enemyColor;
        SpriteRenderer srEnemy = srRPSEnemyList[laneIndex];
        srEnemy.sprite = ssd.GetSymbolSprite(lane.EnemyRPS);
        srEnemy.color = enemyColor;
    }

    private Color getColor(int power, int rawPower)
    {
        if (power == rawPower)
        {
            if (power == 0)
            {
                return colorEmpty;
            }
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
