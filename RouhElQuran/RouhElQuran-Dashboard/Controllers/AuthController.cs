﻿using Microsoft.AspNetCore.Mvc;

namespace RouhElQuran_Dashboard.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult ForgotPasswordBasic() => View();
        public IActionResult LoginBasic() => View();
        public IActionResult RegisterBasic() => View();
    }
}
