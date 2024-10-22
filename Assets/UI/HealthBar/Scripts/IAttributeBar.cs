
using UnityEngine;

public interface IAttributeBar
{
    Vector2 BarPosition { get; set; }
    void CenterBarPosX();
    void SetBarDisplayWidth(float width);
    void SetBarAmount(float percentage);
}
