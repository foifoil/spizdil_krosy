namespace EveAIO.Tasks.Platforms
{
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using System;

    internal class JimmyJazz : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;

        public JimmyJazz(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            throw new NotImplementedException();
        }

        public bool Checkout()
        {
            throw new NotImplementedException();
        }

        public bool DirectLink(string link)
        {
            throw new NotImplementedException();
        }

        public bool Login()
        {
            throw new NotImplementedException();
        }

        public bool Search()
        {
            throw new NotImplementedException();
        }

        public void SetClient()
        {
        }
    }
}

