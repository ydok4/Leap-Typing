using UnityEngine;
using System.Collections;

public class RadialSelection : MonoBehaviour, ICanvasRaycastFilter {
	
	private RectTransform rectTransform;
	private new BoxCollider2D collider2D;
	
	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		collider2D = GetComponent<BoxCollider2D>();
	}
	
	public bool IsRaycastLocationValid(Vector2 screenPosition, Camera raycastEventCamera) //uGUI callback
	{
		// If we don't have a collider to check against, any raycast can be captured
		if (collider2D == null)
			return true;
		
		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, raycastEventCamera, out localPoint);
		
		Vector2 pivot = rectTransform.pivot - new Vector2(0.5f, 0.5f);
		
		Vector2 pivotScaled = Vector2.Scale(rectTransform.rect.size, pivot);
		
		Vector2 realPoint = localPoint + pivotScaled;
		
		Rect colliderRect = new Rect(
			collider2D.offset.x - collider2D.size.x / 2,
			collider2D.offset.y - collider2D.size.y / 2,
			collider2D.size.x,
			collider2D.size.y); // TODO: CACHE
		
		bool containsRect = colliderRect.Contains(realPoint);
		
		return containsRect;
	}
}
