public abstract class BearState
{
    protected readonly BearController _controller;
    protected abstract int AnimTriggerHash { get; }

    public virtual void Enter()
    {
        _controller.Animator.SetTrigger(AnimTriggerHash);
    }
    public virtual void Update() { }
    public virtual void Exit() { }

    public BearState(BearController controller)
    {
        _controller = controller;
    }
}