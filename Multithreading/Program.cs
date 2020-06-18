using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    class Program
    {
        delegate int OwnDelegate(string ciag);

        static void Main(string[] args)
        {
            string gg = "66";


            // A way to make Main async.
            MainAsync().Wait();


            // Thread sample:
            Thread thread1 = new Thread(new ThreadStart(ThreadMethod));
            Thread thread2 = new Thread(new ParameterizedThreadStart(ThreadMethodI));

            thread1.Start();
            thread2.Start(1);

            thread1.Join();
            thread2.Join();


            // Delegate usage sample:
            OwnDelegate a = IntMethod1;
            a += IntMethod2;
            a += (x => x.Length + 1);
            int b = a(gg);


            // Task usage sample:
            Task<int> taskInt1 = new Task<int>(WrapperMethod, gg);
            taskInt1.Start();
            WrapperClass<string, int> wrapperClass = new WrapperClass<string, int>(IntMethod2, gg);
            Task<int> taskInt2 = Task.Run(wrapperClass.Call);

            Task<int> abc = Task.Run(() => IntMethod1(gg));

            // async method call from sync code:
            Task<int> innyTask = IntMethodAsync(1);

            // async lambda:
            var test = new Func<Task<int>>(async () => await IntMethodAsync(gg));

            Task.WaitAll(taskInt1, innyTask);


            // btw: Nullable
            int? nullableInt = null;
            nullableInt = 6;
            if (nullableInt != null) nullableInt++;


            // LINQ
            var list = new List<int> { 1, 2, 3, 4 };
            var aa = list.Select(x => x + 3);
            aa.Where(x => x != null);
            var cc = list.Aggregate((x, y) => x + y);
            Console.WriteLine(cc);

            int WrapperMethod(object state)
            {
                return IntMethod1(gg);
            }
        }

        class WrapperClass<T, TR>
        {
            private readonly Func<T, TR> func;
            private readonly T param;

            internal WrapperClass(Func<T, TR> func, T param)
            {
                this.func = func;
                this.param = param;
            }

            public TR Call()
            {
                return func(param);
            }
        }

        static void ThreadMethod()
        {
            Thread.Sleep(1000);
            //varint++;
        }

        static void ThreadMethodI(object i)
        {
            Thread.Sleep(1000);
            //varint++;
        }

        static async Task MainAsync()
        {
            //Console.WriteLine(await IntMethodAsync(4));

        }

        static int IntMethod1(string ciag)
        {
            return int.Parse(ciag);
        }

        static int IntMethod2(string ciag)
        {
            return int.Parse(ciag) + 2;
        }


        /// <summary>
        /// async method sample
        /// </summary>
        static async Task<int> IntMethodAsync(object i)
        {
            await Task.Delay(100);
            return (int)i;
        }
    }
}
