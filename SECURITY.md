# Security Policy

Thank you for helping keep this project secure. This document explains how to report security vulnerabilities, what to expect after reporting, and the security practices we follow as maintainers.

## Reporting a Vulnerability (Preferred)
If you discover a potential security issue in this repository, please report it privately so we can triage and fix it before public disclosure.

Preferred reporting methods (in order):
1. Use GitHub's private Security Advisories (Security → Create a new security advisory) for this repository.
2. If you cannot access the Security Advisories page, open a new issue in this repository with the title prefixed by `[SECURITY]` and include "CONFIDENTIAL" in the first line of the description, then mention the maintainer `@riteshsingh84`. Mark the issue as private if your organization supports private issues.
3. If you must use email, send a message to the repository maintainers via your GitHub account (mention `@riteshsingh84`) and request a secure channel. We will provide an alternative secure channel (PGP or an encrypted email address) if necessary.

Please do NOT create a public issue or share details publicly before we have had a chance to investigate and patch.

## What to Include in Your Report
A concise, reproducible report helps us triage and fix issues quickly. Please provide:
- A short summary of the issue and impact.
- Steps to reproduce the vulnerability (commands, URLs, sample payloads).
- Expected vs actual behavior.
- The version/commit/tag where you observed the issue.
- Any proof-of-concept (PoC) code or minimal repro project (safe, non-destructive).
- Your contact information and whether you are willing to coordinate disclosure / be credited.
- Any suggested mitigations (if you have ideas).

If you need to share sensitive data (tokens, secret values) to reproduce the issue, ask for a secure channel and we will provide one.

## PGP Key
If you prefer to use PGP to encrypt your report, ask for our PGP public key via the private channel and we will provide it. When encrypting messages:
- Use the provided PGP public key.
- Include a clear subject line and reference this repository.
- Do not paste sensitive data into public issues.

## Response & Triage Timeline
We aim to handle security reports responsibly and promptly:
- Acknowledgement: within 48 hours of receiving the report.
- Initial triage (assessment of severity and reproducibility): within 5 business days.
- Patch / mitigation plan: we will aim to propose a remediation plan or timeline within 14 business days for medium/high severity issues. Urgent/critical issues may be acted on faster.
- Fix and release: timing depends on severity and complexity. We prioritize high/critical fixes and will coordinate timelines with the reporter for disclosure.

If you do not receive a response within the above windows, please follow up (reference your original report).

## Severity Classification & Public Disclosure
- Critical / High: remote code execution, authentication bypass, data exposure of production secrets, major integrity issues. These will be expedited.
- Medium: issues that can lead to significant impact with additional conditions.
- Low: minor issues, information leakage of non-sensitive data, or best-practice improvements.

We follow responsible disclosure:
- We will coordinate with you on a disclosure timeline.
- We intend to fix the issue before public disclosure whenever practicable.
- We will credit reporters who request attribution when the issue is disclosed publicly (unless reporter requests anonymity).
- If a CVE is warranted, we will request assignment (via GitHub or the appropriate CNA) and include the CVE in the advisory.

## Supported Versions / Scope
Report issues affecting code in this repository. For libraries or components with versioned releases, please indicate the specific version(s) affected. We will prioritize supported branches and actively maintained releases. If the vulnerability affects an upstream dependency, we will coordinate with upstream maintainers where possible.

## Security Best Practices for Contributors & Maintainers
- Never commit secrets (API keys, passwords, tokens) to the repository.
  - Use `dotnet user-secrets`, environment variables, or a secret manager (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault).
- Enable dependency scanning:
  - Use Dependabot or similar tooling to detect vulnerable dependencies.
- Run static analysis and SAST tools in CI (Roslyn analyzers, SonarQube, etc.).
- Avoid storing sensitive logs or PII. Mask or redact where necessary.
- Use least privilege for service accounts and rotate keys periodically.
- Use `IHttpClientFactory` for HTTP clients; prefer parameterized queries and ORM protections against injection.
- Add health checks and rate limiting protections for public endpoints as appropriate.

## Incident Response & Mitigation
If a security incident occurs (active exploitation, secrets leaked, production compromise):
1. Notify maintainers immediately via the private channel.
2. Preserve logs and evidence while avoiding further harm.
3. Rotate any affected secrets and credentials ASAP.
4. Coordinate public communication and remediation steps (we will work with reporters and stakeholders).

## Acknowledgments
We appreciate the security community and responsible reporters. Reporters who help improve our security posture and who opt-in may be acknowledged in security advisories and release notes.

## Legal & Safe-Testing Notes
- Do not access or exfiltrate data from production systems when testing.
- Ensure you have authorization to test systems and services.
- We expect responsible disclosure and reasonable testing practices. Illegal activity or data theft will be reported to the appropriate authorities.

## Contact / Maintainers
Preferred: create a private Security Advisory via the repository's Security tab.

Alternative: open a private or confidential issue prefixed with `[SECURITY]` and include "CONFIDENTIAL", then mention `@riteshsingh84`. We will respond and provide a secure channel if needed.

Thank you for helping keep this project safe.