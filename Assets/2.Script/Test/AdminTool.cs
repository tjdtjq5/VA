using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shared.DTOs.Player;
using Shared.CSharp;
using Sirenix.OdinInspector;
using UnityEngine;

public class AdminTool : MonoBehaviour
{
    [Button]
    public void GetLatestPlayerData()
    {
        // 현재 어셈블리에서 PlayerDto를 상속받은 모든 타입 찾기
        var derivedTypes = GetPlayerDtoTypes();

        
        Managers.Web.SendPostRequest<PlayerGetsLatestResponse>("player/gets/latest", new PlayerGetsLatestRequest()
        {
            TypeNames = derivedTypes.Select(type => type.Name).ToList()
            
        }, (_result) =>
        {
            Managers.PlayerData.DbUpdate(_result.Datas);
        });
    }
    [Button]
    public void LogPlayerDataRadis(PlayerDto type)
    {
        List<string> typeNames = new List<string>();
        typeNames.Add(type.GetType().Name);

        PlayerGetsRequest req = new PlayerGetsRequest();
        req.TypeNames = typeNames;

        Managers.Web.SendPostRequest<PlayerGetsResponse>("player/gets/redis", req, (_result) =>
        {
            string json = _result.Datas.SerializeObject_H();
            UnityHelper.Log_H(json);
        });
    }
    [Button]
    public void LogPlayerData(PlayerDto type)
    {
        // 1. List<type> 제네릭 타입 생성
        Type listType = typeof(List<>).MakeGenericType(type.GetType());

        // 2. GetPlayerData<T>() 메서드 찾기 (제네릭 + 파라미터 없음)
        var method = Managers.PlayerData.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(m =>
                m.Name == "GetPlayerData" &&
                m.IsGenericMethodDefinition &&
                m.GetParameters().Length == 0);

        if (method == null)
        {
            UnityHelper.Log_H("GetPlayerData<T>() 메서드를 찾을 수 없습니다!");
            return;
        }

        // 3. 제네릭 메서드 구체화 (T = List<type>)
        var genericMethod = method.MakeGenericMethod(listType);

        // 4. 실행
        var playerDatas = genericMethod.Invoke(Managers.PlayerData, null);

        // 5. 직렬화 및 로그
        string json = CSharpHelper.SerializeObject(playerDatas);
        UnityHelper.Log_H(json);
    }
    [Button]
    public void AddItem(Item item, int count)
    {
        Managers.Web.SendPostRequest<PlayerItemAddResponse>("player/item/add", new PlayerItemAddRequest()
        {
            ItemCode = item.CodeName,
            Count = count
            
        }, (_result) =>
        {
            Managers.PlayerData.DbUpdate(_result.Datas);
        });
    }

    [Button]
    public void UseItem(Item item, int count)
    {
        Managers.Web.SendPostRequest<PlayerItemUseResponse>("player/item/use", new PlayerItemUseRequest()
        {
            ItemCode = item.CodeName,
            Count = count
            
        }, (_result) =>
        {
            Managers.PlayerData.DbUpdate(_result.Datas);
        });
    }

    private List<Type> GetPlayerDtoTypes()
    {
        var baseType = typeof(PlayerDto);
        var derivedTypes = Assembly.GetAssembly(baseType)
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType))
            .ToList();

        return derivedTypes;
    }
}
