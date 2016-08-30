using System;
using System.Threading;

namespace Pricer
{
    public interface  IrandomGenerator
    {
        int getNumber(int i);
       
    }

    public class randomGenerator : IrandomGenerator
    {
        private Random _r;
        public randomGenerator()
        {
           _r  = new Random();
        }
        public int getNumber(int i)
        {
            return _r.Next(i);
        }
    }

    public class debugRandomGenerator : IrandomGenerator
    {
        private Random _r;
        public int Count = 0;
        private Object _lock = new Object();

        public debugRandomGenerator()
        {
            _r = new Random();
        }

        public int getNumber(int i)
        {
            lock (_lock)
            {
                Count++;
            }
              
            return _r.Next(i);
        }
    }

    public class debugRandomGenerator2 : IrandomGenerator
    {
        private Random _r;
        public int Count = 0;

        public debugRandomGenerator2()
        {
            _r = new Random();
        }

        public int getNumber(int i)
        {
            Interlocked.Add(ref Count, 1);
            return _r.Next(i);
        }
    }
}