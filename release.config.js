module.exports = {
    branches: ["main"],
    plugins: [
        [
            "@semantic-release/commit-analyzer",
            {
                preset: "angular",
                releaseRules: [
                    {
                        type: "doc",
                        release: "patch",
                    },
                    {
                        type: "fix",
                        release: "patch",
                    },
                    {
                        type: "patch",
                        release: "patch",
                    },
                    {
                        type: "minor",
                        release: "minor",
                    },
                    {
                        type: "feat",
                        release: "minor",
                    },
                    {
                        type: "feature",
                        release: "minor",
                    },
                    {
                        scope: "no-release",
                        release: false,
                    },
                ],
            },
        ],
        "@semantic-release/release-notes-generator",
        [
            "@semantic-release/github",
            {
                "assets": ["./newpackages/*.nupkg"],
            }],
        [
            "semantic-release-dotnet",
            {
                paths: ["./Directory.Build.props"],
            },
        ],
        [
            "@semantic-release/exec",
            {
                prepareCmd: "sh pack.sh ${nextRelease.version}",
            },
        ],
    ],
}
