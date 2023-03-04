using Dapper;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace FSControl.Controllers
{
    [EnableCors("Policy")]
    [Route("api")]
    [ApiController]
    [BasicAuthorization]
    public  class FSController : ControllerBase
    {
        public const string STAGE_IP = "192.168.3.16";
        public const string WALL_IP = "192.168.3.14";

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
            try
            {
                switch (_action)
                {
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
                    case "stageallwhite":
                        Program.frm?.StageWhite();
                        ret = new { message = "Stage White sent" };
                        break;
                    case "testconnection":
                        bool working = Program.frm!.TestConnection(WALL_IP);
                        bool working2 = Program.frm!.TestConnection(STAGE_IP);
                        if (working && working2)
                            ret = new { message = "Both Freestylers are online" };
                        else
                        {
                            if (!working && !working2)
                                ret = new { message = "Both Freestylers are offline :(" };
                            else if (!working)
                                ret = new { message = "Wall Freestyler is offline" };
                            else if (!working2)
                                ret = new { message = "Stage Freestyler is offline" };
                        }
                        break;
                    case "actions":
                        List<string> combo = new List<string>();
                        foreach (var prop in typeof(Commands).GetFields())
                        {
                            if (prop.FieldType.BaseType!.FullName == "System.Array")
                                combo.Add(prop.Name);
                        }
                        ret = new
                        {
                            actions = new Dictionary<string, object>()
                            {
                                {
                                    "powerallon", "Power All On"
                                },
                                {
                                    "poweralloff", "Power All Off"
                                },
                                {
                                    "stageallwhite", "Stage White"
                                },
                                {
                                    "sundaylights", "Sunday Lights"
                                },
                                {
                                    "combo", combo
                                }
                            }
                        };
                        break;
                    case "history":
                        StreamReader r = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FSControl\\log.log");
                        List<string> lst = r.ReadToEnd().Split(Environment.NewLine).ToList();
                        r.Close();
                        ret = lst.TakeLast(50).ToList();
                        break;
                }
                if (_action.StartsWith("combo"))
                {
                    string val = _action.Replace("combo", "");
                    object? cmd = typeof(Commands).GetField("LIGHTS_" + val)?.GetValue(this);
                    Program.frm?.ChangeCombo((string[])cmd!);
                }
            }
            catch (Exception ex)
            {
                ret = new { message = "Error running command. Most likely because FreeStyler isn't responding. Please re/start FreeStyler", ex = ex.Message };
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

        [HttpGet("icon/{id}")]
        public ActionResult Get(string id)
        {
            if (id == "favicon.ico")
            {
                return new FileStreamResult(new FileStream("spotlight.ico", FileMode.Open), "image/x-icon");
            }
            else
                return Forbid();
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

            using (MySqlConnection db = secrets.GetConnectionString())
            {
                string hash = Argon2.Hash(password);
                int r = db.Execute("INSERT INTO users VALUES(NULL, @user, @pass);", new DynamicParameters(new { user = username, pass = hash }));
                if (r == 1)
                    ret = new { message = "success" };
                else
                    ret = new { message = "fail" };
            }

            return Ok(ret);
        }
    }
}
