using Relay.GameServer.Core.Contracts;
using Riptide;
using Riptide.Utils;
using System;
using System.Collections.Generic;

public static class MessageSerializer
{
  private static Dictionary<Type, Action<Message, object>> typeToMessage = new Dictionary<Type, Action<Message, object>>()
  {
    { typeof(uint), (m, v) => m.AddUInt((uint)v) },
    { typeof(int), (m, v) => m.AddInt((int)v) },
    { typeof(bool), (m, v) => m.AddBool((bool)v)},
    { typeof(byte), (m, v) => m.AddByte((byte)v) },
    { typeof(sbyte), (m, v) => m.AddSByte((sbyte)v) },
    { typeof(ushort), (m, v) => m.AddUShort((ushort)v) },
    { typeof(short), (m, v) => m.AddShort((short)v) },
    { typeof(float), (m, v) => m.AddFloat((float)v)},
    { typeof(double), (m, v) => m.AddDouble((double)v) },
    { typeof(long), (m, v) => m.AddLong((long)v) },
    { typeof(ulong), (m, v) => m.AddULong((ulong)v) },
    { typeof(string), (m, v) => m.AddString((string)v) },
    { typeof(Vector2), (m, v) => AppendVector2ToMessage(m, (Vector2)v) },
    { typeof(Vector3), (m, v) => AppendVector3ToMessage(m, (Vector3)v) },
    { typeof(Vector4), (m, v) => AppendVector4ToMessage(m, (Vector4)v) },
    { typeof(Quaternion), (m, v) => AppendQuaternionToMessage(m, (Quaternion)v) },
  };

  private static Dictionary<Type, Func<Message, object>> messageToType = new Dictionary<Type, Func<Message, object>>()
  {
    { typeof(uint), (m) => m.GetUInt() },
    { typeof(int), (m) => m.GetInt() },
    { typeof(bool), (m) => m.GetBool()},
    { typeof(byte), (m) => m.GetByte() },
    { typeof(sbyte), (m) => m.GetSByte() },
    { typeof(ushort), (m) => m.GetUShort() },
    { typeof(short), (m) => m.GetShort() },
    { typeof(float), (m) => m.GetFloat() },
    { typeof(double), (m) => m.GetDouble() },
    { typeof(long), (m) => m.GetLong() },
    { typeof(ulong), (m) => m.GetULong() },
    { typeof(string), (m) => m.GetString() },
    { typeof(Vector2), (m) => DeserializeVector2(m) },
    { typeof(Vector3), (m) => DeserializeVector3(m) },
    { typeof(Vector4), (m) => DeserializeVector4(m) },
    { typeof(Quaternion), (m) => DeserializeQuaternion(m) },
  };

  private static Dictionary<Type, byte> typeToId = new Dictionary<Type, byte>()
  {
    { typeof(uint), 1 },
    { typeof(int), 2 },
    { typeof(bool), 3 },
    { typeof(byte), 4 },
    { typeof(sbyte), 5 },
    { typeof(short), 6 },
    { typeof(ushort), 16 },
    { typeof(float), 7 },
    { typeof(double), 8 },
    { typeof(long), 9 },
    { typeof(ulong), 10 },
    { typeof(string), 11 },
    { typeof(Vector2), 12 },
    { typeof(Vector3), 13 },
    { typeof(Vector4), 14 },
    { typeof(Quaternion), 15 },
  };

  private static Dictionary<byte, Type> idToType = new Dictionary<byte, Type>()
  {
    { 1, typeof(uint) },
    { 2, typeof(int) },
    { 3, typeof(bool) },
    { 4, typeof(byte) },
    { 5, typeof(sbyte) },
    { 6, typeof(short) },
    { 16, typeof(ushort) },
    { 7, typeof(float) },
    { 8, typeof(double) },
    { 9, typeof(long) },
    { 10, typeof(ulong) },
    { 11, typeof(string) },
    { 12, typeof(Vector2) },
    { 13, typeof(Vector3) },
    { 14, typeof(Vector4) },
    { 15, typeof(Quaternion) },
  };

  public static object ReadObjectFromMessage(Message message)
  {
    var typeId = message.GetByte();

    if (!idToType.ContainsKey(typeId))
    {
      RiptideLogger.Log(LogType.Error, $"Type id {typeId} is not in the idToType dictionary!");
    }

    var type = idToType[typeId];

    return messageToType[type](message);
  }

  public static Message WriteObjectToMessage(Message message, object value)
  {
    var typeId = typeToId[value.GetType()];

    if (!typeToId.ContainsKey(value.GetType()))
    {
      RiptideLogger.Log(LogType.Error, $"Type id {typeId} is not in the typeToId dictionary!");
    }

    message.AddByte(typeId);

    typeToMessage[value.GetType()](message, value);
    return message;
  }

  public static Message WriteParametersToMessage(Message message, object[] parameters)
  {
    message.AddInt(parameters.Length);
    for (int i = 0; i < parameters.Length; i++)
    {
      if (!typeToMessage.ContainsKey(parameters[i].GetType()))
      {
        throw new Exception($"RPC and State updates only support base types, no serialization handler defined for {parameters[i].GetType().Name}");
      }
      message.AddByte(typeToId[parameters[i].GetType()]);
      typeToMessage[parameters[i].GetType()](message, parameters[i]);
    }
    return message;
  }

  public static object[] ReadParametersFromMessage(Message message)
  {
    var parameterCount = message.GetInt();
    if (parameterCount == 0)
    {
      return null;
    }

    var o = new List<object>();

    for (int i = 0; i < parameterCount; i++)
    {
      var typeId = message.GetByte();
      o.Add(messageToType[idToType[typeId]](message));
    }
    return o.ToArray();
  }

  static Vector2 DeserializeVector2(Message m)
  {
    var x = m.GetFloat();
    var y = m.GetFloat();
    return new Vector2(x, y);
  }

  static Vector3 DeserializeVector3(Message m)
  {
    var x = m.GetFloat();
    var y = m.GetFloat();
    var z = m.GetFloat();
    return new Vector3(x, y, z);
  }

  static Vector4 DeserializeVector4(Message m)
  {
    var x = m.GetFloat();
    var y = m.GetFloat();
    var z = m.GetFloat();
    var w = m.GetFloat();
    return new Vector4(x, y, z, w);
  }

  static Quaternion DeserializeQuaternion(Message m)
  {
    var x = m.GetFloat();
    var y = m.GetFloat();
    var z = m.GetFloat();
    var w = m.GetFloat();
    return new Quaternion(x, y, z, w);
  }

  static void AppendVector2ToMessage(Message m, Vector2 vector)
  {
    m.AddFloat(vector.x);
    m.AddFloat(vector.y);
  }

  static void AppendVector3ToMessage(Message m, Vector3 vector)
  {
    m.AddFloat(vector.x);
    m.AddFloat(vector.y);
    m.AddFloat(vector.z);
  }

  static void AppendVector4ToMessage(Message m, Vector4 vector)
  {
    m.AddFloat(vector.x);
    m.AddFloat(vector.y);
    m.AddFloat(vector.z);
    m.AddFloat(vector.w);
  }

  static void AppendQuaternionToMessage(Message m, Quaternion quaternion)
  {
    m.AddFloat(quaternion.x);
    m.AddFloat(quaternion.y);
    m.AddFloat(quaternion.z);
    m.AddFloat(quaternion.w);
  }
}
