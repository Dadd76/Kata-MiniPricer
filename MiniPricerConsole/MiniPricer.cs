using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pricer
{
    public class MiniPricer
    {
        IrandomGenerator _randomGen;
        DateTime _dtStart;

        private Object _lock = new Object();
        private Object _lock2 = new Object();

        public MiniPricer(IrandomGenerator r,DateTime dt)
        {
            _randomGen  = r;
            _dtStart    = dt;
        }

        public int normalizeRandomNumber()
        {
            int result = 0;

          //  Interlocked.Add(ref Count, 1);

            lock (_lock2)
                result = _randomGen.getNumber(3) - 1;
   
            return result;
        }

        public decimal GetMonteCarloPriceWithParalleleFor(DateTime de, decimal volatility, decimal initialPrice, int nbTirage)
        {
            decimal MonteCarloPrice;
            decimal price;
            MonteCarloPrice = 0;

            Parallel.For(0, nbTirage, i =>
             {
                 price = initialPrice;

                 decimal result = GetBasicPriceFor(de, volatility, price);

                 lock (_lock)
                 {
                     MonteCarloPrice +=  result; 
                 }

             });

            return (MonteCarloPrice / nbTirage);
        }

        public decimal GetMonteCarloPriceWithParalleleWithTableFor(DateTime de, decimal volatility, decimal initialPrice, int nbTirage)
        {
            decimal price;
            decimal[] result = new decimal[nbTirage];

            Parallel.For(0, nbTirage, i =>
            {
                price = initialPrice;
                result[i] = GetBasicPriceFor(de, volatility, price);
            });

            return result.Average();
        }




        public decimal GetMonteCarloPriceFor(DateTime de, decimal volatility, decimal initialPrice, int nbTirage)
        {
            decimal price;
            decimal monteCarloPrice = 0;

            for (int y = 0; y < nbTirage; y++)
            {
                price = initialPrice;
                monteCarloPrice += GetBasicPriceFor(de, volatility, price);
            }

            return (monteCarloPrice/ nbTirage);
        }

        public decimal GetBasicPriceFor(DateTime de, decimal volatility, decimal initialPrice)
        {
            int workingDays = GetWorkingDays(de);
            decimal calculatedPrice = initialPrice;
            for (int i = 0; i < workingDays; i++)
            {
                int randomVolatility = this.normalizeRandomNumber();

                if(randomVolatility != 0)
                    calculatedPrice = (calculatedPrice * (1 + randomVolatility * volatility / 100));

                // initialPrice = (initialPrice * Math.Abs( 1 * randomVolatility + volatility / 100));
            }  

            return calculatedPrice;
        }

        private bool isWorkindDay(DateTime dateToTest)
        {
            if (dateToTest.DayOfWeek != DayOfWeek.Saturday && dateToTest.DayOfWeek != DayOfWeek.Sunday)
            {
                return true;
            }
            return false;
        }

        private int GetWorkingDays(DateTime dte)
        {
            TimeSpan ts =   dte - _dtStart;
            int workingDays = 0;

            for (int i = 0; i < ts.Days; i++)
            {
                if(isWorkindDay(_dtStart.AddDays(i)))
                {
                    workingDays++;
                }
            }

            return workingDays;
        }
    }
}
