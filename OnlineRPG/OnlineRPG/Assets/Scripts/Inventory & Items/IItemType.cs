using System.Collections.Generic;

public interface IItemType
{
    List<Option> itemInteractOptions { get; set; }
    void InitializeOptions();
    void ShowInteraction();
}