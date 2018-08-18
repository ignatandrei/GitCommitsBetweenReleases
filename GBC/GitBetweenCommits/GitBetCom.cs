using Octokit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GitBetweenCommits
{
    public class DateForCommit
    {
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime CommitDate { get; set; }
    }
    public class GitBetCom
    {
        public string  Author { get; set; }
        public string RepositoryName { get; set; }
        public async Task<DateForCommit[]> BetweenLatest2Commits()
        {
            var client = new GitHubClient(new ProductHeaderValue("GitBetweenCommits"));
            var rep = await client.Repository.Get(Author, RepositoryName);
            var relLatest = await client.Repository.Release.GetLatest(rep.Id);
            var relAll = await client.Repository.Release.GetAll(rep.Id, new ApiOptions() { PageSize = int.MaxValue - 1 });
            var relBeforeLatest = relAll.OrderByDescending(it => it.CreatedAt).Skip(1).FirstOrDefault();

            var dateLatest = relLatest.CreatedAt;
            var dateBeforeLatest = relBeforeLatest.CreatedAt;
            
            var commits = await client.Repository.Commit.GetAll(rep.Id, ApiOptions.None);
            var res = commits
                .Where(it => 
                it.Commit.Author.Date >= dateBeforeLatest
                &&
                it.Commit.Author.Date<=dateLatest
                )
                .Select(it => new DateForCommit()
                {
                    Author = it.Commit.Author.Name,
                    Message = it.Commit.Message,
                    CommitDate = it.Commit.Author.Date.DateTime
                })
                .ToArray();
            return res;
        }
        public async Task<DateForCommit[]> FromLastCommit()
        {
            var client = new GitHubClient(new ProductHeaderValue("GitBetweenCommits"));
            var rep = await client.Repository.Get(Author, RepositoryName);
            var rel = await client.Repository.Release.GetLatest(rep.Id);
            var date = rel.CreatedAt;

            var commits = await client.Repository.Commit.GetAll(rep.Id, ApiOptions.None);
            var res = commits
                .Where(it => it.Commit.Author.Date > date)
                .Select(it => new DateForCommit()
                { Author= it.Commit.Author.Name,
                   Message=  it.Commit.Message,
                    CommitDate=it.Commit.Author.Date.DateTime })
                .ToArray();
            return res;
        }
    }
}
