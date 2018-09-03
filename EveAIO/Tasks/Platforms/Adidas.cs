namespace EveAIO.Tasks.Platforms
{
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using System;

    internal class Adidas : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private object _region;
        private object _productCode;

        public Adidas(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc() => 
            false;

        public bool Checkout() => 
            false;

        public bool DirectLink(string link) => 
            false;

        public bool Login() => 
            false;

        public bool Search()
        {
            throw new NotImplementedException();
        }

        public void SetClient()
        {
        }
    }
}

