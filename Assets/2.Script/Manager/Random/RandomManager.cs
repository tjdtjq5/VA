using System;
using System.Collections.Generic;
using Shared.Enums;

public class RandomManager
{
    private readonly int _weight = 10000;
    public List<T> RandomDraw<T>(List<T> cards, Dictionary<Grade, float> percentage, int drawCount, bool isDuplicated) where T : IRandom
    {
        if (percentage.Count == 0 || cards.Count == 0 || drawCount <= 0)
        {
            throw new ArgumentException($"RandomDraw can't be empty [Card Count : {cards.Count}] [Percentage Count {percentage.Count}] [Draw Count : {drawCount}]");
        }
        
        List<T> tempCards = new List<T>();
        tempCards.AddRange(cards);
        
        
        List<Grade> removeGrades = new List<Grade>();
        foreach (var per in percentage)
        {
            if (tempCards.Find(c => c.Grade.Equals(per.Key)) == null)
                removeGrades.Add(per.Key);
        }
        
        for (int i = 0; i < removeGrades.Count; i++)
            percentage.Remove(removeGrades[i]);

        Dictionary<Grade, int> gradeWeight = new Dictionary<Grade, int>();
        
        float totalPercent = 0;
        foreach (var per in percentage)
            totalPercent += per.Value;

        int beforeWeight = 0;
        int totalWeight = 0;
        foreach (var per in percentage)
        {
            int weight = (int)(per.Value / totalPercent * _weight);
            totalWeight += weight;
            weight += beforeWeight;
            beforeWeight = weight;
            gradeWeight.Add(per.Key, weight);
        }
        
        List<T> randoms = new List<T>();

        for (int i = 0; i < drawCount; i++)
        {
            int rw = (int)UnityHelper.Random_H(0, totalWeight);

            Grade grade = Grade.D;
            
            foreach (var gw in gradeWeight)
            {
                if (gw.Value > rw)
                {
                    grade = gw.Key;
                    break;
                }
            }

            if (tempCards.Count <= 0)
                break;
            
            List<T> gradeCards = tempCards.FindAll(c => c.Grade.Equals(grade));
            if (gradeCards.Count <= 0)
            {
                drawCount++;
                continue;
            }
            T randomCard = gradeCards[(int)UnityHelper.Random_H(0, gradeCards.Count)];
            randoms.Add(randomCard);

            if (!isDuplicated)
                tempCards.Remove(randomCard);
        }
        
        return randoms;
    }
}
