using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommandLine;
using GitHub.FileUpdater.Model;
using Octokit;
using Octokit.Internal;

namespace GitHub.FileUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineArguments();
            Parser.Default.ParseArguments(args, options);

            if (options.LastParserState != null && options.LastParserState.Errors.Any())
            {

                // Values are available here
                return;
            }

            Console.WriteLine("Uploading {0} to repo {1}/{2} at path {3}", options.FilePath, options.RepoOwner, options.RepoName, options.RepoPath);

            try
            {
                UploadFile(options)
                    .Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem uploading the file: '{0}'\r\n{1}", e.Message, e);
                Environment.ExitCode = 1;
            }

        }


        private static async Task UploadFile(CommandLineArguments commandLineArguments)
        {
            if (!File.Exists(commandLineArguments.FilePath))
            {
                throw new FileNotFoundException("The file was not found", commandLineArguments.FilePath);
            }
            var github = new GitHubClient(new ProductHeaderValue("mmbotScriptsDeployment"), new InMemoryCredentialStore(new Credentials(commandLineArguments.AccessToken)));

            var uri = new Uri(string.Format("https://api.github.com/repos/{0}/{1}/contents/{2}", commandLineArguments.RepoOwner, commandLineArguments.RepoName, commandLineArguments.RepoPath), UriKind.Absolute);
            var getFileResponse = await
                github.Connection.GetAsync<GetContentsResponse>(uri);

            if (getFileResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception(string.Format("Could not fetch file {0}. \r\n{1}", uri, getFileResponse.BodyAsObject));
            }

            var sha = getFileResponse.BodyAsObject.sha;

            var updateContent = new
            {
                message = commandLineArguments.CommitMessage,
                committer = new
                {
                    name = commandLineArguments.CommitterName,
                    email = commandLineArguments.CommitterEmail
                },
                content = Convert.ToBase64String(File.ReadAllBytes(commandLineArguments.FilePath)),
                sha = sha
            };

            var updateFileResponse =
                await github.Connection.PutAsync<UpdateContentsResponse>(uri, updateContent);

            if (updateFileResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception(string.Format("Could not update file {0}. \r\n{1}", uri, getFileResponse.BodyAsObject));
            }
        }
    }
}
