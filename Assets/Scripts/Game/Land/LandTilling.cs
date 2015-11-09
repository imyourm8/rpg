using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Game.Land 
{
	public class LandTilling : BaseLand 
	{
		private GameObject landTile_;
		private List<SpriteRenderer> tiles_ = new List<SpriteRenderer>();
		private List<SpriteRenderer> tilesToMove_ = new List<SpriteRenderer>();
		private Vector3 prevCameraPosition_;

		public override void Prepare()
		{
			landTile_ = Resources.Load<GameObject> ("Prefabs/Land");
			prevCameraPosition_ = camera_.transform.localPosition;

			//generate land tiles
			var spriteBounds = landTile_.GetComponent<SpriteRenderer> ().bounds;
			var cameraBounds = Utils.CameraUtils.OrthographicBounds (camera_);
			float widthAccumulated = 0;

			while (widthAccumulated <= cameraBounds.size.x+spriteBounds.size.x) 
			{
				var tile = landTile_.GetPooled();
				tile.transform.localPosition = new Vector3(widthAccumulated, 0.0f, 0.0f);
				tiles_.Add(tile.GetComponent<SpriteRenderer>());
				root_.AddChild(tile);

				widthAccumulated += spriteBounds.size.x;
			}
		}

		public override void Update()
		{
			var direction = camera_.transform.localPosition - prevCameraPosition_;
			prevCameraPosition_ = camera_.transform.localPosition;
			if (direction.magnitude.Equals (0.0f))
				return;
			tilesToMove_.Clear ();
			var cameraBounds = Utils.CameraUtils.OrthographicBounds (camera_);
			int count = tiles_.Count;
			var lastTilePosition = direction.x > 0.0f ? tiles_ [tiles_.Count - 1].transform.localPosition.x : tiles_ [0].transform.localPosition.x;
			for (int i = 0; i < count; ++i)
			{
				var tile = tiles_[i];
				var tilePosition = tile.gameObject.transform.localPosition;
				var tileBounds = tile.bounds;
				tileBounds.min = camera_.WorldToViewportPoint(tileBounds.min);
				tileBounds.max = camera_.WorldToViewportPoint(tileBounds.max);

				if (direction.x > 0.0f && tileBounds.center.x+tileBounds.size.x < 0.0f || 
				    direction.x < 0.0f && tileBounds.max.x-tileBounds.size.x > 1.0f)
				{
					tilesToMove_.Add(tile);
				}
			}

			count = tilesToMove_.Count;
			for (int i = 0; i < count; ++i) 
			{
				var tile = tilesToMove_[i];
				tiles_.Remove(tile);
				var position = tile.gameObject.transform.localPosition;
				if (direction.x > 0)
				{
					lastTilePosition += tile.bounds.size.x;
					tile.gameObject.transform.localPosition = new Vector3(lastTilePosition, position.y, position.z);
				}
				tiles_.Add(tile);
			}
		}
	}
}