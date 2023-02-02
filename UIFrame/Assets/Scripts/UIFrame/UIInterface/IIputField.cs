using UnityEngine.Events;

namespace UIInterface
{
    public interface IIputField
    {
        void AddOnValueChangeListener(UnityAction<string> action);
        string GetInputFieldText();
        void SetInputFieldText(string text);
    }
}
