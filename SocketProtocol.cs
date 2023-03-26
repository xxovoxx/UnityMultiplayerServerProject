// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: SocketProtocol.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace SocketProtocol
{

    [global::ProtoBuf.ProtoContract()]
    public partial class MainPack : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public RequestCode requestCode { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public ActionCode actionCode { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public ReturnCode returnCode { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public enum RequestCode
    {
        RequestNone = 0,
        User = 1,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum ActionCode
    {
        ActionNone = 0,
        Register = 1,
        Login = 2,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum ReturnCode
    {
        ReturnNone = 0,
        Succeed = 1,
        Failed = 2,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion