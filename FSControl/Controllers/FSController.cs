using Dapper;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
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
        public ContentResult Get()
        {
            //return Ok(new { message = "Hello", actions = new string[] { "toggleall", "powerallon", "poweralloff", "sundaylights" } });
            var html = System.IO.File.ReadAllText(@"api.html");
            return base.Content(html, "text/html");
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
                case "history":
                    StreamReader r = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FSControl\\log.log");
                    List<string> lst = r.ReadToEnd().Split(Environment.NewLine).ToList();
                    r.Close();
                    ret = lst.TakeLast(50).ToList();
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

    [EnableCors("Policy")]
    [Route("register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpGet]
        public ContentResult Get()
        {
            var html = System.IO.File.ReadAllText(@"register.html");
            return base.Content(html, "text/html");
        }

        [HttpPost]
        public ActionResult Post([FromForm] string username, [FromForm] string password)
        {
            Object? ret = null;

            //using (MySqlConnection db = secrets.GetConnectionString())
            //{
            //    string hash = Argon2.Hash(password);
            //    int r = db.Execute("INSERT INTO users VALUES(NULL, @user, @pass);", new DynamicParameters(new { user = username, pass = hash }));
            //    if (r == 1)
            //        ret = new { message = "success" };
            //    else
            //        ret = new { message = "fail" };
            //}
            ret = new { message = "success" };

            return Ok(ret);
        }
    }
}
