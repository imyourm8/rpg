using UnityEngine;
using System.Collections;

namespace LootQuest.Utils
{
	public sealed class CameraUtils
	{
		public static void SetupViewportFromRectTransform(Camera uiCam, Camera innerCam, RectTransform rect)
		{
			float mainCameraHeight = uiCam.orthographicSize * 2;
			float relativeHeight = mainCameraHeight;
			
			Vector3[] corners = new Vector3[4];
			rect.GetWorldCorners(corners);
			
			float rectHeight = corners[1].y - corners[0].y;
			relativeHeight = rectHeight / relativeHeight;
			innerCam.orthographicSize = uiCam.orthographicSize * relativeHeight;
			
			// Solution is taken from http://forum.unity3d.com/threads/recttransform-rect-to-viewport-rect.298293/
			for (int i = 0; i < 4; i++)
			{
				corners[i] = uiCam.WorldToViewportPoint(corners[i]);
			}
			
			innerCam.rect = new Rect(corners[0].x, corners[0].y, corners[3].x - corners[0].x, corners[1].y - corners[0].y);
		}

		public static Bounds OrthographicBounds(Camera camera)
		{
			float screenAspect = (float)Screen.width / (float)Screen.height;
			float cameraHeight = camera.orthographicSize * 2;
			Bounds bounds = new Bounds(
				camera.transform.position,
				new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
			return bounds;
		}
	}
}