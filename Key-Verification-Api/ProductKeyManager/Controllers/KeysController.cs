using Microsoft.AspNetCore.Mvc;
using ProductKeyManagerApp.Modules.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProductKeyManagerApp.Controllers
{
    public class KeysController : ControllerBase
    {
        private readonly IProductKeyManager _keyManager;
        public KeysController(IProductKeyManager keyManager) 
        {
            _keyManager = keyManager;
        }

        [HttpGet("verify")]
        public IActionResult VerifyUserKey(string key)
        {
            try
            {
                var privateKey = @"BwIAAACkAABSU0EyAAgAAAEAAQD3wSFcDtc+qa/g6zw13aTBeRfTneXq7corMEHAHBg4/nmvf4pMLimDHDs/4YbBFgCPqR6303hgIdRRiUD1Cgf0Kh+1+XqibhUT7qM026wBVWez++06xqKVJb1J1Ma/3U3iPOEZD7jBpHuMiEZdgboMY6gilWZcLynFqXlA1G1z3qiFxySXzUAOGem5aaVZ+6m7Zrqb6VoGBfvtfvemE17TawMeQD/BI+bQrDPXchh7RVsbs9jfKDl0jpWBBzB8hwmKdNmbMrmSr51aZpGR1TuJiLFm9/mHByy2c4q2xTai0eenkR5ZQrEPm13+lyqcrkndtlx0YdF9UP2OrmMxFNCjZ/i7hYYvoa40S5MmxROgC36pJAU/EauxI6f26BKQR+m4LoRTerLIipnK4QVyAAVxTuZ9FqJTkMxlX1uiXKXe48nW7CWlg9T3pqd/Z/kAnmdsci29DIWoiROhkBXwNjZTV2HpI0jyLKGgcR6AKfI08ihgXv36bmPGn8wKhFLtALPxL568Yo3zD4WaVYmekxSN5BpX8uPZCmkHoJsjxFwOUUQ+ctINLTv0QRkHy1bW7mVrNDIZPYpxMltOFg1d6cGJyR+ivQcGgmRHfEwOAvE0pn7CGZsgZ+h5yjips7OLfmSAc+2mAlE0VFBr0991AnPgLnVpV0xKN06A7yDTamhG6vvLCDOcqJUqZgbnWN6XTz6paltZXYZ2YLl/q3ZZcK92Lzr+0B/nDGmzCXZjip/t+EF4+4RjVBCZmydiaBnUL12L3xLKkBTTSLxp4wOPcBX4JYGogrlPpD3Q5zVrXYV8vnVpnsTG43FJjr4OxuvJuEo8fzuVb/Es6XQd6OEFlKlGoaLjkVOE/Qeo/bqaon0vAnO23VtVvgUcABx7/we5GLawGMHtnyjJguBCzMEhwyd4AUZUIHXVthz4xxmHxM3OfbPDmR4FhXnw7CLIXpUNtH5t9oOq1rJfPY1aTn5t0ksCvzlwsV9CBHNGmVfIPBEMBXm6J11LO3baJv+1zbojqm2t6ODGEivDyXaHoHgiZHClqqBJbTCEXlM+CH4r4jxuXT5zJJ7m53ZEZRCyHvD3MtKtDJYXZT8d6gYu5lgcaEKP0DwxAgp4bK5cRePxzlwnjU4PcWxWJAR/yhDSQHV1hdkNbK6rFCjSZo8Ka6ZdTv7kyFi2jJMeUWceKd7DXKtTjyEYsIuxrE0pWphCGABkKZSQR1SD93zzBU3If0Fk9UL91U9bUTbBCLAG8yaxCslMYMg+obt4IYVsT9C7GxcBhkhBo8JHAlmnsM+w58utcsl22KZxZV1Tpx4fDyAP7FWHKE6js8kZd2vzaHDGE8zxSEvGT0eOx8zBnE/ZP5zNqbmyllOUuJjkcmk+RBflbet5fRqmYVkkQShBrj3V7nAnTzgQLYgW+tKyZCow1im8idnYI+CrtuBA9++NNWk5CG3x3z2jOSOQWKDw9/n8rT0Az0+BCjRb398/ghmb9/+EklE9NE6InWCKfgRLSNN8+sx2cqXqRlq39QzjrS8ZCLmVJng=";
                var decryptedKey = DecryptMessageWithPrivateKey(key, privateKey);
                return Ok(new { result = _keyManager.VerifyKey(decryptedKey) });
            } 
            catch(Exception)
            {
                return Ok(new { result = false });
            }
        }

        [HttpGet("generate")]
        public IActionResult GenerateKey(string paycheckId)
        {
            return Ok(new { key = _keyManager.GenerateKey()});
        }

        [HttpGet("get-public-key")]
        public IActionResult GetPublicKey()
        {
            var publicKey = @"BgIAAACkAABSU0ExAAgAAAEAAQD3wSFcDtc+qa/g6zw13aTBeRfTneXq7corMEHAHBg4/nmvf4pMLimDHDs/4YbBFgCPqR6303hgIdRRiUD1Cgf0Kh+1+XqibhUT7qM026wBVWez++06xqKVJb1J1Ma/3U3iPOEZD7jBpHuMiEZdgboMY6gilWZcLynFqXlA1G1z3qiFxySXzUAOGem5aaVZ+6m7Zrqb6VoGBfvtfvemE17TawMeQD/BI+bQrDPXchh7RVsbs9jfKDl0jpWBBzB8hwmKdNmbMrmSr51aZpGR1TuJiLFm9/mHByy2c4q2xTai0eenkR5ZQrEPm13+lyqcrkndtlx0YdF9UP2OrmMxFNCj";
            return Ok(new { public_key = publicKey });
        }

        private static string DecryptMessageWithPrivateKey(string base64EncryptedMessage, string privateKey)
        {
            byte[] encryptedMessageBytes = Convert.FromBase64String(base64EncryptedMessage);
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // Import the RSA private key
                    rsa.ImportCspBlob(privateKeyBytes);

                    // Decrypt the message
                    byte[] decryptedBytes = rsa.Decrypt(encryptedMessageBytes, false);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during decryption: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
