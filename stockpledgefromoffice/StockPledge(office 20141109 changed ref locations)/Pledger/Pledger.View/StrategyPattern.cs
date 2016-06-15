using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pledger.View
{
    public interface IQuackBehavior
    {
        string Quacking();
    }

    public class QuackA : IQuackBehavior
    {
        public string Quacking()
        {
            return "Quack";
        }
    }

    public class Duck
    {
        public IQuackBehavior QuackBehavior { get; set; }

        public object PerformQuack()
        {
            return QuackBehavior.Quacking();
        }
    }

    public class MallardDuck : Duck
    {
        public MallardDuck()
        {
            QuackBehavior = new QuackA();
        }
    }
}
