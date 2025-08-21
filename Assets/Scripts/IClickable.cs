using System;

public interface IClickable
{
    StateType CurrentState { get; }
    void OnSelect();
    void OnDeselect();
}
