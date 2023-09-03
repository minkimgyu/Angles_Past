using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class StringBaseBuffSODictionary : SerializableDictionary<string, BaseBuffSO> { }

[Serializable]
public class StringBaseSkillSODictionary : SerializableDictionary<string, BaseSkillSO> { }

[Serializable]
public class StringBaseEntitySODictionary : SerializableDictionary<string, BaseEntitySO> { }

[Serializable]
public class StringBasicProjectileSODictionary : SerializableDictionary<string, BasicProjectileSO> { }

[Serializable]
public class EffectConditionEffectDataDictionary : SerializableDictionary<EffectCondition, EffectData> { }

[Serializable]
public class EffectConditionSoundDataDictionary : SerializableDictionary<EffectCondition, SoundData> { }



[Serializable]
public class StringFollowEnemyStatDictionary : SerializableDictionary<string, FollowEnemyStat> { }

[Serializable]
public class StringReflectEnemyStatDictionary : SerializableDictionary<string, ReflectEnemyStat> { }

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}