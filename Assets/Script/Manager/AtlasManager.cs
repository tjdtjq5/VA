using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager
{
    public SpriteAtlas Grade { get; private set; }  
    public SpriteAtlas GradeBg { get; private set; }
    public SpriteAtlas Item { get; private set; }
    public SpriteAtlas Job { get; private set; }
    public SpriteAtlas Tribe { get; private set; }

    public void Initialize()
    {
        Grade = Managers.Resources.Load<SpriteAtlas>("Atlas/Grade");
        GradeBg = Managers.Resources.Load<SpriteAtlas>("Atlas/GradeBg");
        Item = Managers.Resources.Load<SpriteAtlas>("Atlas/Item");
        Job = Managers.Resources.Load<SpriteAtlas>("Atlas/Job");
        Tribe = Managers.Resources.Load<SpriteAtlas>("Atlas/Tribe");
    }

    #region Format
    string gradeFormat = "Grade_{0}";
    #endregion

    public Sprite GetGrade(Grade grade)
    {
        return Grade.GetSprite(CSharpHelper.Format_H(gradeFormat, grade.ToString()));
    }
    public Sprite GetGradeBg(Grade grade)
    {
        return GradeBg.GetSprite(CSharpHelper.Format_H(gradeFormat, grade.ToString()));
    }
    public Sprite GetItem(ItemTableCodeDefine item)
    {
        return Item.GetSprite(item.ToString());
    }
    public Sprite GetJob(CharacterJob job)
    {
        return Job.GetSprite(job.ToString());
    }
    public Sprite GetTribe(Tribe tribe)
    {
        return Tribe.GetSprite(tribe.ToString());
    }
}
