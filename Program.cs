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


        public static List<TicketsPerSeat> Sellings = new List<TicketsPerSeat>();
        static void Main(string[] args)
        {
            Init();

            var process = new Process(Sellings);
            int i = 0;
            while (i++<50)
            {
                var t = new Thread(process.GenerateTicket);
                t.Start();

                if (!Process.SellOut)
                {
                    var t1 = new Thread(process.GenerateTicket);
                    t1.Start();

                    var t2 = new Thread(process.ProcessTicket);
                    t2.Start();
                }
            }
            
            Console.ReadLine();
        }

        static void Init()
        {
            for (int i = 0; i < SEATS; i++)
            {
                var seatSelling = new TicketsPerSeat();
                seatSelling.SeatSerial = i + 1;
                seatSelling.Selled = new List<Ticket>();
                Sellings.Add(seatSelling);
            }
        }
 
        
    }
}
