namespace ResourceCollectorXNA.Engine.Logic {
    public class IdGenerator {
        private uint _lastValue;
        public IdGenerator(uint lastEnd)
        {
            _lastValue = lastEnd;
        }
        public uint NewId()
        {
            return _lastValue++;
        }

        public void ClearIdsCounter() {
            _lastValue = 0;
        }
    }

    public class ObjectEditorType {
        public const uint SolidObject = 0;
        public const uint TerrainObject = 10;
        public const uint LightSource = 20;
    };

    public class EditorData {
        public string DescriptionName;
        public uint id;// = IdGenerator.NewId();
        public uint group_id = 0;
        public bool isActive;
        public uint objtype;

        public bool isGroundRayCasted;
        public bool isBulletRayCasted;

        public EditorData(string name,
                          uint type)
        {
            objtype = type;
            DescriptionName = name;
        }
    }
}