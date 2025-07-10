﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseoverScript : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	public void OnSelect(BaseEventData eventData) => mouseOver.Invoke();
	
	public void OnPointerEnter(PointerEventData eventData) => mouseOver.Invoke();

	public void OnDeselect(BaseEventData eventData) => mouseLeave.Invoke();

	public void OnPointerExit(PointerEventData eventData) => mouseLeave.Invoke();

	[Header("Events")]
	[SerializeField] private UnityEvent mouseOver;
	[SerializeField] private UnityEvent mouseLeave;
	
}
