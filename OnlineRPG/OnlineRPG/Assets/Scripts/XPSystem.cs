using UnityEngine;

[System.Serializable]
public class XPSystem
{
    [Header("Levels")]
    [Tooltip("The max level this Skill can go to")] public int MaxLevel = 98;

    [Space]

    [Header("Experience")]
    [Tooltip("Base XP required to get from lvl 0 to lvl 1")] public float BaseExperienceRequiredForNextLevel = 100;
    [Tooltip("The mutliplier for how much XP is required to get to the next level")] public float ExperienceRequiredForNextLevelMultiplier = 1.103f;
    [Tooltip("Whether or not to round to the nearest integer.")] public bool roundToInt = true;

    public int CurrentLevel = 0;

    public int CurrentLevelReal // Adjusted for display purposes. (Indexing starts at 0, so add 1)
    {
        get
        {
            return CurrentLevel + 1;
        }
    }

    [HideInInspector] public float TotalExperience = 0f;
    [HideInInspector] public float TotalExperienceRequiredForNextLevel = 100f;
    [HideInInspector] public float ExperienceRequiredForNextLevel = 100f;

    public delegate void OnExperienceChangedDelegate();
    public OnExperienceChangedDelegate OnExperienceChanged; // Invoked when the XP changes.

    public float ExperienceForLevel(int level) // returns the xp increase NOT the TOTAL needed.
    {
        if (level < 0) return 0; // If the level you are requesting is negative, return 0 as a placeholder.
        float exp = this.BaseExperienceRequiredForNextLevel * Mathf.Pow(this.ExperienceRequiredForNextLevelMultiplier, level); //This is the xp required to get from one level below to the new level, not the total experience required.
        return roundToInt ? Mathf.RoundToInt(exp) : exp;
    }

    public void GrantXP(float xp)
    {
        this.TotalExperience += xp;

        while (this.TotalExperience >= this.TotalExperienceRequiredForNextLevel)
        {
            if (this.CurrentLevel > this.MaxLevel)
            {
                Debug.LogWarning("Already max level! Cannot level up anymore!");
                return;
            }

            this.CurrentLevel++;
            this.ExperienceRequiredForNextLevel = ExperienceForLevel(this.CurrentLevel);
            this.TotalExperienceRequiredForNextLevel += this.ExperienceRequiredForNextLevel;
        }

        if (OnExperienceChanged != null)
        {
            OnExperienceChanged.Invoke();
        }
    }

    public float PercentageIntoLevel()
    {
        float oldExp = this.TotalExperienceRequiredForNextLevel - ExperienceForLevel(this.CurrentLevel);
        return (this.TotalExperience - oldExp) / (this.TotalExperienceRequiredForNextLevel - oldExp);
    }

    public float XpIntoLevel()
    {
        float exp = PercentageIntoLevel() * ExperienceRequiredForNextLevel;
        return roundToInt ? Mathf.RoundToInt(exp) : exp;
    }
}