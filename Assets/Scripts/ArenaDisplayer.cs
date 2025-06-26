using System.Collections.Generic;
using System.Drawing;
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

    public Arena arena;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateDisplay()
    {
        for (int i = 0; i < arena.lanes.Count; i++)
        {
            ArenaLane lane = arena.lanes[i];
            string allyColor = $"<color=#{ColorUtility.ToHtmlStringRGB(getColor(lane.allyPower,lane.AllyPowerRaw))}>";
            string enemyColor = $"<color=#{ColorUtility.ToHtmlStringRGB(getColor(lane.enemyPower, lane.EnemyPowerRaw))}>";
            txtAllyList[i].text = $"{allyColor}{GameManager.SymbolSetData.GetSymbolString(lane.AllyRPS)}  {GameManager.SymbolSetData.GetSymbolString(lane.allyPower)}</color>";
            txtEnemyList[i].text = $"{enemyColor}{GameManager.SymbolSetData.GetSymbolString(lane.EnemyRPS)}  {GameManager.SymbolSetData.GetSymbolString(lane.enemyPower)}</color>";
        }
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
