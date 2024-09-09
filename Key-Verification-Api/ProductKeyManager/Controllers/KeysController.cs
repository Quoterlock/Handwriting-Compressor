using Microsoft.AspNetCore.Mvc;
using ProductKeyManagerApp.Modules.Interfaces;

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
            return Ok(new { result = _keyManager.VerifyKey(key)});
        }

        [HttpGet("generate")]
        public IActionResult GenerateKey(string paycheckId)
        {
            return Ok(new { key = _keyManager.GenerateKey()});
        }
    }
}
