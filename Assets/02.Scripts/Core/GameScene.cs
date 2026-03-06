using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void Start()
    {
        CharacterSpawner.Instance.SpawnPlayer();
    }
}