using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ProcessAlert
{
    public class Orchestrator : IState
    {
        public bool IsRunning { get; set; } = false;
        private Worker _worker;
        private List<Worker> _workers;
        public string KAcess { get; set; }

        public Orchestrator(string kAcess)
        {
            this.KAcess = kAcess;
        }
        
        public void CreateWorker(string symbol, string aboveValue, string downValue, string id)
        {
            this._worker = new Worker(this,  symbol, aboveValue, downValue, id);
        }
        
        public void InsertWorkerToPool(string id)
        {
            if(id == this._worker.Id)
                this._workers.Add(this._worker);
        }
        
        public int ManyWorkerActive()
        {
            return _workers.Count;
        }
    }
    
    public class Worker : IState
    {
        public bool IsRunning { get; set; } = false;
        public bool Done = false;
        public string KAcess { get; set; }
        public string Id { get; set; }
        private IState _context;
        private string _symbol;
        private string _aboveValue;
        private string _downValue;

        public Worker(IState context, string symbol, string aboveValue, string downValue, string id)
        {
            this.Id = id;
            this._context = context;
            this._symbol = symbol;
            this._aboveValue = aboveValue;
            this._downValue = downValue;
        }
        
        public async Task<string> CallProcess()
        {
            this._context.IsRunning = true;
            string output = "";

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "/home/agl/Codes/csharp/test/Task/test_codigo.o";
                process.StartInfo.Arguments =
                    $"app --symbol {this._symbol} --above-value {this._aboveValue} --down-value {this._downValue} --k-acess {this._context.KAcess}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                
                // Synchronously read the standard output of the spawned process. 
                StreamReader reader = process.StandardOutput;
                output = await reader.ReadToEndAsync();
                process.WaitForExit();

                output = output.ToString();
            }
        
            this._context.IsRunning = false;
            this.Done = true;
            return output;
        }
    }
}