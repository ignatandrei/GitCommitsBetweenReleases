using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GitBetweenCommits;
using McMaster.Extensions.CommandLineUtils;

namespace GBC
{
    [Command(Description = "GitHub notes between commits")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "The name of the author")]
        [Required]
        public string AuthorName { get; }

        [Argument(1, Description = "The name of the repository")]
        [Required]
        public string RepositoryName { get; }

        //[Option(Description = "An optional parameter, with a default value.\nThe number of times to say hello.")]
        //[Range(1, 1000)]
        //public int Count { get; } = 1;

        private int OnExecute()
        {
            try
            {
                var gbc = new GitBetCom();
                gbc.RepositoryName = RepositoryName;
                gbc.Author = AuthorName;
                var data = gbc.BetweenLatest2Commits().GetAwaiter().GetResult();
                var lines = data
                    .Select(it => $"at {it.CommitDate}  {it.Author} {it.Message}")
                    .ToArray();
                ;
                Console.WriteLine(string.Join(Environment.NewLine, lines));
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("exception !" + ex.ToString());
                return -1;
            }
            
        }
    }
}
