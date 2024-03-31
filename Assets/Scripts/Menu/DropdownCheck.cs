using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DropdownCheck : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public void SelectedText() {
        Debug.Log(dropdown.value);
        Debug.Log(dropdown.options[dropdown.value].text);
    }
}