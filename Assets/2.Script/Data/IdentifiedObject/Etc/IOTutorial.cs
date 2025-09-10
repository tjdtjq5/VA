using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ETC/Tutorial")]
public class IOTutorial : ScriptableObject
{
    public string CodeName => codeName;
    public string Subject => subject;
    public List<IOTutorialData> Scripts => scripts;
    public TutorialContents TutorialContents => _tutorialContents;

    [SerializeField] private string codeName;
    [SerializeField] private string subject;
    [SerializeField] private TutorialContents _tutorialContents;
    [SerializeField] private List<IOTutorialData> scripts = new List<IOTutorialData>();
}
[System.Serializable]
public struct IOTutorialData
{
    [Multiline(3)] public string script;
}