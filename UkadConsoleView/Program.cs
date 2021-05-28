using System;
using UKADConsoleView.Views;


namespace UkadConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var LinkView = new LinkView(new ResultWritter());

            LinkView.StartWork();
        }
    }
}
