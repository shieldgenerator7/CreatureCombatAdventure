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
    public List<TMP_Text> txtAllyCapacityList;
    public List<TMP_Text> txtEnemyCapacityList;

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

                    int cardcount = lane.allyHolder.CardCount;
                    NumberUIAnimation nuia2 =
                    NumberUIAnimation.adjustTo(
                        txtAllyCapacityList[index],
                        cardcount,
                        cardcount,
                        getColor(cardcount), 
                        ssd, 
                        (count) =>$"{count}/{arena.data.lanes[index].limit}"
                        );
                    scoreAnimations.Add(nuia2);

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

                    int cardCount = lane.enemyHolder.CardCount;
                    NumberUIAnimation nuia2 =
                    NumberUIAnimation.adjustTo(
                        txtAllyCapacityList[index],
                        cardCount,
                        cardCount,
                        getColor(cardCount),
                        ssd,
                        (count) => $"{count}/{arena.data.lanes[index].limit}"
                        );
                    scoreAnimations.Add(nuia2);

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

        //secrets
        match.encounterData.secrets.ForEach(secret =>
        {
            GameObject go = Instantiate(secret.prefab, Bin.Transform);
            SecretDisplayer sd = go.GetComponent<SecretDisplayer>();
            sd.init(secret);
        });

    }

    private void updateLaneNow(int laneIndex)
    {
        ArenaLane lane = arena.lanes[laneIndex];
        SymbolSetData ssd = GameManager.SymbolSetData;
        int laneLimit = arena.data.lanes[laneIndex].limit;
        bool isLaneValid = laneIndex < arena.data.lanes.Count && laneLimit> 0;

        Color allyColor = getColor(lane.allyPower, lane.AllyPowerRaw);
        TMP_Text txtNumberAlly = txtAllyList[laneIndex];
        txtNumberAlly.text = ssd.GetSymbolString(lane.allyPower);
        txtNumberAlly.color = allyColor;
        txtNumberAlly.gameObject.SetActive(isLaneValid);
        SpriteRenderer srAlly = srRPSAllyList[laneIndex];
        srAlly.sprite = ssd.GetSymbolSprite(lane.AllyRPS);
        srAlly.color = allyColor;
        srAlly.gameObject.SetActive(isLaneValid);
        TMP_Text txtCapacityAlly = txtAllyCapacityList[laneIndex];
        txtCapacityAlly.text = $"{lane.allyHolder.CardCount}/{laneLimit}";
        txtCapacityAlly.gameObject.SetActive(isLaneValid);
        txtCapacityAlly.color = getColor(lane.allyHolder.CardCount);

        Color enemyColor = getColor(lane.enemyPower, lane.EnemyPowerRaw);
        TMP_Text txtNumberEnemy = txtEnemyList[laneIndex];
        txtNumberEnemy.text = ssd.GetSymbolString(lane.enemyPower);
        txtNumberEnemy.color = getColor(lane.enemyPower, lane.EnemyPowerRaw);
        txtNumberEnemy.color = enemyColor;
        txtNumberEnemy.gameObject.SetActive(isLaneValid);
        SpriteRenderer srEnemy = srRPSEnemyList[laneIndex];
        srEnemy.sprite = ssd.GetSymbolSprite(lane.EnemyRPS);
        srEnemy.color = enemyColor;
        srEnemy.gameObject.SetActive(isLaneValid);
        TMP_Text txtCapacityEnemy = txtEnemyCapacityList[laneIndex];
        txtCapacityEnemy.text = $"{lane.enemyHolder.CardCount}/{laneLimit}";
        txtCapacityEnemy.gameObject.SetActive(isLaneValid);
        txtCapacityEnemy.color = getColor(lane.enemyHolder.CardCount);
    }

    private Color getColor(int power, int rawPower = -1)
    {
        if (rawPower < 0)
        {
            rawPower = power;
        }
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
