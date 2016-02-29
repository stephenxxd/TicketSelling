using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tickets
{
    class Program
    {
        public static  int STOPS = 7;
        public static int SEATS = 10;
        public static bool SellOut = false;


        public static TotalSelling ToSell = new TotalSelling();
        static void Main(string[] args)
        {
            Init();

            var process = new Process(ToSell);
            int i = 0,j=0;
            while (i++<50)
            {
                 
                var t = new Thread(process.GenerateTicket);
                t.Start();

                var t2 = new Thread(process.ProcessTicket);
                t2.Start();

            }

            lock (ToSell)
            {
                while (!ToSell.SellOut)
                {
                    var t = new Thread(process.GenerateTicket);
                    t.Start();

                    var t2 = new Thread(process.ProcessTicket);
                    t2.Start();
                }
            }

            //j = 0;
            //while (!ToSell.SellOut)
            //{
            //    var t1 = new Thread(process.GenerateTicket);
            //    t1.Start();

            //    while (j++ < 3)
            //    {
            //        var t2 = new Thread(process.ProcessTicket);
            //        t2.Start();
            //    }
            //}
            
            Console.ReadLine();
        }

        static void Init()
        {
            ToSell.Sellings = new List<TicketsPerSeat>();
            for (int i = 0; i < SEATS; i++)
            {
                var seatSelling = new TicketsPerSeat();
                seatSelling.SeatSerial = i + 1;
                seatSelling.Selled = new List<Ticket>();
                ToSell.Sellings.Add(seatSelling);
            }

            ToSell.SellOut = false;
        }
 
        
    }
}
