using UnityEngine;
using UnityEngine.Events;

public class PuzzleCubosController : MonoBehaviour
{

    [SerializeField] private GameObject pinkCubeObj;
    [SerializeField] private GameObject blueCubeObj;

    bool pinkCube = false;
    bool blueCube = false;

    [SerializeField] private UnityEvent onEnabled;

    public void TryToEnableBlue()
    {
        if (InventoryManager.Instance.ReturnAmountOfItem("BlueCube") > 0)
        {
            blueCube = true;
            blueCubeObj.SetActive(true);
            InventoryManager.Instance.RemoveItem("BlueCube");

            if (pinkCube && blueCube)
            {
                onEnabled.Invoke();
            }
        }
    }
   
    public void TryToEnablePink()
    {
        if (InventoryManager.Instance.ReturnAmountOfItem("PinkCube") > 0)
        {
            pinkCube = true;
            pinkCubeObj.SetActive(true);
            InventoryManager.Instance.RemoveItem("PinkCube");

            if (pinkCube && blueCube)
            {
                onEnabled.Invoke();
            }
        }
    }
}
