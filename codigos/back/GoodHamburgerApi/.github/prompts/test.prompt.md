# /test

Validate implementation with automated tests.

## Rules
- Discover relevant tests first.
- Run focused tests, then broader scope when needed.
- Keep tests deterministic.
- Follow project testing preferences:
  - xUnit + Moq
  - no `DisplayName`
  - Fixture + Helper
  - scenario methods in fixture named `GerarCenario...`
  - scenario methods reset mocks at start

## Output format
- Test scope executed
- Result summary
- Failures (if any)
- Next action
