// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace h4x0r.MessagesInternal
{

using global::System;
using global::FlatBuffers;

public enum MessageContainer : byte
{
 NONE = 0,
 CreateAccountMessage = 1,
 CreateAccountResultMessage = 2,
 LoginMessage = 3,
 LoginResultMessage = 4,
};

public enum CreateAccountResult : sbyte
{
 Success = 0,
 AlreadyExists = 1,
};

public struct MessageBase : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static MessageBase GetRootAsMessageBase(ByteBuffer _bb) { return GetRootAsMessageBase(_bb, new MessageBase()); }
  public static MessageBase GetRootAsMessageBase(ByteBuffer _bb, MessageBase obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public MessageBase __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public MessageContainer DataType { get { int o = __p.__offset(4); return o != 0 ? (MessageContainer)__p.bb.Get(o + __p.bb_pos) : MessageContainer.NONE; } }
  public TTable? Data<TTable>() where TTable : struct, IFlatbufferObject { int o = __p.__offset(6); return o != 0 ? (TTable?)__p.__union<TTable>(o) : null; }

  public static Offset<MessageBase> CreateMessageBase(FlatBufferBuilder builder,
      MessageContainer data_type = MessageContainer.NONE,
      int dataOffset = 0) {
    builder.StartObject(2);
    MessageBase.AddData(builder, dataOffset);
    MessageBase.AddDataType(builder, data_type);
    return MessageBase.EndMessageBase(builder);
  }

  public static void StartMessageBase(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddDataType(FlatBufferBuilder builder, MessageContainer dataType) { builder.AddByte(0, (byte)dataType, 0); }
  public static void AddData(FlatBufferBuilder builder, int dataOffset) { builder.AddOffset(1, dataOffset, 0); }
  public static Offset<MessageBase> EndMessageBase(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<MessageBase>(o);
  }
  public static void FinishMessageBaseBuffer(FlatBufferBuilder builder, Offset<MessageBase> offset) { builder.Finish(offset.Value); }
};

public struct CreateAccountMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static CreateAccountMessage GetRootAsCreateAccountMessage(ByteBuffer _bb) { return GetRootAsCreateAccountMessage(_bb, new CreateAccountMessage()); }
  public static CreateAccountMessage GetRootAsCreateAccountMessage(ByteBuffer _bb, CreateAccountMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public CreateAccountMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Username { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetUsernameBytes() { return __p.__vector_as_arraysegment(4); }
  public string Email { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetEmailBytes() { return __p.__vector_as_arraysegment(6); }
  public string Password { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetPasswordBytes() { return __p.__vector_as_arraysegment(8); }

  public static Offset<CreateAccountMessage> CreateCreateAccountMessage(FlatBufferBuilder builder,
      StringOffset usernameOffset = default(StringOffset),
      StringOffset emailOffset = default(StringOffset),
      StringOffset passwordOffset = default(StringOffset)) {
    builder.StartObject(3);
    CreateAccountMessage.AddPassword(builder, passwordOffset);
    CreateAccountMessage.AddEmail(builder, emailOffset);
    CreateAccountMessage.AddUsername(builder, usernameOffset);
    return CreateAccountMessage.EndCreateAccountMessage(builder);
  }

  public static void StartCreateAccountMessage(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddUsername(FlatBufferBuilder builder, StringOffset usernameOffset) { builder.AddOffset(0, usernameOffset.Value, 0); }
  public static void AddEmail(FlatBufferBuilder builder, StringOffset emailOffset) { builder.AddOffset(1, emailOffset.Value, 0); }
  public static void AddPassword(FlatBufferBuilder builder, StringOffset passwordOffset) { builder.AddOffset(2, passwordOffset.Value, 0); }
  public static Offset<CreateAccountMessage> EndCreateAccountMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<CreateAccountMessage>(o);
  }
};

public struct CreateAccountResultMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static CreateAccountResultMessage GetRootAsCreateAccountResultMessage(ByteBuffer _bb) { return GetRootAsCreateAccountResultMessage(_bb, new CreateAccountResultMessage()); }
  public static CreateAccountResultMessage GetRootAsCreateAccountResultMessage(ByteBuffer _bb, CreateAccountResultMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public CreateAccountResultMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public CreateAccountResult Result { get { int o = __p.__offset(4); return o != 0 ? (CreateAccountResult)__p.bb.GetSbyte(o + __p.bb_pos) : CreateAccountResult.Success; } }

  public static Offset<CreateAccountResultMessage> CreateCreateAccountResultMessage(FlatBufferBuilder builder,
      CreateAccountResult result = CreateAccountResult.Success) {
    builder.StartObject(1);
    CreateAccountResultMessage.AddResult(builder, result);
    return CreateAccountResultMessage.EndCreateAccountResultMessage(builder);
  }

  public static void StartCreateAccountResultMessage(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddResult(FlatBufferBuilder builder, CreateAccountResult result) { builder.AddSbyte(0, (sbyte)result, 0); }
  public static Offset<CreateAccountResultMessage> EndCreateAccountResultMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<CreateAccountResultMessage>(o);
  }
};

public struct LoginMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static LoginMessage GetRootAsLoginMessage(ByteBuffer _bb) { return GetRootAsLoginMessage(_bb, new LoginMessage()); }
  public static LoginMessage GetRootAsLoginMessage(ByteBuffer _bb, LoginMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public LoginMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Username { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetUsernameBytes() { return __p.__vector_as_arraysegment(4); }
  public string Password { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetPasswordBytes() { return __p.__vector_as_arraysegment(6); }

  public static Offset<LoginMessage> CreateLoginMessage(FlatBufferBuilder builder,
      StringOffset usernameOffset = default(StringOffset),
      StringOffset passwordOffset = default(StringOffset)) {
    builder.StartObject(2);
    LoginMessage.AddPassword(builder, passwordOffset);
    LoginMessage.AddUsername(builder, usernameOffset);
    return LoginMessage.EndLoginMessage(builder);
  }

  public static void StartLoginMessage(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddUsername(FlatBufferBuilder builder, StringOffset usernameOffset) { builder.AddOffset(0, usernameOffset.Value, 0); }
  public static void AddPassword(FlatBufferBuilder builder, StringOffset passwordOffset) { builder.AddOffset(1, passwordOffset.Value, 0); }
  public static Offset<LoginMessage> EndLoginMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<LoginMessage>(o);
  }
};

public struct LoginResultMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static LoginResultMessage GetRootAsLoginResultMessage(ByteBuffer _bb) { return GetRootAsLoginResultMessage(_bb, new LoginResultMessage()); }
  public static LoginResultMessage GetRootAsLoginResultMessage(ByteBuffer _bb, LoginResultMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public LoginResultMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public bool Placeholder { get { int o = __p.__offset(4); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }

  public static Offset<LoginResultMessage> CreateLoginResultMessage(FlatBufferBuilder builder,
      bool placeholder = false) {
    builder.StartObject(1);
    LoginResultMessage.AddPlaceholder(builder, placeholder);
    return LoginResultMessage.EndLoginResultMessage(builder);
  }

  public static void StartLoginResultMessage(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddPlaceholder(FlatBufferBuilder builder, bool placeholder) { builder.AddBool(0, placeholder, false); }
  public static Offset<LoginResultMessage> EndLoginResultMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<LoginResultMessage>(o);
  }
};


}
