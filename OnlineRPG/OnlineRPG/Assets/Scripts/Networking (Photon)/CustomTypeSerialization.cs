using UnityEngine;
using ExitGames.Client.Photon;

public static class CustomTypeSerialization
{
    public static void Register()
    {
        //PhotonPeer.RegisterType(typeof(Item), (byte)'I', SerializeItem, DeserializeItem);
    }

    #region De/Serialize Methods
    //public static readonly byte[] memItem = new byte[];
    //private static short SerializeItem(StreamBuffer outStream, object customobject)
    //{
    //    Item item = (Item)customobject;

    //    int index = 0;
    //    lock (memItem)
    //    {
    //        byte[] bytes = memItem;
    //        Protocol.Serialize(item.Id, bytes, ref index);
    //        Protocol.Serialize(item.Name, bytes, ref index);
    //        Protocol.Serialize(, bytes, ref index);
    //        outStream.Write(bytes, 0, 3 * 4);
    //    }

    //    return 3 * 4;
    //}

    //private static object DeserializeVector3(StreamBuffer inStream, short length)
    //{
    //    Item item = new Item();
    //    lock (memItem)
    //    {
    //        inStream.Read(memItem, 0, 3 * 4);
    //        int index = 0;
    //        Protocol.Deserialize(out item.Id, memItem, ref index);
    //        Protocol.Deserialize(out vo.y, memVector3, ref index);
    //        Protocol.Deserialize(out vo.z, memVector3, ref index);
    //    }

    //    return item;
    //}
    #endregion

    const int INTEGER_BYTE = 1; // Unsigned (0 to 255)
    const int INTEGER_SBYTE = 2; // Signed (-128 to 127)
    const int INTEGER_SHORT = 2; // Signed (-32,768 to 32,767)
    const int INTEGER_USHORT = 2; // Unsigned (0 to 65,535)
    const int INTEGER_INT = 4; // Singed (-2,147,483,648 to 2,147,483,647)
    const int INTEGER_UINT = 4; // Unsigned (0 to 4,294,967,295)
    const int INTEGER_LONG = 8; //  Signed (-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807)
    const int INTEGER_ULONG = 8; // Unsinged (0 to 18,446,744,073,709,551,615)
    const int FLOAT_FLOAT = 4; // ±1.5e−45 to ±3.4e38  (Precision:7 digits)
    const int FLOAT_DOUBLE = 8; // ±5.0e−324 to ±1.7e308 (Precision:15-16 digits)
    const int FLOAT_DECIMAL = 16; // (-7.9 x 1028 to 7.9 x 1028) / (100 to 28) (Precision:28-29 digits)
    const int CHARACTER_CHAR = 2;
    const int OTHER_DATETIME = 8;
    const int OTHER_BOOL = 1;
}