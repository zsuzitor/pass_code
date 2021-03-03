

namespace PassCode.Models.BL.Interfaces
{
    public interface ICoder
    {
        byte[] EncryptWithByte(string dataForEncrypt, string key);
        string DecryptFromBytes(byte[] dataForDecrypt, string key);
        string AddRandomizeToString(string str);
        string RemoveRandomizeFromString(string str);

        string BytesToCustomString(byte[] bytes);
        byte[] CustomStringToBytes(string str);
    }
}
