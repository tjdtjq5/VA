using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.DTOs.Player;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;
using Spine.Unity;

public class UIRobbyGrowGrow : UIRobby
{
    [SerializeField] private GoodsPrice _atkPrice;
    [SerializeField] private GoodsPrice _hpPrice;
    [SerializeField] private SkeletonGraphic _atkCharacter;
    [SerializeField] private SkeletonGraphic _hpCharacter;

    private int _pendingUpgradeCount_Atk = 0;
    private int _pendingUpgradeCount_Hp = 0;
    private float _upgradeSendDelay = 0.25f; // 누적 입력 후 서버로 보낼 딜레이(초)
    private Coroutine _upgradeCoroutine_Atk;
    private Coroutine _upgradeCoroutine_Hp;
    private SpineAniController _atkCharacterController;
    private SpineAniController _hpCharacterController;

    private readonly string _atkIdleAniName = "at_idle";
    private readonly string _hpIdleAniName = "hp_idle";
    private readonly string _atkUpgradeAniName = "at_action";
    private readonly string _hpUpgradeAniName = "hp_action";
    public readonly List<string> _voiceSoundNames = new List<string>()
    {
        "Voice/PlayerVoice_1",
        "Voice/PlayerVoice_2",
        "Voice/PlayerVoice_3",
        "Voice/PlayerVoice_4"
    };


    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<GrowRandMark>(typeof(GrowRandMarkE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));

        _atkCharacterController = _atkCharacter.Initialize();
        _hpCharacterController = _hpCharacter.Initialize();

        _atkCharacterController.SetEndFunc(_atkUpgradeAniName, () => EndAni(PlayerGrowType.Atk));
        _hpCharacterController.SetEndFunc(_hpUpgradeAniName, () => EndAni(PlayerGrowType.Hp));

        Get<UIButton>(UIButtonE.SafeArea_Atk_PriceButton).AddClickEvent((ped) => OnClickUpgrade(PlayerGrowType.Atk));
        Get<UIButton>(UIButtonE.SafeArea_Hp_PriceButton).AddClickEvent((ped) => OnClickUpgrade(PlayerGrowType.Hp));

