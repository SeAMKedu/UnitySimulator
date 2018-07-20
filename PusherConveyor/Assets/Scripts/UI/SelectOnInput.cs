using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour 
{
    private EventSystem eventSystem;

    [SerializeField]
    private GameObject selectedObject;

    bool buttonSelected;

    void Start () 
	{
        eventSystem = EventSystem.current;
	}
	
	void Update () 
	{
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
