using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mech.Data;

namespace Mech.MClient
{
    public class Client
    {
        public int bodyIDIncrementor = 0;
        public bool isConnected;
        public String ansData;

        public void addData(string data)
        {
            //    Console.WriteLine(data);
            ansData = data;
        }

        public virtual void SendData()
        {
            //Send Data

            //Clear           
            ansData = "";
        }
        public virtual string WaitForAnswer() { return null; }

        public virtual Body Init()
        {
            return null;
        }


    }
}
