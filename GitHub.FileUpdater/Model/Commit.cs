namespace GitHub.FileUpdater.Model
{
    public class Commit
    {
        public string sha { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public Author author { get; set; }
        public Committer committer { get; set; }
        public Tree tree { get; set; }
        public string message { get; set; }
        public Parent[] parents { get; set; }
    }
}