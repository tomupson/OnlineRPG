public interface IOptionHandler
{
    //Dictionary<string, object> valuesToBeChanged { get; set; }
    void ApplySettings();
    void CancelChanges();
}