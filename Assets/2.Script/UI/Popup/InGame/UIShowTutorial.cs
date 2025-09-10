using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIShowTutorial : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));


		_textWait = new WaitForSeconds(0.04F);
		GetButton(UIButtonE.Button).AddClickEvent((ped) => OnClick());

        base.Initialize();
    }
    
	[SerializeField] private Transform _tutorialContentsParent;
    [SerializeField] private List<IOTutorial> tutorials = new List<IOTutorial>();
    
    private IOTutorial _currentTutorial;
	private TutorialContents _currentContents;
    private int _currentPage;
    private bool _isScript;
    private WaitForSeconds _textWait;
    private IEnumerator _showTextOneByOne;

    public void UISet(string codeName)
    {
	    _currentTutorial = tutorials.Find(t => t.CodeName == codeName);

		ContentsSet(_currentTutorial.TutorialContents);

	    GetText(UITextE.Subject).text = _currentTutorial.Subject;

	    this._currentPage = 0;
	    SetPage(_currentPage);
    }

	void ContentsSet(TutorialContents tutorialContents)
	{
		foreach (Transform child in _tutorialContentsParent)
		{
			Managers.Resources.Destroy(child.gameObject);
		}

		_currentContents = Managers.Resources.Instantiate<TutorialContents>(tutorialContents, _tutorialContentsParent);
		_currentContents.Initialize();
	}

    private void SetPage(int page)
    {
	    SetScript(_currentTutorial.Scripts[page].script, true);

		_currentContents.Set(page);
    }
    
    private void SetScript(string script, bool isOneByOne)
    {
	    if (isOneByOne)
	    {
		    if (_showTextOneByOne != null)
			    StopCoroutine(_showTextOneByOne);
		    
		    _showTextOneByOne = ShowTextOneByOne(script);
		    StartCoroutine(_showTextOneByOne);
	    }
	    else
	    {
		    _isScript = false;
		    if (_showTextOneByOne != null)
			    StopCoroutine(_showTextOneByOne);
		    GetText(UITextE.Script).text = script;
	    }
    }

    private void OnClick()
    {
	    if (_isScript)
	    {
		    SetScript(_currentTutorial.Scripts[_currentPage].script, false);
		    return;
	    }
	    
	    _currentPage++;

	    int len = _currentTutorial.Scripts.Count;
	    if (_currentPage >= len)
	    {
		    ClosePopupUI();
	    }
	    else
	    {
		    SetPage(_currentPage);
	    }
    }
    
    IEnumerator ShowTextOneByOne(string text)
    {
	    _isScript = true;
	    GetText(UITextE.Script).text = "";
	    
	    StringBuilder displayText = new StringBuilder();
	    Stack<string> richTagStack = new Stack<string>();
	    bool insideTag = false;
	    StringBuilder currentTag = new StringBuilder();
	    int i = 0;

	    while (i < text.Length)
	    {
		    char c = text[i];

		    if (c == '<')
		    {
			    insideTag = true;
			    currentTag.Clear();
			    currentTag.Append(c);
		    }
		    else if (insideTag)
		    {
			    currentTag.Append(c);
			    if (c == '>')
			    {
				    insideTag = false;
				    string tag = currentTag.ToString();

				    // 여는 태그면 스택에 push, 닫는 태그면 pop
				    if (tag.StartsWith("</"))
				    {
					    if (richTagStack.Count > 0)
						    richTagStack.Pop(); // 마지막 스타일 제거
				    }
				    else
				    {
					    richTagStack.Push(tag);
				    }
			    }
		    }
		    else
		    {
			    // 일반 문자 출력
			    displayText.Clear();

			    // 현재 활성화된 스타일 적용
			    foreach (var tag in richTagStack)
				    displayText.Insert(0, tag);

			    displayText.Append(text[i]);

			    // 닫는 태그 자동 추가
			    foreach (var tag in richTagStack)
			    {
				    string tagName = tag.Substring(1, tag.Length - 2).Split('=')[0];
				    displayText.Append($"</{tagName}>");
			    }

			    GetText(UITextE.Script).text += displayText.ToString();
			    yield return _textWait;
		    }

		    i++;
	    }
	    
	    _isScript = false;
    }

	public enum UIImageE
    {
		Black,
    }
	public enum UITextE
    {
		Subject,
		Script,
    }
	public enum UIButtonE
    {
		Button,
    }
}