using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput { get; private set; }
    private PlayerInputSet input;
    #region UI Components
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    public UI_SkillTree skillTreeUI { get; private set; }

    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_Death deathUI { get; private set; }
    public UI_Win winUI { get; private set; }
    public UI_FadeScreen fadeScreenUI { get; private set; }

    #endregion

    private bool skillTreeEnabled;
    private bool inventoryEnabled;
    private void Awake()
    {
        instance = this;

        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathUI = GetComponentInChildren<UI_Death>(true);
        winUI = GetComponentInChildren<UI_Win>(true);
        fadeScreenUI = GetComponentInChildren<UI_FadeScreen>(true);

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }
    private void Start()
    {
        skillTreeUI.UnlockDefaultSkills();
    }
    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.ToggleSkillTreeUI.performed += context => ToggleSkillTreeUI();
        input.UI.ToggleCharacterUI.performed += context => ToggleInvenToryUI();
        input.UI.AlternativeInput.performed += context => alternativeInput = true;
        input.UI.AlternativeInput.canceled += context => alternativeInput = false;

        input.UI.ToggleOptionsUI.performed += context =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }
    public void OpenDeathUI()
    {
        SwitchTo(deathUI.gameObject);
        HideAllToolTips();
        input.Disable();
    }
    public void OpenWinUI()
    {
        SwitchTo(winUI.gameObject);
        HideAllToolTips();
        input.Disable();
    }
    public void OpenOptionsUI()
    {
        SwitchTo(optionsUI.gameObject);
        HideAllToolTips();
        StopPlayerControls(true);
    }

    public void SwitchToInGameUI()
    {
        HideAllToolTips();
        StopPlayerControls(false);
        SwitchTo(inGameUI.gameObject);
        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        objectToSwitchOn.SetActive(true);
    }

    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }
    private void StopPlayerControlsIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;

            }

        }
        StopPlayerControls(false);
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetToolTipsAboveOtherElements();
        fadeScreenUI.transform.SetAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllToolTips();

        StopPlayerControlsIfNeeded();
    }
    public void ToggleInvenToryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetToolTipsAboveOtherElements();
        fadeScreenUI.transform.SetAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        HideAllToolTips();

        StopPlayerControlsIfNeeded();
    }
    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControlsIfNeeded();

        if (openStorageUI == false)
        {
            craftUI.gameObject.SetActive(false);
            HideAllToolTips();
        }
    }
    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControlsIfNeeded();
        

        if (openMerchantUI == false)
            HideAllToolTips();
    }
    public void HideAllToolTips()
    {
        itemToolTip.ShowToolTip(false, null);
        skillToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }
    private void SetToolTipsAboveOtherElements()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }
}
