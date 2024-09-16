using Microsoft.AspNetCore.Mvc;
using ProductKeyManagerApp.Modules.Interfaces;

namespace ProductKeyManagerApp.Controllers
{
    public class ProductKeysController : ControllerBase
    {
        private readonly IProductKeyManager _keyManager;
        public ProductKeysController(IProductKeyManager keyManager) 
        {
            _keyManager = keyManager;
        }

        [HttpPost("verify")]
        public IActionResult VerifyUserKey([FromBody] UserKey key)
        {
            try
            {
                return Ok(new { result = _keyManager.VerifyKey(key.Value) });
            } 
            catch(Exception)
            {
                return Ok(new { result = false });
            }
        }

        /// <summary>
        /// Backdoor entrypoint to generate new key
        /// </summary>
        /// <returns></returns>
        [HttpGet("generate")]
        public IActionResult GenerateKey()
        {
            return Ok(new { key = _keyManager.GenerateKey() });
        }
    }

    public class UserKey
    {
        public string Value { get; set; }
    }
}
