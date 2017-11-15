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
 UpdateAddressMessage = 5,
 UpdateCreditsMessage = 6,
 UpdateReputationMessage = 7,
 UpdateKnownAddressMessage = 8,
 NodeConnectMessage = 9,
 NodeConnectResultMessage = 10,
};

public enum CreateAccountResult : sbyte
{
 Success = 0,
 AlreadyExists = 1,
};

public enum LoginResult : sbyte
{
 Success = 0,
 Failed = 1,
 Banned = 2,
};

public enum NodeType : sbyte
{
 Invalid = -1,
 Gateway = 0,
 Server = 1,
 Terminal = 2,
 Mainframe = 3,
 Blackmarket = 4,
 Decoy = 5,
 Home = 6,
};

public enum NodeConnectResult : sbyte
{
 Success = 0,
 Timeout = 1,
 ConnectionRejected = 2,
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

  public LoginResult Result { get { int o = __p.__offset(4); return o != 0 ? (LoginResult)__p.bb.GetSbyte(o + __p.bb_pos) : LoginResult.Success; } }

  public static Offset<LoginResultMessage> CreateLoginResultMessage(FlatBufferBuilder builder,
      LoginResult result = LoginResult.Success) {
    builder.StartObject(1);
    LoginResultMessage.AddResult(builder, result);
    return LoginResultMessage.EndLoginResultMessage(builder);
  }

  public static void StartLoginResultMessage(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddResult(FlatBufferBuilder builder, LoginResult result) { builder.AddSbyte(0, (sbyte)result, 0); }
  public static Offset<LoginResultMessage> EndLoginResultMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<LoginResultMessage>(o);
  }
};

