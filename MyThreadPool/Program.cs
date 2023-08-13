namespace MyThreadPool
{
    internal class Program
    {
        private static readonly object _lock = new object();
        static void Main(string[] args)
        {
            var t = new MyThreadPool(4, Priority.NORMAL);

            for (int i = 0; i < Console.WindowWidth; i++)
            {
                if (i == 5) //Зупеняю Thread
                {
                    t.Stop();
                }
                t.Run((margenLeft) => // margenLeft звідкідя ?
                {
                    for (int j = 0; j < 10; j++)
                    {
                        lock (_lock)
                        {
                            Console.SetCursorPosition((int)margenLeft, j);
                            Console.WriteLine("*");
                        }
                    }

                }, i);
            }
        }
    }
}