using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace FSControl.Controllers
{
    [EnableCors("Policy")]
    [Route("api")]
    [ApiController]
    [BasicAuthorization]
    public  class FSController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(new { message = "Hello", actions = new string[] { "toggleall", "powerallon", "poweralloff", "sundaylights" } });
        }

        [HttpGet("{_action}")]
        public ActionResult<string> Get(string _action)
        {
            Object? ret = null;
            switch (_action)
            {
                case "toggleall":
                    Program.frm?.ToggleAll();
                    ret = new { message = "Toggle All sent" };
                    break;
                case "powerallon":
                    Program.frm?.PowerAllOn();
                    ret = new { message = "Power All On sent" };
                    break;
                case "poweralloff":
                    Program.frm?.PowerAllOff();
                    ret = new { message = "Power All Off sent" };
                    break;
                case "sundaylights":
                    Program.frm?.SundayLights();
                    ret = new { message = "Sunday Lights sent" };
                    break;
            }
            return Ok(ret);
        }
    }

    [EnableCors("Policy")]
    [Route("")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Scary music comes from FreeStyler" };
        }
    }
}
