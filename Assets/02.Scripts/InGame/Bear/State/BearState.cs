public abstract class BearState
{
    protected BearController _controller;
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    public BearState(BearController controller)
    {
        _controller = controller;
    }
}