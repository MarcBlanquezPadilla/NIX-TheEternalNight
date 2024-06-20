using UnityEngine;

public class SavedDataManager : MonoBehaviour
{

    private void Awake()
    {
        SavedGamesContainer.Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SavedGamesContainer.CreateNewGame();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SavedGamesContainer.Save();
        }
    }
}
