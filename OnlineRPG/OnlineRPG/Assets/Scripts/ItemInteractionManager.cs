using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ItemInteractionManager : MonoBehaviour
{
    public static ItemInteractionManager instance;

    [SerializeField] private List<Option> baseOptions;
    [SerializeField] private GameObject interactionGO;
    [SerializeField] private GameObject interactionContentGO;
    [SerializeField] private GameObject optionPrefab;
    private List<Option> options;

    void Awake()
    {
        if (instance == null) instance = this;
        else Debug.LogError("Already an ItemInteractionManager in the scene!");
    }

    void Start()
    {
        interactionGO.SetActive(false);
        options = new List<Option>();
    }

    public void SetOptions(List<Option> options)
    {
        this.options.Clear();
        this.options.AddRange(this.baseOptions);
        if (options != null)
        {
            for (int i = 0; i < options.Count; i++)
            {
                this.options.Insert((i + 1), options[i]);
            }
        }
    }

    public void Show()
    {
        interactionGO.SetActive(true);
        interactionGO.transform.position = Input.mousePosition;

        foreach (Option option in options)
        {
            GameObject o = Instantiate(optionPrefab, interactionContentGO.transform, false);
            o.GetComponentInChildren<TextMeshProUGUI>().text = option.text;
            o.GetComponent<Button>().onClick.AddListener(() =>
            {
                option.onClick.Invoke();
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

    public void DropItem()
    {
        FindObjectOfType<Inventory>().RemoveItem("item_logs", 1);
    }
}