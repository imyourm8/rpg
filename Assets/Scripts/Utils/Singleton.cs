namespace LootQuest.Utils
{
    public class Singleton<T> where T : class, new()
    {
        class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T instance_ = new T();
        }

        protected static T instance_;

        public static T Instance
        {
            get { return SingletonCreator.instance_; }
			protected set { instance_ = value; }
        }
    }

    public class SingletonOnDemand<T> where T : class, new()
    {
        protected static T instance_;

        public static T Instance
        {
            get { return instance_; }
            protected set { instance_ = value; }
        }
    }
}
