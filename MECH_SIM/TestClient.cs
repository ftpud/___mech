using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mech.Data;
using Mech.MClient;
using Newtonsoft.Json;
using MathHelper;

namespace Mech.MClient
{
    /// <summary>
    /// AI sample class
    /// </summary>
    public class TestClient : Client
    {
        public TestClient()
        {
            isConnected = true;
        }

        public override void SendData()
        {
            AnswerData data = JsonConvert.DeserializeObject<AnswerData>(ansData);
            Console.WriteLine(data.Tick);

            long curTick = data.Tick;

            float X = data.Settings.First().X;
            float Y = data.Settings.First().Y;

            Console.WriteLine("Pos: " + X.ToString() + " " + Y.ToString());

            cmds = new List<Cmd>();

            if (curTick == 3)
            {
                cmds.Add(new Cmd() { cmd = Command.CreateBody, id = 0, data = 50 });
            }

            if (curTick < 250)
            {
                cmds.Add(new Cmd() { cmd = Command.Force, id = 1, data = 1, data2 = 0 });

                cmds.Add(new Cmd() { cmd = Command.Force, id = 0, data = 1, data2 = 0 });
                cmds.Add(new Cmd() { cmd = Command.Rotate, id = 0, data = 0.1f, data2 = 0 });
            }

            base.SendData();
        }

        public override Body Init()
        {
            return new Body()
            {
                Angle = 0,
                Controllable = true,
                Energy = 1000,
                Forces = new Vector2(0, 0),
                Fraction = 0,
                Hp = 100,
                ID = 0,
                Parent = this,
                Position = new Vector2(100, 100),
                Radius = 7 
            };
        }

        List<Cmd> cmds;
        public override string WaitForAnswer()
        {
            return JsonConvert.SerializeObject(cmds);
        }
    }

        
}
