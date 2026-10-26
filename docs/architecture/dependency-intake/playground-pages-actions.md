# Playground Pages action intake

GitHub Pages was selected by Michaël Hompus for issue #111, followed by an
instruction to implement deployment. The required new dependencies are the
official GitHub-owned `actions/upload-pages-artifact` 5.0.0 and
`actions/deploy-pages` 5.0.1 actions. They execute only in the publication
workflow; they do not add browser or compiler dependencies.

Both actions come from the same GitHub-owned source already used for checkout,
toolchain setup, and artifact handling. The repository's `gh actions-lock`
tool records immutable commits, repository/owner identities, and transitive
action references in [actions.lock](../../../.github/workflows/actions.lock).
The upload composite's transitive `actions/upload-artifact` reference is also
SHA-pinned and locked. The deploy action runs its checked-in Node 24 bundle;
the upload action archives the already validated static files. Neither invokes
application package installation or a compiler service.

Using the official actions preserves GitHub's supported Pages artifact format,
OIDC authorization, and deployment status handling. A custom tar/API publisher
would duplicate those responsibilities. Existing checkout, setup-node,
download-artifact, and upload-artifact pins are reused. The new actions have
read/artifact access during preparation; only the isolated deploy job receives
Pages write and OIDC permissions. It does not check out or execute the downloaded
application. PR artifacts cannot reach it.

Review upstream action changes through the existing weekly GitHub Actions
Dependabot configuration, regenerate the lockfile with `gh actions-lock`, and
rerun candidate/live verification on updates. Tooling and GitHub runners remain
build-time trust boundaries; public byte/MIME verification and retained source
and hash evidence cover the resulting deployment. See the
[runbook](../../guides/playground-deployment.md) for gate and failure behavior.

Primary sources: [upload action](https://github.com/actions/upload-pages-artifact/tree/v5.0.0),
[deployment action](https://github.com/actions/deploy-pages/tree/v5.0.1), and
[GitHub Pages workflows](https://docs.github.com/en/pages/getting-started-with-github-pages/using-custom-workflows-with-github-pages).
