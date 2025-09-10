using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LanguageManager
{
    public Action OnChangeLanguage;

    private Font _font;
    private TMP_FontAsset _tmpFont;
    private TMP_FontAsset _tmpFontOutline;

    private AsyncOperationHandle<Font> _fontHandle;
    private AsyncOperationHandle<TMP_FontAsset> _tmpFontHandle;
    private AsyncOperationHandle<TMP_FontAsset> _tmpFontOutlineHandle;

    public Font Font => _font;
    public TMP_FontAsset TMP_Font => _tmpFont;
    public TMP_FontAsset TMP_FontOutline => _tmpFontOutline;

    private bool _isChanging;
    private Language _currentLanguage;

    public void Initialize()
    {
        ChangeLanguage(Language.KR);
    }

    public async Task ChangeLanguage(Language language)
    {
        if (_isChanging || _currentLanguage == language)
            return;

        _isChanging = true;
        _currentLanguage = language;

        try
        {
            ReleaseLanguage();
            await LoadLanguage(language);
            OnChangeLanguage?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to change language: {e.Message}");
        }
        finally
        {
            _isChanging = false;
        }
    }

    private async Task LoadLanguage(Language language)
    {
        try
        {
            string fontName = $"Font_{language}";
            string tmpFontName = $"FontPro_{language}";
            string tmpFontOutlineName = $"FontOutlinePro_{language}";

            _fontHandle = Addressables.LoadAssetAsync<Font>(fontName);
            _tmpFontHandle = Addressables.LoadAssetAsync<TMP_FontAsset>(tmpFontName);
            _tmpFontOutlineHandle = Addressables.LoadAssetAsync<TMP_FontAsset>(tmpFontOutlineName);

            await _fontHandle.Task;
            await _tmpFontHandle.Task;
            await _tmpFontOutlineHandle.Task;

            if (_fontHandle.Status == AsyncOperationStatus.Succeeded)
                _font = _fontHandle.Result;
            else
                Debug.LogWarning($"Failed to load font: {fontName}");
            
            if (_tmpFontHandle.Status == AsyncOperationStatus.Succeeded)
                _tmpFont = _tmpFontHandle.Result;
            else
                Debug.LogWarning($"Failed to load TMP font: {tmpFontName}");
            
            if (_tmpFontOutlineHandle.Status == AsyncOperationStatus.Succeeded)
                _tmpFontOutline = _tmpFontOutlineHandle.Result;
            else
                Debug.LogWarning($"Failed to load TMP font outline: {tmpFontOutlineName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load language resources: {e.Message}");
            throw;
        }
    }

    private void ReleaseLanguage()
    {
        if (_fontHandle.IsValid())
            Addressables.Release(_fontHandle);
        
        if (_tmpFontHandle.IsValid())
            Addressables.Release(_tmpFontHandle);
        
        if (_tmpFontOutlineHandle.IsValid())
            Addressables.Release(_tmpFontOutlineHandle);

        _font = null;
        _tmpFont = null;
        _tmpFontOutline = null;
    }
}

public enum Language
{
    CN,
    TW,
    DE,
    EN,
    FR,
    JP,
    KR,
    TH,
    VN,
}

