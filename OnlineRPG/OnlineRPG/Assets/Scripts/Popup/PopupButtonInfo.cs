public class PopupButtonInfo
{
    #region Delegates
    public delegate void OnPopupButtonClickedDelegate();
    #endregion

    public string ButtonText;
    public OnPopupButtonClickedDelegate OnPopupClick;

    public PopupButtonInfo(string buttonText, OnPopupButtonClickedDelegate onPopupClick)
    {
        this.ButtonText = buttonText;
        this.OnPopupClick = onPopupClick;
    }
}