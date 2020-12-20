namespace ProcessAlert
{
    public interface IState
    {
        public bool IsRunning { get; set; }
        public string KAcess { get; set; }
    }
}