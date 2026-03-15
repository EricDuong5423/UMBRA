using UnityEngine;

public class TabManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private UITabButton inventoryButton;
    [SerializeField] private UITabButton statsButton;
    [SerializeField] private UITabButton settingsButton;

    [Header("Sections")]
    [SerializeField] private TabSection inventorySection;
    [SerializeField] private TabSection statsSection;
    [SerializeField] private TabSection settingsSection;

    private UITabButton[] buttons;
    private TabSection[] sections;
    private int currentIndex = -1;

    private void Awake()
    {
        buttons = new UITabButton[] { inventoryButton, statsButton, settingsButton };
        sections = new TabSection[] { inventorySection, statsSection, settingsSection };

        inventoryButton.OnClick += () => SelectTab(0);
        statsButton.OnClick += () => SelectTab(1);
        settingsButton.OnClick += () => SelectTab(2);
        foreach (var section in sections)
            section.HideImmediate();
        SelectTab(0);
    }

    private void OnEnable()
    {
        SelectTab(0);
    }

    private void OnDisable()
    {
        currentIndex = -1;
    }

    private void SelectTab(int index)
    {
        if (currentIndex == index) return;

        currentIndex = index;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetSelected(i == index);

            if (i == index)
                sections[i].Show();
            else
                sections[i].Hide();
        }
    }
}