using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class MilestoneSystem
{
    [Serializable]
    private class DateFormatCustom
    {
        [Range(0, 24)][SerializeField] private int hours;
        [Range(0, 60)][SerializeField] private int minutes;
        [Range(0, 60)][SerializeField] private int seconds;
        [Range(0, 100)][SerializeField] private int milliseconds;

        public int Hours { get => hours; }
        public int Minutes { get => minutes; }
        public int Seconds { get => seconds; }
        public int Milliseconds { get => milliseconds; }
    }

    [Serializable]
    private class RankSystem
    {
        [Serializable]
        internal struct RankTable
        {
            [SerializeField] private int key;
            [SerializeField] private char value;
            [SerializeField] private Color color;

            public int Key { get => key; }
            public char Value { get => value; }
            public Color Color { get => color; }
        }


        [Serializable]
        internal class RankTableMap
        {
            [SerializeField] private RankTable[] set;

            internal char GetValue(int key) => set.First(rt => rt.Key == key).Value;

            internal Color GetColor(int key) => set.First(rt => rt.Key == key).Color;

            internal int GetKeyMaxValue()
            {
                int max = 0;

                foreach(RankTable table in set)
                {
                    if(table.Key > max)
                        max = table.Key;
                }
                return max;
            }

            internal char GetValueMaxValue() 
            {
                int max = 0;

                foreach (RankTable table in set)
                {
                    int value = table.Value;
                    if(value > max)
                        max = value;
                }

                return (char)max;
            }
        }
        
        [SerializeField] private RankTableMap table;

        internal RankTableMap Table { get => table; set => table = value; } 
    }

    [SerializeField] private int maxCogCount;
    [SerializeField] private DateFormatCustom maxTime;
    [SerializeField] private int maxEnemiesToDefeatCount;

    [Header("KEY = Number of objectives to complete, Value = the rank to assign at the end")]
    [SerializeField] private RankSystem Ranks;

    public float Timer { get; set; }
    public int CogCount { get; set; }
    public int DefeatedEnemiesCount { get; set; }

    private int MilestoneCounter { get; set; }

    public string CalculateTime()
    {
        Debug.Log($"Time one: {Timer}");

        float timeInH = Timer / 3600f;
        float H = Mathf.FloorToInt(timeInH);
        float M = (timeInH - H) * 60f;
        float S = (M - Mathf.FloorToInt(M)) * 60f;
        float MS = Mathf.Round((S - Mathf.FloorToInt(S)) * 100f);

        M = Mathf.FloorToInt(M);
        S = Mathf.FloorToInt(S);
        return $"{H}:{M}:{S}.{MS}";
    }

    private string CalculateRank()
    {
        MilestoneCounter = 0;

        float H = maxTime.Hours;
        float M = maxTime.Minutes / 60f;
        float S = maxTime.Seconds / 3600f;
        float MS = maxTime.Milliseconds / 100f;

        float t = (H + M + S) * 3600;

        t += MS;

        if(Timer <= t)
            MilestoneCounter++;

        if (CogCount >= maxCogCount)
            MilestoneCounter++;

        if (DefeatedEnemiesCount >= maxEnemiesToDefeatCount)
            MilestoneCounter++;

        return $"{Ranks.Table.GetValue(MilestoneCounter)}";
    }

    public void EndResults()
    {
        EventManager.SetResultScreen?.Invoke
            (CalculateTime(), CogCount, maxCogCount, DefeatedEnemiesCount, maxEnemiesToDefeatCount, CalculateRank(), $"{Ranks.Table.GetValueMaxValue()}", Ranks.Table.GetColor(MilestoneCounter), Ranks.Table.GetColor(Ranks.Table.GetKeyMaxValue()));
    }
}

