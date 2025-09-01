using System;

public interface IClickable
{
    StateType currentState { get; }
    void OnSelect();
    void OnDeselect();
}
