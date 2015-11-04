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

		public Entity()
		{
			stats_ = new LootQuest.Game.Attributes.AttributeManager ();
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.Health));
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
				float ms = stats_.GetFinValue(LootQuest.Game.Attributes.AttributeID.MovementSpeed);
				X += Time.deltaTime * direction_ * ms;
				UpdateView();
			}
		}

		private float direction_;
		
		public float Direction
		{
			set { direction_ = value; }
			get { return direction_; }
		}
		
		public float X
		{
			set
			{
				position_ = new Vector3(value, 0.0f, 0.0f);
			}
			
			get 
			{
				return position_.x;
			}
		}

		public virtual void OnRemoveFromGame()
		{

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
	}
}