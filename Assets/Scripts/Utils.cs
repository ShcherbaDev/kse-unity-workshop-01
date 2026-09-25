using UnityEngine;

public static class Utils
{
	public static (float, float) GetHorizontalEdges(float margin = 0f)
	{
		Camera cam = Camera.main;
		float cameraLeftEdge = cam.ViewportToWorldPoint(Vector2.zero).x + margin;
		float cameraRightEdge = cam.ViewportToWorldPoint(Vector2.one).x + margin;
		return (cameraLeftEdge, cameraRightEdge);
	}
}
