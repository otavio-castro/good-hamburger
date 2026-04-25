# Copilot Instructions (Spec-Driven)

You are working in a .NET 10 solution.

## Working model
Use a **spec-driven** workflow with these stages:
1. `/plan`
2. `/exec`
3. `/test`

Do not skip stages unless the user explicitly asks to skip.

## Global rules
- Keep changes minimal and focused on the requested task.
- Follow existing project style and naming.
- Do not introduce unrelated refactors.
- Prefer clear, small commits/patches by concern.
- Keep responses short and practical.

## API rules
- Preserve business rules and validation messages.
- Prefer service + repository separation already used in this solution.
- Keep controllers thin.

## Testing rules (important)
- Use xUnit + Moq.
- Keep tests simple.
- **Do not use `DisplayName` in `[Fact]`.**
- Organize tests with `#region` for flows and AAA sections.
- Use Fixture + Helper pattern.
- If a test needs multiple mock configs, create a **single** scenario method in fixture:
  - Name format: `GerarCenario...`
  - Scenario method must reset mocks at start.
- Test files should not manually reset mocks.
- Avoid direct setup in test method when scenario setup can live in fixture.

## Cardápio test preference
- Do not manipulate real file system paths in tests.
- Mock the file provider behavior.
- Use in-memory fake menu generation helper (for example `GerarCardapioMock`).

## Language
- Write code and comments in English when creating new configuration/spec files.
