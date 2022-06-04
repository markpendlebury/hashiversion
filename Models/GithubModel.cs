namespace hashiversion.Models
{

    /// <summary>
    /// These classes are used to deserialise the 
    /// response from github
    /// They make it easier to work with
    /// in linq
    /// </summary>
    public class Commit
    {
        public string sha { get; set; }
        public string url { get; set; }
    }

    public class GithubModel
    {
        public string name { get; set; }
        public string zipball_url { get; set; }
        public string tarball_url { get; set; }
        public Commit commit { get; set; }
        public string node_id { get; set; }
    }
}