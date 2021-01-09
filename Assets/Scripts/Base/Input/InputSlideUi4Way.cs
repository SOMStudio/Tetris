using System;
using System.Collections.Generic;
using Base.Input.InputImplementation;
using Base.Input.Interfaces;
using Base.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Base.Input
{
	public class InputSlideUi4Way : ExtendedCustomMonoBehaviour, IInputManager, IPointerDownHandler, IDragHandler, IPointerUpHandler {

		[Header("Slide Clump")]
		public float slideClump = 100f;

		private InputBindings inputBindings;
		private IMouseInputHandler mouseInputHandler;
		private Dictionary<string, UnityAction> actionMapKeyUp = new Dictionary<string, UnityAction>();
		private Dictionary<string, UnityAction> actionMapKeyDown = new Dictionary<string, UnityAction>();
		
		private Vector2 _shiftClick;
		private Vector2 _startPosition = Vector2.zero;

		private float _ver = 0.0f;
		private float _hor = 0.0f;

		protected override void Init()
		{
			base.Init();
		}

		public void InitBindings(InputBindings inputBindings)
		{
			this.inputBindings = inputBindings;
		}

		public void InitRadials(IMouseInputHandler inputRadials)
		{
			mouseInputHandler = inputRadials;
		}

		public void OnPointerDown (PointerEventData data) {
			if (_startPosition == Vector2.zero)
				_startPosition = new Vector2(myTransform.position.x, myTransform.position.y);
			
			_shiftClick = data.position - _startPosition;

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
			Vector2 vectorToPoint = data.position - _startPosition;
			Vector2 dirToPoint = vectorToPoint.normalized;
			float distanceToPoint = Mathf.Clamp (vectorToPoint.magnitude, -slideClump, slideClump);
			Vector2 controlPos = dirToPoint * distanceToPoint;
			
			_ver = controlPos.y / slideClump;
			_hor = controlPos.x / slideClump;
			
			myTransform.position = new Vector3 (_startPosition.x + controlPos.x, _startPosition.y + controlPos.y, myTransform.position.z);
		}

		public void OnPointerUp (PointerEventData data) {
			myTransform.position = new Vector3(_startPosition.x, _startPosition.y, myTransform.position.z);
			
			_ver = 0f;
			_hor = 0f;

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
				return _hor;
			} else if (axisName == "Vertical")
			{
				return _ver;
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
