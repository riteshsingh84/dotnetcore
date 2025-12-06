<!--
Use this template to open PRs. Fill in sections and check the checklist before requesting review.
-->

## Summary
<!-- Short description of the change and motivation -->

## Related issues
- Fixes / relates to: #

## Changes
- What changed (high level)
- Key files / areas touched

## How to test
- Steps to reproduce / test locally
- Any special setup (user-secrets, docker-compose, env vars)

## Checklist (required)
- [ ] The code builds locally and CI is green.
- [ ] New logic has unit and/or integration tests where appropriate.
- [ ] Tests are passing.
- [ ] New public APIs are documented (Swagger + XML comments).
- [ ] No secrets or sensitive data in the diff.
- [ ] Static analyzers and formatting applied (`dotnet format`, analyzers).
- [ ] Security considerations documented (threats/mitigations).
- [ ] Database migration created and applied (if schema changed).
- [ ] Documentation (.md) updated if behavior/config changed.

## Notes
<!-- Any additional notes for reviewers (migration tips, backward compat) -->