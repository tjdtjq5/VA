using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBar : MonoBehaviour
{
    private List<BuffBarCard> _cards = new List<BuffBarCard>();
    
    private readonly string _cardPrefabPath = "Prefab/Character/BuffBarCard";

    public void Push(Buff buff)
    {
        if (!buff.BuffIcon)
            return;

        if (_cards.Find(c => c.Buff.CodeName.Equals(buff.CodeName)))
            return;

        if (buff.Count < 1)
            return;

        BuffBarCard card = FindCard(buff);

        if (!card)
        {
            card = Instantiate();
            _cards.Add(card);
        }
        
        card.gameObject.SetActive(true);
        card.transform.localScale = Vector3.one;
        card.Initialize(buff);
        card.OnCardDestroy -= OnCardDestroy;
        card.OnCardDestroy += OnCardDestroy;
    }

    void OnCardDestroy(BuffBarCard card) => _cards.Remove(card);

    public void Destroy(Buff buff)
    {
        BuffBarCard card = FindCard(buff);
        
        if (card)
            card.Destroy();
    }
    public void Destroy(string codeName)
    {
        BuffBarCard card = FindCard(codeName);
        
        if (card)
            card.Destroy();
    }

    BuffBarCard FindCard(Buff buff) => _cards.Find(c => c.Buff.CodeName.Equals(buff.CodeName));
    BuffBarCard FindCard(string codeName) => _cards.Find(c => c.Buff.CodeName.Equals(codeName));
    BuffBarCard Instantiate() => Managers.Resources.Instantiate<BuffBarCard>(_cardPrefabPath, this.transform);
}
