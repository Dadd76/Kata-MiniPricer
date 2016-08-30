using Pricer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MiniPricerTestProject
{
    public class RandomMockGenerator : IrandomGenerator
    {
        Queue<int> _queue = new Queue<int>();

         public void addNumber(int i)
        {
            _queue.Enqueue(i);
        }

        public int getNumber(int i)
        {
            return _queue.Dequeue();
        }

        public void clear()
        {
            _queue.Clear();
        }
    }

    public class randomMockGenerator2 : IrandomGenerator
    {
        Queue<int> _queue = new Queue<int>();
        public int called =0;

        public void addNumber(int i)
        {
            _queue.Enqueue(i);
        }

        public int getNumber(int i)
        {
     
                called++ ;
  
            return _queue.Dequeue();
        }
    }
}
