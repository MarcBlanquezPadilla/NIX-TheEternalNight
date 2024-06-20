using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_2 : MonoBehaviour
{

    public Animator menuHome;
    public Animator menuOptions;
    public Animator menuHUD;
    public Animator menuPause;
    public Animator menuinventory;

    int menuIndex;


    void Start()
    {
        menuHome.SetTrigger("Show");
        menuIndex = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void HideCurrentMenu()
    {
        if (menuIndex == 0) { menuHome.SetTrigger("Hide"); }
        else if (menuIndex == 1) { menuOptions.SetTrigger("Hide"); }
        else if (menuIndex == 3) { menuHUD.SetTrigger("Hide"); }
        else if (menuIndex == 5) {menuinventory.SetTrigger("Hide");}
        else { menuPause.SetTrigger("Hide"); }
    }

    public void ShowMenuHome()
    {
        HideCurrentMenu();
        menuHome.SetTrigger("Show");
        menuIndex = 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

     public void DesplegarJugar()
    {
        menuHome.SetTrigger("Desplegar");
    }

}
