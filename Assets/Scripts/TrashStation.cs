using UnityEngine;

public class TrashStation : Interactable
{
    public override void Action()
    {
        MainChar.SetPlayerState(PlayerState.Normal);

        base.Action();
    }
}
