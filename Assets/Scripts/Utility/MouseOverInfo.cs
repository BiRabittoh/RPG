using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ActionButton))]
public class MouseOverInfo : MonoBehaviour
{
    public Text status_text;
    
    private PointerEventData pe = new PointerEventData(EventSystem.current);
    private List<RaycastResult> hits;
    private ActionButton btn = null;
    private ActionButton oldBtn = null;

    private void Start()
    {
        oldBtn = GetComponent<ActionButton>();
    }

    void Update()
    {
        pe.position = Input.mousePosition;
        hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits); //Raycast

        foreach (RaycastResult h in hits)
        {
            btn = h.gameObject.GetComponent<ActionButton>();
            if (btn)
            {
                if (!btn.Equals(oldBtn))
                {
                    oldBtn = btn;
                    UI.changeText(status_text, btn.getDescription());
                }
            }
        }
    }
}
