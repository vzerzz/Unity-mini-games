using UnityEngine.Events;

namespace UIInterface
{
    public interface IButton
    {
        void AddOnClickListener(UnityAction action);
    }
}
