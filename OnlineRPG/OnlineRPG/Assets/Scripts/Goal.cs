using UnityEngine;

public class Goal : MonoBehaviour
{
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public virtual void Init()
    {

    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount) Complete();
    }

    public void Complete() => Completed = true;
}