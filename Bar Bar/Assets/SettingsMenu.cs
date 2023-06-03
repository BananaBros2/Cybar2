using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public Dropdown screenDropdown;

    private void Start()
    {
        screenDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(screenDropdown); });
    }
    
    void DropdownItemSelected(Dropdown screenDropdown)
    {
        int index = screenDropdown.value;
        if(index == 0) { Screen.fullScreenMode = FullScreenMode.FullScreenWindow; }
        else if (index == 1) { Screen.fullScreenMode = FullScreenMode.Windowed; }
        else if (index == 2) { Screen.fullScreenMode = FullScreenMode.MaximizedWindow; }
    }

}
