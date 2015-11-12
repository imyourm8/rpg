using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LootQuest.Game.Units;

namespace LootQuest.Game {
    public class GameController : Scene, IEntityControl, ITargetProvider
    {
    	[SerializeField]
    	private Camera uiCamera;
    	
    	[SerializeField]
    	private Camera gameCamera;

		[SerializeField]
		private GameObject landRoot_;

		private bool started_;
		protected Land.BaseLand land_;
    	
		private List<Entity> entities_;
		private List<Unit> unitCache_;
		private HashSet<Entity> removeList_;
		private HashSet<Entity> addList_;
        protected GameRules.BaseRule rules_;
        
		public GameController()
		{
			entities_ = new List<Entity> ();
			unitCache_ = new List<Unit> ();
			removeList_ = new HashSet<Entity> ();
			addList_ = new HashSet<Entity> ();
		}

    	void Update () 
    	{
			if (!started_)
				return;

			land_.Update ();
			RemoveAll ();
			AddAll();
			UpdateLogic ();
    	}

        public GameRules.BaseRule Rules
        {
            set { rules_ = value;  }
            protected get { return rules_;  }
        }

		protected Camera GameCamera
		{
			get { return gameCamera; }
		}

		private void RemoveAll()
		{
			foreach(var entity in removeList_)
			{
				land_.Remove (entity);
				entities_.Remove (entity);
				OnEntityRemoved(entity);
			}
			removeList_.Clear ();
		}

		private void AddAll()
		{
			foreach (var entity in addList_) 
			{
				land_.Add (entity);
				entities_.Add (entity);
				
				entity.OnAddedToGame ();
			}
			addList_.Clear ();
		}

		protected virtual void OnEntityRemoved(Entity entity)
		{
		}

		protected virtual void UpdateLogic()
		{
			int count = entities_.Count;
			for (int i = 0; i < count; ++i)
			{
				var entity = entities_[i];
				entity.Update();

				if (entity.NeedToRemove())
				{
					Remove(entity);
				}
			}
		}

		protected virtual void Prepare()
		{
			land_ = new Land.LandTilling ();
			land_.Init (landRoot_, gameCamera);
			land_.Prepare ();
		}

		public override void OnStart ()
		{
			base.OnStart ();

			Prepare ();

			started_ = true;
		}

		public override void OnFinish ()
		{
			base.OnFinish ();

			started_ = false;
		}

		public void Add(Units.Entity entity)
		{
			entity.Game = this;

			addList_.Add (entity);
		}

		public void Remove(Units.Entity entity)
		{
			removeList_.Add (entity);
			entity.OnRemoveFromGame ();
		}

		public float Distance(Entity obj1, Entity obj2)
		{
			return Mathf.Abs (obj1.X - obj2.X);
		}

		public List<Unit> GetUnitsInRadius(float radius, Entity finder)
		{
			unitCache_.Clear ();
			int count = entities_.Count;
			for (int i = 0; i < count; ++i) 
			{
				var entity = entities_[i];
				var unit = (entity as Unit);
				if (entity == finder || 
				    unit == null || 
				    Distance(unit, finder)>radius) continue;
				unitCache_.Add(unit);
			}
			return unitCache_;
		}

		#region ITargetProvider implementation

		private int targetProviderIndex_ = 0;
		void ITargetProvider.InitTargets ()
		{
			targetProviderIndex_ = 0;
		}

		bool ITargetProvider.NextTarget (out Entity target)
		{
			target = null;
			if (targetProviderIndex_ < entities_.Count) 
			{
				target = entities_[targetProviderIndex_++];
			}
			return target != null;
		}

		#endregion
    }
}