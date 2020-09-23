using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Client
{
    interface IBike
    {
        event EventHandler<float> OnSpeed;
    }



    public class SimBike : IBike
    {
        public event EventHandler<float> OnSpeed;


        public SimBike()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
        }

        private void WorkThreadFunction()
        {
            while(true)
            {
                OnSpeed?.Invoke(this, 10.0f);
                Thread.Sleep(1000);
            }
        }
    }


    public class RealBike : IBike
    {
        public event EventHandler<float> OnSpeed;
        public RealBike(string name)
        {

        }
    }
}
