namespace Application.Common.Models;

public class ActionWithDialogModel<T>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public T Data { get; set; }
}