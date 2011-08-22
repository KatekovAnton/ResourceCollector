namespace ResourceCollectorXNA.Engine.Logic {
    internal static class IdGenerator {
        private static int _lastValue;

        public static int NewId() {
            return _lastValue++;
        }

        public static void ClearIdsCounter() {
            _lastValue = 0;
        }
    }

    public enum ObjectEditorType {
        SolidObject,
        TerrainObject
    };

    public class EditorData {
        public string DescriptionName;
        public int id = IdGenerator.NewId();
        public int group_id = -1;
        public bool isActive;
        public ObjectEditorType objtype;

        public EditorData(string name,
                          ObjectEditorType type) {
            objtype = type;
            DescriptionName = name;
        }
    }
}