using System;
using NUnit.Framework;
using Pricer;
using System.Diagnostics;

namespace MiniPricerTestProject
{
    [TestFixture]
    public class MiniPricerUnitTest
    {

        static int[] EvenNumbers = new int[] { 5, 100, 1, 105 };
      
        //104.7375
        [TestCase(5,100,1,105)]
        [TestCase(5,100,2,110.25)]
        [TestCase(5,100,3,110.25)]
        [TestCase(5,100,4,110.25)]
        [TestCase(5,100,5,104.7375)]
       //  [Test, TestCaseSource("EvenNumbers")]
        public void CheckBasicPriceForDay(decimal volatility,decimal price,int day, decimal result )
        {
            RandomMockGenerator r       = new RandomMockGenerator();
            DateTime startDate          = new DateTime(2016,6,23);

            r.addNumber(2);
            r.addNumber(2);
            r.addNumber(0);
            r.addNumber(1);
            r.addNumber(2);
            r.addNumber(1);
            r.addNumber(1);

            MiniPricer myMiniPricer = new MiniPricer(r,startDate);
            Assert.AreEqual(result, myMiniPricer.GetBasicPriceFor(startDate.AddDays(day), volatility,price));
        }


        [TestCase(5, 100, 100)]
        public void CheckExtremumPricePriceForDay(decimal volatility, decimal price, int day)
        {
            RandomMockGenerator r = new RandomMockGenerator();
            DateTime startDate = new DateTime(2016, 6, 23);

            for (int i=0; i<100; i++)
                r.addNumber(2);

            MiniPricer myMiniPricer = new MiniPricer(r, startDate);
            Assert.IsNotNull( myMiniPricer.GetBasicPriceFor(startDate.AddDays(day), volatility, price));

            r.clear();
            myMiniPricer = new MiniPricer(r, startDate);

            for (int i = 0; i < 100; i++)
                r.addNumber(0);

            Assert.IsNotNull(myMiniPricer.GetBasicPriceFor(startDate.AddDays(day), volatility, price));
        }

        //3354.5134152904012714143138861M
        //2.4894280619189487224780035505M
        [TestCase(5, 100, 100)]
        public void CheckPriceForDay(decimal volatility, decimal price, int day)
        {
            randomGenerator r = new randomGenerator();
            DateTime startDate = new DateTime(2016, 6, 23);
            MiniPricer myMiniPricer = new MiniPricer(r, startDate);
            decimal result = myMiniPricer.GetBasicPriceFor(startDate.AddDays(day), volatility, price);
            Assert.IsTrue( (result >= 2.4894280619189487224780035505M) && (result <= 3354.5134152904012714143138861M)  );
        }


        [TestCase(5, 100, 100, 100000)]
        public void CheckMonteCarloPriceForDay(decimal volatility, decimal price, int day, int nbTirage)
        {
            // randomGenerator r = new randomGenerator();
            debugRandomGenerator dr     = new debugRandomGenerator();
            DateTime startDate          = new DateTime(2016, 6, 23);  
            MiniPricer myMiniPricer     = new MiniPricer(dr, startDate);

            decimal result = myMiniPricer.GetMonteCarloPriceFor(startDate.AddDays(day), volatility, price, nbTirage);

            Assert.IsTrue((result >= 2.4894280619189487224780035505M) && (result <= 3354.5134152904012714143138861M));
            Assert.That(dr.Count == 7200000);
        }

        [TestCase(5, 100, 100, 100000)]
        public void CheckMonteCarloPriceForDay2(decimal volatility, decimal price, int day, int nbTirage)
        {
            //randomGenerator r = new randomGenerator();
            debugRandomGenerator dr     = new debugRandomGenerator();
            DateTime startDate          = new DateTime(2016, 6, 23);
            MiniPricer myMiniPricer     = new MiniPricer(dr, startDate);
            Stopwatch stopWatch         = new Stopwatch();

            stopWatch.Start();
            decimal result = myMiniPricer.GetMonteCarloPriceWithParalleleFor(startDate.AddDays(day), volatility, price, nbTirage);
            stopWatch.Stop();

            Assert.IsTrue((result >= 2.4894280619189487224780035505M) && (result <= 3354.5134152904012714143138861M));
            Assert.That(dr.Count == 7200000);
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Trace.WriteLine("RunTime ---->" + elapsedTime);
        }

        [TestCase(5, 100, 100, 100000)]
        public void CheckMonteCarloPriceForDay3(decimal volatility, decimal price, int day, int nbTirage)
        {
            //randomGenerator r = new randomGenerator();
            debugRandomGenerator2 dr    = new debugRandomGenerator2(); 
            DateTime startDate          = new DateTime(2016, 6, 23);
            MiniPricer myMiniPricer     = new MiniPricer(dr, startDate);
            Stopwatch stopWatch         = new Stopwatch();

            stopWatch.Start();
            decimal result = myMiniPricer.GetMonteCarloPriceWithParalleleWithTableFor(startDate.AddDays(day), volatility, price, nbTirage);
            stopWatch.Stop();

            Assert.IsTrue((result >= 2.4894280619189487224780035505M) && (result <= 3354.5134152904012714143138861M));
            Assert.That(dr.Count == 7200000);
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine("RunTime ---->" + elapsedTime);

        }




    }

}
