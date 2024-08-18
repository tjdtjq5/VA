using UnityEngine;

public class FloatingTextManager
{
    string prefabPath = "Prefab/InGame/FloatingText";
    Color basicDamageColor = Color.white;
    Color critialDamageColor = Color.yellow;

    public void DamageSpawn(Entity targetEntity, BBNumber damage, bool isCritial)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);

        floatingText.Text.text = $"{damage}";
        floatingText.Text.color = isCritial ? critialDamageColor : basicDamageColor;

        floatingText.Play(GetPosByTargetEntity(targetEntity));
    }
    public void TextSpawn(Entity targetEntity, string text, Color color)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);

        floatingText.Text.text = $"{text}";
        floatingText.Text.color = color;

        floatingText.Play(GetPosByTargetEntity(targetEntity));
    }

    Vector3 GetPosByTargetEntity(Entity target)
    {
        BoxCollider collider = target.Collider;
        Vector3 pos = target.transform.position;
        pos.y += collider.size.y;
        return pos;
    }
}
