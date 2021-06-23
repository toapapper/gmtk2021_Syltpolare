public readonly struct InputContext
{
    public readonly float Value;
    public readonly InputState State;

    public InputContext(float value, InputState state)
    {
        Value = value;
        State = state;
    }

    public enum InputState
    {
        Performed,
        Canceled,
    }
}
