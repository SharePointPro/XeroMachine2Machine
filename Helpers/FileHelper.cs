using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XeroMachine2Machine.Model;

namespace XeroMachine2Machine.Helpers
{
    public class FileHelper
    {
        private const string FILE_NAME = "settings.dat";

        public static void WriteFile(RefreshTokenResponse refreshTokenResponse)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(refreshTokenResponse);
            var encryptedJson = EncryptHelper.Encrypt(json);
            File.WriteAllBytes(FILE_NAME, Encoding.ASCII.GetBytes(encryptedJson));
        }

        public static RefreshTokenResponse ReadFile()
        {
            var fileBytes = File.ReadAllBytes(FILE_NAME);
            var encryptedJson = Encoding.ASCII.GetString(fileBytes);
            var json = EncryptHelper.Decrypt(encryptedJson);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RefreshTokenResponse>(json);
        }
    }
}
