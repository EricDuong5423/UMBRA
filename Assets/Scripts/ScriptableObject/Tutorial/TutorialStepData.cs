using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewTutorialStep", menuName = "Tutorial/Tutorial Step")]
public class TutorialStepData : ScriptableObject
{
    [Header("Contents")] 
    public string Title;
    [TextArea(2, 5)] public string Description;
    public Sprite ExampleImage;
    public Sprite Icon;
    public KeyCode keyToPress = KeyCode.None;

    [Header("Behavior")] 
    public bool PauseGameWhileShowing = false;

    public float CloseAfterSecond = 0f;
}
