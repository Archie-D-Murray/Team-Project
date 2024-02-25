namespace Data {
    public interface ISerialize {
        void OnSerialize(ref GameData data);
        void OnDeserialize(GameData data);
    }
}