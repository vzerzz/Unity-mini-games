
using UnityEngine;

namespace UIInterface
{
    public interface IImage
    {
        void SetSprite(Sprite sprite);
        Sprite GetSprite();
        void SetImageColor(Color color);
        Color GetImageColor();
    }
}
