using UnityEngine;

[System.Serializable]
public class XPSystem
{
    [Header("Levels")]
    [Tooltip("The max level this Skill can go to")]
    public int MaxLevel = 98;

    [Space]

    [Header("Experience")]
    [Tooltip("Base XP required to get from lvl 0 to lvl 1")]
    public float BaseExperienceRequiredForNextLevel = 100;
    [Tooltip("The mutliplier for how much XP is required to get to the next level")] public float ExperienceRequiredForNextLevelMultiplier = 1.103f;

    private int currentLevel = 0;

    public int CurrentLevel
    {
        get
        {
            return currentLevel + 1;
        }
        set
        {
            currentLevel = value - 1;
        }
    }

    [HideInInspector] public float TotalExperience = 0;
    [HideInInspector] public float TotalExperienceRequiredForNextLevel = 100;

    public delegate void OnExperienceChangedDelegate();
    public OnExperienceChangedDelegate OnExperienceChanged; // Invoked when the XP changes.

    public float ExperienceForLevel(int level)
    {
        if (level < 0) return 0; // If the level you are requesting is negative, return 0 as a placeholder.
        return this.BaseExperienceRequiredForNextLevel * Mathf.Pow(this.ExperienceRequiredForNextLevelMultiplier, level); //This is the xp required to get from one level below to the new level, not the total experience required.
    }

    public void GrantXP(float xp)
    {
        this.TotalExperience += xp;

        while (this.TotalExperience >= this.TotalExperienceRequiredForNextLevel)
        {
            if (this.currentLevel > this.MaxLevel)
            {
                Debug.LogWarning("Already max level! Cannot level up anymore!");
                return;
            }

            this.currentLevel++;
            this.TotalExperienceRequiredForNextLevel += ExperienceForLevel(this.currentLevel);
        }

        if (OnExperienceChanged != null)
        {
            OnExperienceChanged.Invoke();
        }
    }

    public float PercentageIntoLevel()
    {
        float oldExp = this.TotalExperienceRequiredForNextLevel - ExperienceForLevel(this.currentLevel);
        return (this.TotalExperience - oldExp) / (this.TotalExperienceRequiredForNextLevel - oldExp);
    }
}