using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mech.MClient;
using MathHelper;

namespace Mech.Data
{


    public class AnswerData
    {
        public long Tick;
        public int PlayerID;

        public List<BodySettings> Settings;
        public List<BodySettings> EnemySettings;
    }

    public class BodySettings
    {
        public float X;
        public float Y;
        public float Radius;
        public float Hp;
        public float Energy;
        public float Angle;
        public int ID;
        public bool isEnemy;
        public int Fraction;
    }



    public class Body
    {
        public Vector2 Position;
        public Vector2 Forces;
        public float Energy;
        public float Hp;
        public float Radius;
        public float Angle;
        public int Fraction;

        public bool Controllable;
        public Client Parent;
        public int ID;
    }

    public enum Command
    {
        Force,
        Rotate,

        CreateBody,
        
        
    }

    public class Cmd
    {
        public Command cmd;
        public int id;
        public float data;
        public float data2;
        public float data3;
    }
    
}
