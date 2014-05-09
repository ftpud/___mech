using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mech.MClient;
using Mech.Data;

using MathHelper;
using Newtonsoft.Json;


namespace Mech.Server
{
    /// Server
    public class Server
    {

        public Dictionary<long, List<BodySettings>> History = new Dictionary<long, List<BodySettings>>();

        List<Client> Bots = new List<Client>();
        List<Body> Bodies = new List<Body>();

        long Tick = 0;


        public Server()
        { }

        public void Update()
        {
            // Send Current State
            foreach (Client c in Bots)
            {
                var ClientBodies = Bodies.Where(x => x.Parent.Equals(c));
                AnswerData data = new AnswerData();
                data.Tick = Tick;

                data.Settings = new List<BodySettings>();
                data.EnemySettings = new List<BodySettings>();

                foreach (Body b in ClientBodies)
                {
                    BodySettings Set = new BodySettings()
                    {
                        Angle = b.Angle,
                        Energy = b.Energy,
                        Hp = b.Hp,
                        ID = b.ID,
                        isEnemy = false,
                        Radius = b.Radius,
                        X = b.Position.X,
                        Y = b.Position.Y
                    };
                    data.Settings.Add(Set);

                }

                var enClientBodies = Bodies.Where(x => x.Parent != c);
                foreach (Body b in enClientBodies)
                {
                    BodySettings Set = new BodySettings()
                    {
                        Angle = 0,
                        Energy = 0,
                        Hp = 0,
                        ID = 0,
                        isEnemy = true,
                        Radius = b.Radius,
                        X = b.Position.X,
                        Y = b.Position.Y
                    };
                    data.EnemySettings.Add(Set);
                }

                c.addData(JsonConvert.SerializeObject(data));
                c.SendData();
            }
            // Wait For Answers

            // Parse CMDS
            foreach (Client c in Bots)
            {
                List<Cmd> Cmds = JsonConvert.DeserializeObject< List<Cmd> >(c.WaitForAnswer());
                if (Cmds != null)
                {
                    foreach (Cmd cmd in Cmds)
                    {
                        var body = Bodies.Where(x => x.Parent == c && x.ID == cmd.id);

                        //Body Current = Bodies.First();
                        if (body.Count() == 0) continue;

                        Body Current = body.First();

                        switch (cmd.cmd)
                        {
                            case Command.CreateBody:

                                float NewEnergy = cmd.data;
                                if(NewEnergy <= 10) NewEnergy = 10;

                                Current.Energy -= NewEnergy;
                                float NewRadius = 5; // NewEnergy / 10;

                                Body newBody = new Body()
                                {
                                    Angle = Current.Angle,
                                    Controllable = true,
                                    Energy = NewEnergy,
                                    Forces = new Vector2(0, 0),
                                    Fraction = Current.Fraction,
                                    Hp = 1,
                                    ID = c.bodyIDIncrementor + 1,
                                    Parent = c,
                                    Position = Current.Position,
                                    Radius = NewRadius
                                };

                                Bodies.Add(newBody);

                                c.bodyIDIncrementor++;

                                break;

                            case Command.Rotate:
                                Current.Angle += cmd.data;
                                Current.Energy -= cmd.data;
                                break;

                            case Command.Force:
                                if (cmd.data > 1) cmd.data = 1;
                                if (cmd.data2 > 1) cmd.data2 = 1;

                                Current.Energy -= cmd.data;
                                Current.Energy -= cmd.data2;

                                Vector2 Fwd =  Vector2.GetForward(Current.Angle);
                                Vector2 Side =  Vector2.GetForward(Current.Angle + (float)Math.PI / 2);


                                Current.Forces =  (Fwd * cmd.data) + (Side * cmd.data2) ;
                                if (Current.Forces.X > 1) Current.Forces.X = 1;
                                if (Current.Forces.Y > 1) Current.Forces.Y = 1;
                                if (Current.Forces.X < -1) Current.Forces.X = -1;
                                if (Current.Forces.Y < -1) Current.Forces.Y = -1;


                                break;
                        }
                    }
                }
            }
            // Do physics updates

            foreach (Body b in Bodies)
            {
                b.Position += b.Forces * 2f;
                b.Forces -= b.Forces * 0.1f;
            }

            // Save visualization data

            List<BodySettings> Hist = new List<BodySettings>();
            foreach (Body b in Bodies)
            {
                Hist.Add(new BodySettings()
                {
                    X = b.Position.X,
                    Y = b.Position.Y,
                    Angle = b.Angle,
                    Radius = b.Radius,
                    Hp = b.Hp,
                    Energy = b.Energy,
                    Fraction = b.Fraction,
                });
            }

            History.Add(Tick, Hist);

            if (Tick % 100 == 0)
            {
                string save = JsonConvert.SerializeObject(History);
                System.IO.File.WriteAllText("save.js", "$('#data').val('" + save + "');");
            }
        }

        public void Start()
        {
            // Wait for Connections            
            if (!WaitForInitialization()) return;

            // Start Loop            
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Tick: " + Tick);
                Console.ForegroundColor = ConsoleColor.White;
                Update();
                System.Threading.Thread.Sleep(10);
                Tick++;
            }
        }

        public bool WaitForInitialization()
        {
            Console.WriteLine("Waiting for Connections");
            // Initialization

            // Test client
             Bots.Add(new TestClient());
             Bots.Add(new TestClient());




            // //            
            foreach (Client c in Bots)
            {
                Bodies.Add(c.Init());
            }

            Bodies[1].Position.X = 500;
            Bodies[1].Fraction = 1;

            return true; // false on error
        }

    }
}

