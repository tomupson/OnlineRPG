using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    #region Singleton
    public static InteractionManager singleton;
    #endregion

    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject interactionGO;
    [SerializeField] private GameObject interactionContentGO;
    [SerializeField] private GameObject optionPrefab;

    Option cancelOption;
    List<Option> options;

    void Awake()
    {
        if (singleton == null) singleton = this;
        else Debug.LogError("Already an InteractionManager in the scene!");
    }

    void Start()
    {
        interactionGO.SetActive(false);
        InitializeOptions();
    }

    void InitializeOptions()
    {
        this.options = new List<Option>();

        cancelOption = new Option()
        {
            Text = "Cancel",
            OnOptionClick = Hide
        };
    }

    public void SetOptions(List<Option> options, bool usePresetCancelOption = false)
    {
        this.options.Clear(); // Not entirely necessary but doing it anyway to be safe.
        this.options.AddRange(options);
        // There's an option to use the default cancel button with the only functionality being to hide the menu.
        if (usePresetCancelOption)
            this.options.Add(cancelOption);
    }

    public void Show()
    {
        interactionGO.SetActive(true);
        interactionGO.transform.position = Input.mousePosition;
        Clear();

        foreach (Option option in options)
        {
            GameObject o = Instantiate(optionPrefab, interactionContentGO.transform, false);
            o.GetComponentInChildren<TMP_Text>().text = option.Text;
            o.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (option.OnOptionClick != null)
                    option.OnOptionClick.Invoke();
            });
        }
    }

    public void Hide()
    {
        interactionGO.SetActive(false);
    }

    void Clear()
    {
        foreach (Transform child in interactionContentGO.transform)
        {
            Destroy(child.gameObject);
        }
    }
}