using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Root_Arrow;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Root_Arrow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        Root_Arrow.SetActive(false);

    }
}