public struct UpdateAddressMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static UpdateAddressMessage GetRootAsUpdateAddressMessage(ByteBuffer _bb) { return GetRootAsUpdateAddressMessage(_bb, new UpdateAddressMessage()); }
  public static UpdateAddressMessage GetRootAsUpdateAddressMessage(ByteBuffer _bb, UpdateAddressMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public UpdateAddressMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Address { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetAddressBytes() { return __p.__vector_as_arraysegment(4); }
  public string Hostname { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetHostnameBytes() { return __p.__vector_as_arraysegment(6); }

  public static Offset<UpdateAddressMessage> CreateUpdateAddressMessage(FlatBufferBuilder builder,
      StringOffset addressOffset = default(StringOffset),
      StringOffset hostnameOffset = default(StringOffset)) {
    builder.StartObject(2);
    UpdateAddressMessage.AddHostname(builder, hostnameOffset);
    UpdateAddressMessage.AddAddress(builder, addressOffset);
    return UpdateAddressMessage.EndUpdateAddressMessage(builder);
  }

  public static void StartUpdateAddressMessage(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddAddress(FlatBufferBuilder builder, StringOffset addressOffset) { builder.AddOffset(0, addressOffset.Value, 0); }
  public static void AddHostname(FlatBufferBuilder builder, StringOffset hostnameOffset) { builder.AddOffset(1, hostnameOffset.Value, 0); }
  public static Offset<UpdateAddressMessage> EndUpdateAddressMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<UpdateAddressMessage>(o);
  }
};

public struct UpdateCreditsMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static UpdateCreditsMessage GetRootAsUpdateCreditsMessage(ByteBuffer _bb) { return GetRootAsUpdateCreditsMessage(_bb, new UpdateCreditsMessage()); }
  public static UpdateCreditsMessage GetRootAsUpdateCreditsMessage(ByteBuffer _bb, UpdateCreditsMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public UpdateCreditsMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public long Credits { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }

  public static Offset<UpdateCreditsMessage> CreateUpdateCreditsMessage(FlatBufferBuilder builder,
      long credits = 0) {
    builder.StartObject(1);
    UpdateCreditsMessage.AddCredits(builder, credits);
    return UpdateCreditsMessage.EndUpdateCreditsMessage(builder);
  }

  public static void StartUpdateCreditsMessage(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddCredits(FlatBufferBuilder builder, long credits) { builder.AddLong(0, credits, 0); }
  public static Offset<UpdateCreditsMessage> EndUpdateCreditsMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<UpdateCreditsMessage>(o);
  }
};

public struct UpdateReputationMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static UpdateReputationMessage GetRootAsUpdateReputationMessage(ByteBuffer _bb) { return GetRootAsUpdateReputationMessage(_bb, new UpdateReputationMessage()); }
  public static UpdateReputationMessage GetRootAsUpdateReputationMessage(ByteBuffer _bb, UpdateReputationMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public UpdateReputationMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Reputation { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<UpdateReputationMessage> CreateUpdateReputationMessage(FlatBufferBuilder builder,
      int reputation = 0) {
    builder.StartObject(1);
    UpdateReputationMessage.AddReputation(builder, reputation);
    return UpdateReputationMessage.EndUpdateReputationMessage(builder);
  }

  public static void StartUpdateReputationMessage(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddReputation(FlatBufferBuilder builder, int reputation) { builder.AddInt(0, reputation, 0); }
  public static Offset<UpdateReputationMessage> EndUpdateReputationMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<UpdateReputationMessage>(o);
  }
};

public struct UpdateKnownAddressMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static UpdateKnownAddressMessage GetRootAsUpdateKnownAddressMessage(ByteBuffer _bb) { return GetRootAsUpdateKnownAddressMessage(_bb, new UpdateKnownAddressMessage()); }
  public static UpdateKnownAddressMessage GetRootAsUpdateKnownAddressMessage(ByteBuffer _bb, UpdateKnownAddressMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public UpdateKnownAddressMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Address { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetAddressBytes() { return __p.__vector_as_arraysegment(4); }
  public string Hostname { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetHostnameBytes() { return __p.__vector_as_arraysegment(6); }
  public NodeType Type { get { int o = __p.__offset(8); return o != 0 ? (NodeType)__p.bb.GetSbyte(o + __p.bb_pos) : NodeType.Gateway; } }

  public static Offset<UpdateKnownAddressMessage> CreateUpdateKnownAddressMessage(FlatBufferBuilder builder,
      StringOffset addressOffset = default(StringOffset),
      StringOffset hostnameOffset = default(StringOffset),
      NodeType type = NodeType.Gateway) {
    builder.StartObject(3);
    UpdateKnownAddressMessage.AddHostname(builder, hostnameOffset);
    UpdateKnownAddressMessage.AddAddress(builder, addressOffset);
    UpdateKnownAddressMessage.AddType(builder, type);
    return UpdateKnownAddressMessage.EndUpdateKnownAddressMessage(builder);
  }

  public static void StartUpdateKnownAddressMessage(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddAddress(FlatBufferBuilder builder, StringOffset addressOffset) { builder.AddOffset(0, addressOffset.Value, 0); }
  public static void AddHostname(FlatBufferBuilder builder, StringOffset hostnameOffset) { builder.AddOffset(1, hostnameOffset.Value, 0); }
  public static void AddType(FlatBufferBuilder builder, NodeType type) { builder.AddSbyte(2, (sbyte)type, 0); }
  public static Offset<UpdateKnownAddressMessage> EndUpdateKnownAddressMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<UpdateKnownAddressMessage>(o);
  }
};

public struct NodeConnectMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static NodeConnectMessage GetRootAsNodeConnectMessage(ByteBuffer _bb) { return GetRootAsNodeConnectMessage(_bb, new NodeConnectMessage()); }
  public static NodeConnectMessage GetRootAsNodeConnectMessage(ByteBuffer _bb, NodeConnectMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public NodeConnectMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Route(int j) { int o = __p.__offset(4); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int RouteLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<NodeConnectMessage> CreateNodeConnectMessage(FlatBufferBuilder builder,
      VectorOffset routeOffset = default(VectorOffset)) {
    builder.StartObject(1);
    NodeConnectMessage.AddRoute(builder, routeOffset);
    return NodeConnectMessage.EndNodeConnectMessage(builder);
  }

  public static void StartNodeConnectMessage(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddRoute(FlatBufferBuilder builder, VectorOffset routeOffset) { builder.AddOffset(0, routeOffset.Value, 0); }
  public static VectorOffset CreateRouteVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartRouteVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<NodeConnectMessage> EndNodeConnectMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<NodeConnectMessage>(o);
  }
};

public struct NodeConnectResultMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static NodeConnectResultMessage GetRootAsNodeConnectResultMessage(ByteBuffer _bb) { return GetRootAsNodeConnectResultMessage(_bb, new NodeConnectResultMessage()); }
  public static NodeConnectResultMessage GetRootAsNodeConnectResultMessage(ByteBuffer _bb, NodeConnectResultMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public NodeConnectResultMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public NodeConnectResult Success { get { int o = __p.__offset(4); return o != 0 ? (NodeConnectResult)__p.bb.GetSbyte(o + __p.bb_pos) : NodeConnectResult.Success; } }
  public NodeType Type { get { int o = __p.__offset(6); return o != 0 ? (NodeType)__p.bb.GetSbyte(o + __p.bb_pos) : NodeType.Gateway; } }

  public static Offset<NodeConnectResultMessage> CreateNodeConnectResultMessage(FlatBufferBuilder builder,
      NodeConnectResult success = NodeConnectResult.Success,
      NodeType type = NodeType.Gateway) {
    builder.StartObject(2);
    NodeConnectResultMessage.AddType(builder, type);
    NodeConnectResultMessage.AddSuccess(builder, success);
    return NodeConnectResultMessage.EndNodeConnectResultMessage(builder);
  }

  public static void StartNodeConnectResultMessage(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddSuccess(FlatBufferBuilder builder, NodeConnectResult success) { builder.AddSbyte(0, (sbyte)success, 0); }
  public static void AddType(FlatBufferBuilder builder, NodeType type) { builder.AddSbyte(1, (sbyte)type, 0); }
  public static Offset<NodeConnectResultMessage> EndNodeConnectResultMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<NodeConnectResultMessage>(o);
  }
};


}
