using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tickets
{
    public class Process
    {
        public Process(Object lockObject)
        {
            this._lock = lockObject; 
        }
  
        readonly object _lock;
        private static Queue<Ticket> queue = new Queue<Ticket>();
        public static int ticketSelled = 0;
        public static bool SellOut = false;
        public void GenerateTicket()
        {
            lock (_lock)
            {
                var r = new Random(GetRandomSeed());

                var start = r.Next(1, Program.STOPS);
                var end = r.Next(start + 1, Program.STOPS + 1);
                var ti =  new Ticket()
                {
                    Start = start,
                    End = end,
                };
                
                queue.Enqueue(ti);
                Monitor.Pulse(_lock);
            }
        }   

        public void ProcessTicket()
        {
            lock (_lock)
            {
                if(SellOut) return;
                while (queue.Count == 0)
                {
                    // This releases _lock, and block itself, only after reacquiring it
                    // does it continue
                    Monitor.Wait(_lock);
                }

                var sellings = _lock as List<TicketsPerSeat>;  

                var processItem = queue.Dequeue();

                var assignableSeats = sellings.Where(s => s.MinAvailableStop <= processItem.Start).ToList();
                if (assignableSeats.Any())
                {
                    assignableSeats.Sort();
                    var targetSeat = assignableSeats[0];
                    targetSeat.Selled.Add(processItem);
                    ticketSelled++;
                }
                
                var availableSeats = sellings.Where(s => s.MinAvailableStop <= (Program.STOPS - 1)).ToList();
                if (!availableSeats.Any())
                {
                    SellOut = true;
                    Console.WriteLine("A total of " + ticketSelled + " tickets were sold");
                    Console.WriteLine("Seats - " + Program.SEATS + " Stops - "+Program.STOPS);
                    Console.WriteLine();

                    var totalCells = Program.STOPS*Program.SEATS;
                    var selledCells = 0;

                    foreach (var selling in Program.Sellings)
                    {
                        Console.WriteLine("-------------- for seat " + selling.SeatSerial);
                        foreach (var ticket in selling.Selled)
                        {
                            Console.WriteLine("Ticket start " + ticket.Start + " destination " + ticket.End);
                            selledCells += ticket.End - ticket.Start + 1;
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine("Vacancy rate - " + (1- selledCells/(totalCells+0.0)));
                }
 
            }
        }

        private int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
