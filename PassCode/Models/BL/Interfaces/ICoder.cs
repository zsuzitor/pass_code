

namespace PassCode.Models.BL.Interfaces
{
    public interface ICoder
    {
        byte[] EncryptWithByte(string dataForEncrypt, string key);
        string DecryptFromBytes(byte[] dataForDecrypt, string key);

        string BytesToCustomString(byte[] bytes);
        byte[] CustomStringToBytes(string str);
    }
}