        base.Initialize();
    }

    protected override void UISet()
    {
        base.UISet();

        _atkPrice.UISet(GrowFomula.UpgradeItemCode);
        _hpPrice.UISet(GrowFomula.UpgradeItemCode);

        InitSet();
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

        _atkCharacterController.Play(_atkIdleAniName, true);
        _hpCharacterController.Play(_hpIdleAniName, true);

        // Player Data Load
        List<Type> types = new List<Type> { typeof(PlayerGrowDto) };
        Managers.PlayerData.DbGets(types, () =>
        {
            List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();

            int atkLevel = GetAtkLevel(playerGrowDatas);
            int hpLevel = GetHpLevel(playerGrowDatas);
            int growLevel = GetGrowLevel(playerGrowDatas);

            Get<GrowRandMark>(GrowRandMarkE.SafeArea_LandMark).Set();
            AtkSet(atkLevel, growLevel);
            HpSet(hpLevel, growLevel);
        });

    }

    #region Setter

    private void InitSet()
    {
        Get<GrowRandMark>(GrowRandMarkE.SafeArea_LandMark).InitSet();

        AtkSet(0, 0);
        HpSet(0, 0);
    }

    private void AtkSet(int atkLevel, int growLevel)
    {
        GetTextPro(UITextProE.SafeArea_Atk_Title).text = $"공격력 LV.{atkLevel}";
        GetText(UITextE.SafeArea_Atk_Value).text = $"{new BBNumber(GrowFomula.GetAtk(atkLevel)).Alphabet()}";

        int nextDestLevel = (growLevel + 1) * GrowFomula.MaxAtkLevelUnit;
        bool isMax = nextDestLevel <= atkLevel;

        BBNumber price = GrowFomula.GetPrice(atkLevel + 1);
        _atkPrice.SetCount(price, true);

        BBNumber myItemCount = Managers.PlayerData.GetPlayerItemCount(GrowFomula.UpgradeItemCode);

        Get<UIButton>(UIButtonE.SafeArea_Atk_PriceButton).UISet( (myItemCount >= price && !isMax) ? ButtonSprite.Button_Yellow : ButtonSprite.Button_Gray);
    }
    private void HpSet(int hpLevel, int growLevel)
    {
        GetTextPro(UITextProE.SafeArea_Hp_Title).text = $"체력 LV.{hpLevel}";
        GetText(UITextE.SafeArea_Hp_Value).text = $"{new BBNumber(GrowFomula.GetHp(hpLevel)).Alphabet()}";

        int nextDestLevel = (growLevel + 1) * GrowFomula.MaxHpLevelUnit;
        bool isMax = nextDestLevel <= hpLevel;

        BBNumber price = GrowFomula.GetPrice(hpLevel);
        _hpPrice.SetCount(price, true);

        BBNumber myItemCount = Managers.PlayerData.GetPlayerItemCount(GrowFomula.UpgradeItemCode);

        Get<UIButton>(UIButtonE.SafeArea_Hp_PriceButton).UISet( (myItemCount >= price && !isMax) ? ButtonSprite.Button_Yellow : ButtonSprite.Button_Gray);
    }

    #endregion

    #region Getter

    public int GetAtkLevel(List<PlayerGrowDto> playerGrowDatas)
    {
        PlayerGrowType type = PlayerGrowType.Atk;

        int level = 0;

        if (playerGrowDatas == null)
            return 0;

        foreach (var item in playerGrowDatas)
        {
            if (item.Type == type)
            {
                level = Mathf.Max(level, item.Level);
            }
        }

        return level;
    }
    public int GetHpLevel(List<PlayerGrowDto> playerGrowDatas)
    {
        PlayerGrowType type = PlayerGrowType.Hp;

        int level = 0;

        if (playerGrowDatas == null)
            return 0;

        foreach (var item in playerGrowDatas)
        {
            if (item.Type == type)
            {
                level = Mathf.Max(level, item.Level);
            }
        }

        return level;
    }
    public int GetTotalLevel(List<PlayerGrowDto> playerGrowDatas)
    {
        int atkLevel = GetAtkLevel(playerGrowDatas);
        int hpLevel = GetHpLevel(playerGrowDatas);

        return atkLevel + hpLevel;
    }
    public int GetGrowLevel(int totalLevel)
    {
        int growLevel = GrowFomula.CurrentGrowLevel(totalLevel);

        return growLevel;
    }
    public int GetGrowLevel(List<PlayerGrowDto> playerGrowDatas)
    {
        int totalLevel = GetTotalLevel(playerGrowDatas);
        int growLevel = GrowFomula.CurrentGrowLevel(totalLevel);

        return growLevel;
    }
    #endregion

    #region Upgrade

    private void UpgradeRandMarkSet(int plusLevel)
    {
        Get<GrowRandMark>(GrowRandMarkE.SafeArea_LandMark).UpgradeSet(plusLevel);
    }
    private void UpgradeAtkSet(int plusLevel)
    {
        List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();
        int atkLevel = GetAtkLevel(playerGrowDatas);
        int growLevel = GetGrowLevel(playerGrowDatas);
        int newAtkLevel = atkLevel + plusLevel;

        Managers.PlayerData.UsePlayerItem(GrowFomula.UpgradeItemCode, GrowFomula.GetPrice(newAtkLevel));

        AtkSet(newAtkLevel, growLevel);

        _atkCharacterController.Play(_atkUpgradeAniName, false, true);
        PlayVoice();
    }
    private void UpgradeHpSet(int plusLevel)
    {
        List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();
        int hpLevel = GetHpLevel(playerGrowDatas);
        int growLevel = GetGrowLevel(playerGrowDatas);
        int newHpLevel = hpLevel + plusLevel;

        Managers.PlayerData.UsePlayerItem(GrowFomula.UpgradeItemCode, GrowFomula.GetPrice(newHpLevel));

        HpSet(newHpLevel, growLevel);

        _hpCharacterController.Play(_hpUpgradeAniName, false, true);
        PlayVoice();
    }

    private void EndAni(PlayerGrowType type)
    {
        if (type == PlayerGrowType.Atk)
        {
            _atkCharacterController.Play(_atkIdleAniName, true, true);
        }
        else if (type == PlayerGrowType.Hp)
        {
            _hpCharacterController.Play(_hpIdleAniName, true, true);
        }
    }

    private void OnClickUpgrade(PlayerGrowType type)
    {
        BBNumber myItemCount = Managers.PlayerData.GetPlayerItemCount(GrowFomula.UpgradeItemCode);

        if (type == PlayerGrowType.Atk)
        {
            List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();
            int atkLevel = GetAtkLevel(playerGrowDatas);

            if (myItemCount < GrowFomula.GetPrice(atkLevel + _pendingUpgradeCount_Atk + 1))
                return;

            int growLevel = GetGrowLevel(playerGrowDatas);
            int nextDestLevel = (growLevel + 1) * GrowFomula.MaxAtkLevelUnit;
            bool isMax = nextDestLevel <= atkLevel;

            if (isMax)
                return;

            _pendingUpgradeCount_Atk++;
            if (_upgradeCoroutine_Atk != null)
                StopCoroutine(_upgradeCoroutine_Atk);
            _upgradeCoroutine_Atk = StartCoroutine(UpgradeDelayCoroutine(type));

            UpgradeAtkSet(_pendingUpgradeCount_Atk);
            UpgradeRandMarkSet(_pendingUpgradeCount_Atk);
        }
        else if (type == PlayerGrowType.Hp)
        {
            List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();
            int hpLevel = GetHpLevel(playerGrowDatas);

            if (myItemCount < GrowFomula.GetPrice(hpLevel + _pendingUpgradeCount_Hp + 1))
                return;

            int growLevel = GetGrowLevel(playerGrowDatas);
            int nextDestLevel = (growLevel + 1) * GrowFomula.MaxHpLevelUnit;
            bool isMax = nextDestLevel <= hpLevel;

            if (isMax)
                return;

            _pendingUpgradeCount_Hp++;
            if (_upgradeCoroutine_Hp != null)
                StopCoroutine(_upgradeCoroutine_Hp);
            _upgradeCoroutine_Hp = StartCoroutine(UpgradeDelayCoroutine(type));

            UpgradeHpSet(_pendingUpgradeCount_Hp);
            UpgradeRandMarkSet(_pendingUpgradeCount_Hp);
        }
    }

    private IEnumerator UpgradeDelayCoroutine(PlayerGrowType type)
    {
        yield return new WaitForSeconds(_upgradeSendDelay);

        int upgradeCount = 0;
        if (type == PlayerGrowType.Atk)
        {
            upgradeCount = _pendingUpgradeCount_Atk;
            _pendingUpgradeCount_Atk = 0;
            _upgradeCoroutine_Atk = null;
        }
        else if (type == PlayerGrowType.Hp)
        {
            upgradeCount = _pendingUpgradeCount_Hp;
            _pendingUpgradeCount_Hp = 0;
            _upgradeCoroutine_Hp = null;
        }

        if (upgradeCount > 0)
            RequestUpgradeToServer(type, upgradeCount);
    }

    private void RequestUpgradeToServer(PlayerGrowType type, int upgradeCount)
    {
        return;

        PlayerGrowUpgradeRequest req = new PlayerGrowUpgradeRequest()
        {
            Type = type,
            PlusLevel = upgradeCount
        };

        Managers.Web.SendPostRequest<PlayerGrowUpgradeResponse>("player/grow/upgrade",req, (res) =>
        {
            Managers.PlayerData.DbUpdate(res.Datas);

            List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();

            int atkLevel = GetAtkLevel(playerGrowDatas);
            int hpLevel = GetHpLevel(playerGrowDatas);
            int growLevel = GetGrowLevel(playerGrowDatas);

            Get<GrowRandMark>(GrowRandMarkE.SafeArea_LandMark).Set();
            AtkSet(atkLevel, growLevel);
            HpSet(hpLevel, growLevel);
        });
    }

    public void PlayVoice()
    {
        int rIndex = (int)UnityHelper.Random_H(0, _voiceSoundNames.Count);
        Managers.Sound.Play(_voiceSoundNames[rIndex], Sound.InGmae);
    }

    #endregion
	

    public enum UIImageE
    {
		SafeArea_BG,
		SafeArea_BG_Flow1,
		SafeArea_BG_Flow2,
		SafeArea_BG_Flow3,
		SafeArea_Atk,
		SafeArea_Atk_Character,
		SafeArea_Atk_Icon,
		SafeArea_Hp,
		SafeArea_Hp_Character,
		SafeArea_Hp_Icon,
    }
	public enum GrowRandMarkE
    {
		SafeArea_LandMark,
    }
	public enum UITextProE
    {
		SafeArea_Atk_Title,
		SafeArea_Hp_Title,
    }
	public enum UITextE
    {
		SafeArea_Atk_Value,
		SafeArea_Hp_Value,
    }
	public enum UIButtonE
    {
		SafeArea_Atk_PriceButton,
		SafeArea_Hp_PriceButton,
    }
}