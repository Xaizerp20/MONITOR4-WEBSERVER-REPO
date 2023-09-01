namespace Monitor4WebServer.Model
{
    abstract class StateBase
    {
        public abstract void Free();
        public abstract void Close();
        public abstract void Occupied();
        public abstract void Open();
        public abstract void Cleaning();
        public abstract void Maintenance();
    }
}
