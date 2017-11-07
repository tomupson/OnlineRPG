using System.Collections.Generic;

public interface IInteractable
{
    List<Option> interactOptions { get; set; }
    void OpenContextMenu();
    void InitializeOptions();
}