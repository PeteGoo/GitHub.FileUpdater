using CommandLine;
using CommandLine.Text;

namespace GitHub.FileUpdater
{
    internal class CommandLineArguments
    {
        [Option('f', "filePath", Required = true, HelpText = "The path to the file to upload")]
        public string FilePath { get; set; }

        [Option('o', "repoOwner", Required = true, HelpText = "The owner of the repo")]
        public string RepoOwner { get; set; }

        [Option('r', "repoName", Required = true, HelpText = "The name of the repo")]
        public string RepoName { get; set; }

        [Option('p', "repoPath", Required = true, HelpText = "The relative path within the repo of the file to update.")]
        public string RepoPath { get; set; }

        [Option('t', "accessToken", Required = true, HelpText = "The access token that provides write access to the repo.")]
        public string AccessToken { get; set; }

        [Option('m', "message", Required = true, HelpText = "The commit message.")]
        public string CommitMessage { get; set; }

        [Option('n', "name", Required = true, HelpText = "The committer's name.")]
        public string CommitterName { get; set; }

        [Option('e', "email", Required = true, HelpText = "The committer's email.")]
        public string CommitterEmail { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {

            var help = new HelpText
            {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("Usage: github.fileupdater -f pathToFile -o repoOwner -m repoName -p repoPath -t accessToken -m message -n name -e email");
            help.AddOptions(this);

            return help;
        }

    }
}