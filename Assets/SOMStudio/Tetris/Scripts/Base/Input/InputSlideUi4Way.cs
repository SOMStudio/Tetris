using System;
using System.Collections.Generic;
using SOMStudio.Tetris.Scripts.Base.Input.InputImplementation;
using SOMStudio.Tetris.Scripts.Base.Input.Interfaces;
using SOMStudio.Tetris.Scripts.Base.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SOMStudio.Tetris.Scripts.Base.Input
{
	public class InputSlideUi4Way : ExtendedCustomMonoBehaviour, IInputManager, IPointerDownHandler, IDragHandler, IPointerUpHandler {

		[Header("Slide Clump")]
		public float slideClump = 100f;

		private InputBindings inputBindings;
		private IMouseInputHandler mouseInputHandler;
		private readonly Dictionary<string, UnityAction> actionMapKeyUp = new();
		private readonly Dictionary<string, UnityAction> actionMapKeyDown = new();
		
		private Vector2 shiftClick;
		private Vector2 startPosition = Vector2.zero;

		private float vertical;
		private float horizontal;

		public void InitBindings(InputBindings inputBindings)
		{
			this.inputBindings = inputBindings;
		}

		public void InitRadials(IMouseInputHandler inputRadials)
		{
			mouseInputHandler = inputRadials;
		}

		public void OnPointerDown (PointerEventData data) {
			if (startPosition == Vector2.zero)
				startPosition = new Vector2(myTransform.position.x, myTransform.position.y);
			
			shiftClick = data.position - startPosition;

			if (inputBindings == null) return;
			
			foreach (var kvp in inputBindings.KeyBindings)
			{
				if (actionMapKeyDown.TryGetValue(kvp.Key, out var action))
				{
					action.Invoke();
				}
			}
		}

		public void OnDrag(PointerEventData data) {
			Vector2 vectorToPoint = data.position - startPosition;
			Vector2 dirToPoint = vectorToPoint.normalized;
			float distanceToPoint = Mathf.Clamp (vectorToPoint.magnitude, -slideClump, slideClump);
			Vector2 controlPos = dirToPoint * distanceToPoint;
			
			vertical = controlPos.y / slideClump;
			horizontal = controlPos.x / slideClump;
			
			myTransform.position = new Vector3 (startPosition.x + controlPos.x, startPosition.y + controlPos.y, myTransform.position.z);
		}

		public void OnPointerUp (PointerEventData data) {
			myTransform.position = new Vector3(startPosition.x, startPosition.y, myTransform.position.z);
			
			vertical = 0f;
			horizontal = 0f;

			if (inputBindings == null) return;
			
			foreach (var kvp in inputBindings.KeyBindings)
			{
				if (actionMapKeyUp.TryGetValue(kvp.Key, out var action))
				{
					action.Invoke();
				}
			}
		}

		public void AddActionToBindingKeyUp(string binding, UnityAction action)
		{
			actionMapKeyUp.Add(binding, action);
		}

		public void AddActionToBindingKeyDown(string binding, UnityAction action)
		{
			actionMapKeyDown.Add(binding, action);
		}

		public float GetAxis(string axisName)
		{
			if (axisName == "Horizontal")
			{
				return horizontal;
			} else if (axisName == "Vertical")
			{
				return vertical;
			}

			throw new NotImplementedException();
		}

		public bool GetButton(string buttonName)
		{
			throw new NotImplementedException();
		}

		public Vector2 GetMouseVector(Vector2 relativePosition)
		{
			if (mouseInputHandler != null)
				return mouseInputHandler.GetInput(relativePosition);
			else
				throw new NotImplementedException();
		}

		public void CheckForInput()
		{
			throw new NotImplementedException();
		}
	}
}
