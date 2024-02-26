# Contributing to ReForge.Union

First off, thank you for considering contributing to ReForge.Union! It's people like you that make ReForge.Union such a great tool.

## Where do I go from here?

If you've noticed a bug or have a feature request, make sure to check our [Issues](https://github.com/nalcorso/ReForge.Union/issues) to see if someone else in the community has already created a ticket. If not, go ahead and [make one](https://github.com/nalcorso/ReForge.Union/issues/new)!

## Fork & create a branch

If this is something you think you can fix, then [fork ReForge.Union](https://help.github.com/articles/fork-a-repo) and create a branch with a descriptive name.

A good branch name would be (where issue #123 is the ticket you're working on):

```shell
git checkout -b fix-123
```

## Implement your fix or feature

At this point, you're ready to make your changes! Feel free to ask for help; everyone is a beginner at first.

## Get the code

The first thing you'll need to do is get the code. The easiest way to do this is to fork the project on GitHub and clone it:

```shell
git clone https://github.com/yourusername/ReForge.Union.git
```

## Make a Pull Request

At this point, you should switch back to your master branch and make sure it's up to date with the latest ReForge.Union master branch:

```shell
git remote add upstream https://github.com/nalcorso/ReForge.Union.git
git checkout master
git pull upstream master
```

Then update your feature branch from your local copy of master and push it!

```shell
git checkout fix-123
git rebase master
git push --set-upstream origin fix-123
```

Go to the [ReForge.Union repo](https://github.com/nalcorso/ReForge.Union) and press the "Compare & pull request" button.

Write a description of your changes, choose "Create pull request", and you're done!

## Keeping your Pull Request updated

If a maintainer asks you to "rebase" your PR, they're saying that a lot of code has changed, and that you need to update your branch so it's easier to merge.

To learn more about rebasing in Git, there are a lot of [good](https://git-scm.com/book/en/v2/Git-Branching-Rebasing) [resources](https://www.atlassian.com/git/tutorials/merging-vs-rebasing) but here's the suggested workflow:

```shell
git checkout fix-123
git pull --rebase upstream master
git push --force-with-lease origin fix-123
```

## Merging a PR (maintainers only)

A PR can only be merged into master by a maintainer if:

- It is passing CI.
- It has been approved by at least two maintainers. If it was a maintainer who opened the PR, only one extra approval is needed.
- It has no requested changes.
- It is up to date with current master.

Any maintainer is allowed to merge a PR if all of these conditions are met.

## Thank you!

Your contributions to open source, large or small, make great projects like this possible. Thank you for taking the time to contribute.