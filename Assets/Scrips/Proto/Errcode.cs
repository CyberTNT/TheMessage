// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: errcode.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from errcode.proto</summary>
public static partial class ErrcodeReflection {

  #region Descriptor
  /// <summary>File descriptor for errcode.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static ErrcodeReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "Cg1lcnJjb2RlLnByb3RvIlMKDmVycm9yX2NvZGVfdG9jEhkKBGNvZGUYASAB",
          "KA4yCy5lcnJvcl9jb2RlEhIKCmludF9wYXJhbXMYAiADKAMSEgoKc3RyX3Bh",
          "cmFtcxgDIAMoCSrfAQoKZXJyb3JfY29kZRIcChhjbGllbnRfdmVyc2lvbl9u",
          "b3RfbWF0Y2gQABIQCgxub19tb3JlX3Jvb20QARIVChFyZWNvcmRfbm90X2V4",
          "aXN0cxACEhYKEmxvYWRfcmVjb3JkX2ZhaWxlZBADEhwKGHJlY29yZF92ZXJz",
          "aW9uX25vdF9tYXRjaBAEEhEKDW5hbWVfdG9vX2xvbmcQBRIWChJqb2luX3Jv",
          "b21fdG9vX2Zhc3QQBhIVChFyb2JvdF9ub3RfYWxsb3dlZBAHEhIKDmFscmVh",
          "ZHlfb25saW5lEAhCFgoUY29tLmZlbmdzaGVuZy5wcm90b3NiBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(new[] {typeof(global::error_code), }, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::error_code_toc), global::error_code_toc.Parser, new[]{ "Code", "IntParams", "StrParams" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Enums
public enum error_code {
  /// <summary>
  /// 客户端版本号不匹配，int_params[0]为服务器版本号
  /// </summary>
  [pbr::OriginalName("client_version_not_match")] ClientVersionNotMatch = 0,
  /// <summary>
  /// 没有更多的房间了
  /// </summary>
  [pbr::OriginalName("no_more_room")] NoMoreRoom = 1,
  /// <summary>
  /// 录像不存在
  /// </summary>
  [pbr::OriginalName("record_not_exists")] RecordNotExists = 2,
  /// <summary>
  /// 读取录像失败了
  /// </summary>
  [pbr::OriginalName("load_record_failed")] LoadRecordFailed = 3,
  /// <summary>
  /// 录像的版本号不匹配，，int_params[0]为录像的版本号
  /// </summary>
  [pbr::OriginalName("record_version_not_match")] RecordVersionNotMatch = 4,
  /// <summary>
  /// 玩家名字太长了
  /// </summary>
  [pbr::OriginalName("name_too_long")] NameTooLong = 5,
  /// <summary>
  /// 加入房间的请求太快了
  /// </summary>
  [pbr::OriginalName("join_room_too_fast")] JoinRoomTooFast = 6,
  /// <summary>
  /// 禁止添加机器人
  /// </summary>
  [pbr::OriginalName("robot_not_allowed")] RobotNotAllowed = 7,
  /// <summary>
  /// 你已经在线，不能重复登录
  /// </summary>
  [pbr::OriginalName("already_online")] AlreadyOnline = 8,
}

#endregion

#region Messages
public sealed partial class error_code_toc : pb::IMessage<error_code_toc> {
  private static readonly pb::MessageParser<error_code_toc> _parser = new pb::MessageParser<error_code_toc>(() => new error_code_toc());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<error_code_toc> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::ErrcodeReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public error_code_toc() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public error_code_toc(error_code_toc other) : this() {
    code_ = other.code_;
    intParams_ = other.intParams_.Clone();
    strParams_ = other.strParams_.Clone();
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public error_code_toc Clone() {
    return new error_code_toc(this);
  }

  /// <summary>Field number for the "code" field.</summary>
  public const int CodeFieldNumber = 1;
  private global::error_code code_ = global::error_code.ClientVersionNotMatch;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public global::error_code Code {
    get { return code_; }
    set {
      code_ = value;
    }
  }

  /// <summary>Field number for the "int_params" field.</summary>
  public const int IntParamsFieldNumber = 2;
  private static readonly pb::FieldCodec<long> _repeated_intParams_codec
      = pb::FieldCodec.ForInt64(18);
  private readonly pbc::RepeatedField<long> intParams_ = new pbc::RepeatedField<long>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<long> IntParams {
    get { return intParams_; }
  }

  /// <summary>Field number for the "str_params" field.</summary>
  public const int StrParamsFieldNumber = 3;
  private static readonly pb::FieldCodec<string> _repeated_strParams_codec
      = pb::FieldCodec.ForString(26);
  private readonly pbc::RepeatedField<string> strParams_ = new pbc::RepeatedField<string>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<string> StrParams {
    get { return strParams_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as error_code_toc);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(error_code_toc other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Code != other.Code) return false;
    if(!intParams_.Equals(other.intParams_)) return false;
    if(!strParams_.Equals(other.strParams_)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Code != global::error_code.ClientVersionNotMatch) hash ^= Code.GetHashCode();
    hash ^= intParams_.GetHashCode();
    hash ^= strParams_.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (Code != global::error_code.ClientVersionNotMatch) {
      output.WriteRawTag(8);
      output.WriteEnum((int) Code);
    }
    intParams_.WriteTo(output, _repeated_intParams_codec);
    strParams_.WriteTo(output, _repeated_strParams_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Code != global::error_code.ClientVersionNotMatch) {
      size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Code);
    }
    size += intParams_.CalculateSize(_repeated_intParams_codec);
    size += strParams_.CalculateSize(_repeated_strParams_codec);
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(error_code_toc other) {
    if (other == null) {
      return;
    }
    if (other.Code != global::error_code.ClientVersionNotMatch) {
      Code = other.Code;
    }
    intParams_.Add(other.intParams_);
    strParams_.Add(other.strParams_);
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Code = (global::error_code) input.ReadEnum();
          break;
        }
        case 18:
        case 16: {
          intParams_.AddEntriesFrom(input, _repeated_intParams_codec);
          break;
        }
        case 26: {
          strParams_.AddEntriesFrom(input, _repeated_strParams_codec);
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code
