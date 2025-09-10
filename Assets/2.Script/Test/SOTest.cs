using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

public class SOTest : MonoBehaviour
{
    [Button]
    public void Excel(SkillDeckType deckType)
    {
        List<Skill> skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + $"/{deckType.ToString()}").ToList();
        string text = "";

        skills = skills.OrderBy(x => x.Grade).ToList();

        for (int i = 0; i < skills.Count; i++)
        {
            Skill skill = skills[i];
            string cleanedDescription = Regex.Replace(skill.Description, "<.*?>", "");
            cleanedDescription = cleanedDescription.Replace("\n", " ");
            text += $"{skill.CodeName}\t{skill.Grade}\t{skill.DisplayName}\t{cleanedDescription}\n";
        }
        
        UnityHelper.Log_H(text);
        GUIUtility.systemCopyBuffer = text;
    }
}
