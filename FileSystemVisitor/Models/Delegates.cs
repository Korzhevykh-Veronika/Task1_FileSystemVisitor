namespace FileSystemVisitor.Models
{
    public class Delegates
    {
        public delegate bool Filter(string name);

        public delegate void ShowMessage();

        public delegate SearchOperation ProcessItem(string name);
    }
}
