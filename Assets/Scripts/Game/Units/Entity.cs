using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Units 
{
	public class Entity
	{
		protected Attributes.AttributeManager stats_;
		protected GameController game_;
		protected View.UnitView view_;
		protected Vector3 position_;
		private int groupID_;
		private bool moving_ = false;
		private bool firstPositionSet_ = true;

		public static Vector2 ToRight = Vector2.right;

		public Entity()
		{
			stats_ = new LootQuest.Game.Attributes.AttributeManager ();
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.Health));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init (LootQuest.Game.Attributes.AttributeID.MovementSpeed));
		}

		public Attributes.AttributeManager Stats
		{
			get { return stats_; }
		}

		public int GroupID
		{
			get { return groupID_; }
			set { groupID_ = value; }
		}

		public GameController Game
		{
			set { game_ = value; }
			get { return game_; } 
		}

		public bool Moving
		{
			protected set { moving_ = value; }
			get { return moving_; }
		}

		public View.UnitView View
		{
			get { return view_; }
			protected set { view_ = value; }
		}

		public virtual void Update()
		{
			if (moving_) 
			{
				float ms = stats_.GetFinValue(LootQuest.Game.Attributes.AttributeID.MovementSpeed) * Time.deltaTime;
				Position = Position + Direction.Scale(ms);
				UpdateView();
			}
		}

		public Vector2 Center()
		{
			return Position + View.GetCenter ();
		}

		private Vector2 direction_;
		public Vector2 Direction
		{
			set { direction_ = value; direction_.Normalize(); }
			get { return direction_; }
		}

		public Vector2 Position
		{
			set 
			{
				position_ = new Vector3(value.x, value.y, 0.0f);
				
				if (firstPositionSet_)
				{
					UpdateView();
					firstPositionSet_ = false;
				}
			}
			
			get { return new Vector2(position_.x, position_.y); }
		}

		public float Y
		{
			set 
			{
				position_ = new Vector3(value, 0.0f, 0.0f);
				
				if (firstPositionSet_)
				{
					UpdateView();
					firstPositionSet_ = false;
				}
			}

			get { return position_.y; }
		}

		public float X
		{
			set
			{
				position_ = new Vector3(value, 0.0f, 0.0f);

				if (firstPositionSet_)
				{
					UpdateView();
					firstPositionSet_ = false;
				}
			}
			
			get { return position_.x; }
		}

		public virtual void OnAddedToGame()
		{
			if (view_ != null) 
			{
				view_.Reset();
			}
		}

		public virtual void OnRemoveFromGame()
		{
			firstPositionSet_ = true;
		}

		public void Stop()
		{
			moving_ = false;
		}

		public void Move()
		{
			moving_ = true;
		}

		private void UpdateView()
		{
			if (view_ != null) 
			{
				view_.gameObject.transform.localPosition = position_;
			}
		}

		public virtual bool CanAttack(Entity target)
		{
			return target != this && target.groupID_ != groupID_;
		}

		public virtual bool NeedToRemove()
		{
			return false;
		}
	}
}