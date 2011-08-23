﻿namespace ResourceCollectorXNA.Engine.Logic {
    internal static class IdGenerator {
        private static uint _lastValue;

        public static uint NewId()
        {
            return _lastValue++;
        }

        public static void ClearIdsCounter() {
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
        public uint id = IdGenerator.NewId();
        public uint group_id = 0;
        public bool isActive;
        public uint objtype;

        public EditorData(string name,
                          uint type)
        {
            objtype = type;
            DescriptionName = name;
        }
    }
}