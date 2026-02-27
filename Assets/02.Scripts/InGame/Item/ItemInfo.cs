public readonly struct ItemInfo
{
    public readonly int ViewId;
    public readonly int ActorNumber;
    public readonly int ScoreAmount;

    public ItemInfo(int viewId, int actorNumber, int scoreAmount)
    {
        ViewId = viewId;
        ActorNumber = actorNumber;
        ScoreAmount = scoreAmount;
    }
}