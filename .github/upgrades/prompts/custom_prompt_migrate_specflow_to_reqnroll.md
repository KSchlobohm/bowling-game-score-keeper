# Migration Plan: SpecFlow ? Reqnroll

> Source of truth: Reqnroll’s official migration guide and docs. ([docs.reqnroll.net][1])

---

## 0) Overview & Strategy

There are **two supported paths**:

1. **Compatibility Package path** (fastest, few/no code changes) — swap NuGets, keep `TechTalk.SpecFlow` usings via a shim package. ([docs.reqnroll.net][1])
2. **Full Namespace path** (cleanest) — swap NuGets and **replace namespaces** (`TechTalk.SpecFlow` ? `Reqnroll`), plus a couple of symbol renames. ([docs.reqnroll.net][1])

> Reqnroll is based on **SpecFlow v4**; migrating from **SpecFlow v3** may surface v3?v4 breaking changes (Cucumber Expressions behavior, removed “call step from step”, etc.). ([docs.reqnroll.net][1])

This plan lets an AI automation tool choose either path per repo/solution and includes verifications, fallbacks, and CI notes.

---

## 1) Preconditions & Discovery (AI Tasks)

* **Detect test framework** per project: MSTest / NUnit / xUnit.

  * Look for `MSTest.TestFramework`, `nunit`, `xunit` refs in `.csproj`.
* **Detect SpecFlow usage**: any `SpecFlow.*` packages or `TechTalk.SpecFlow` in code.
* **Detect config**: `specflow.json` or legacy `App.config` SpecFlow section.
* **Detect “SpecFlow\.Actions.\*”** packages; note equivalent Reqnroll Actions shims. ([docs.reqnroll.net][1])
* **Detect BoDi usages** (`using BoDi;`, `IObjectContainer`). ([docs.reqnroll.net][1])
* **Detect ISpecFlowOutputHelper** usage (will be `IReqnrollOutputHelper` after full migration). ([docs.reqnroll.net][1])
* **Detect Scenario Outlines** if MSTest is used (row-test behavior nuance). ([docs.reqnroll.net][1])
* **Detect SpecFlow LivingDoc** usage (doc generation expectations). ([docs.reqnroll.net][1])

**Exit criteria:** Inventory JSON artifact listing projects, packages, config files, metrics above.

---

## 2) Package Changes (both paths)

For each SpecFlow test project:

* **Remove**: all `SpecFlow.*` & `CucumberExpressions.SpecFlow.*` packages. ([docs.reqnroll.net][1])
* **Add** (pick 1 per framework):

  * `Reqnroll.MsTest` **or** `Reqnroll.NUnit` **or** `Reqnroll.xUnit` (plus test SDK packages already in use). ([docs.reqnroll.net][1])
* **If Compatibility path**: also **add** `Reqnroll.SpecFlowCompatibility`.

  * If any `SpecFlow.Actions.*` were present, add the matching `Reqnroll.SpecFlowCompatibility.Actions.*` (e.g., Selenium). ([docs.reqnroll.net][1])

> The Reqnroll MSBuild generator replaces SpecFlow’s generator and handles `*.feature` ? `*.cs` generation. ([docs.reqnroll.net][2])

**Acceptance:** Project builds; feature-code-behind files generated.

---

## 3) Namespaces & Symbols (Full Namespace path only)

* **Replace all** `TechTalk.SpecFlow` ? `Reqnroll` (match case & whole word). ([docs.reqnroll.net][1])
* **BoDi** DI container namespace: `BoDi` ? `Reqnroll.BoDi`. ([docs.reqnroll.net][1])
* **Output helper**: `ISpecFlowOutputHelper` ? `IReqnrollOutputHelper`. ([docs.reqnroll.net][1])
* **Assist helpers**: main extensions live under `Reqnroll` now (using may simplify). ([docs.reqnroll.net][1])
* **Table alias**: `DataTable` alias exists; `Table` still works (no change required). ([docs.reqnroll.net][1])

**Acceptance:** Project compiles without `TechTalk.SpecFlow` or `BoDi` namespace errors.

---

## 4) Configuration Migration

* Reqnroll uses **`reqnroll.json`**; it also honors **`specflow.json`** unchanged. Quick path: keep `specflow.json` now; rename later. ([docs.reqnroll.net][1])
* **Full migration**: rename `specflow.json` ? `reqnroll.json` and update schema to
  `https://schemas.reqnroll.net/reqnroll-config-latest.json`.

  * `stepAssemblies` ? `bindingAssemblies` (rename recommended).
  * `bindingCulture/name` ? `language/binding`. ([docs.reqnroll.net][1])
* **If legacy `App.config`** was used, port settings into `reqnroll.json` per configuration docs. ([docs.reqnroll.net][1])

**Acceptance:** Running tests show configuration applied (e.g., language/binding, bindingAssemblies).

---

## 5) Breaking-Change Checks (v3?v4 lineage)

* **Cucumber Expressions detection**

  * If regex steps get misdetected as Cucumber Expressions, **force regex** with `^…$` in step attributes, or update to valid Cucumber Expressions, or adjust global detection per guide.
  * Bulk-fix regex markers with the documented regex replace, when needed. ([docs.reqnroll.net][1])
* **Removed “call step from step” API** (removed in v4; not available in Reqnroll). Refactor to helper methods. ([docs.reqnroll.net][1])

**Acceptance:** No “No matching step definition found” errors from mis-detected expressions; no usage of removed step-calling pattern remains.

---

## 6) MSTest Scenario Outlines (Row Tests)

* Reqnroll generates **data-driven row tests** for Scenario Outlines with MSTest. Some tooling (e.g., Azure Pipelines `VSTest` with distributed execution or filtering) may need adjustments; if incompatible, **set** `generator/allowRowTests=false` in `reqnroll.json` to revert to SpecFlow-like behavior. ([docs.reqnroll.net][1])

**Acceptance:** Scenario Outline examples execute as intended in local + CI runs; filtering/reporting behaves as required.

---

## 7) IDE & Plugins

* **Visual Studio extension**: Reqnroll VSIX handles both SpecFlow & Reqnroll projects and .NET 8. Install/verify. ([docs.reqnroll.net][1])
* **Plugins**: Replace SpecFlow-managed integrations with `Reqnroll.*` equivalents (e.g., Autofac). ([docs.reqnroll.net][1])
* **Living Documentation**: SpecFlow+ LivingDoc was closed-source; see the **workaround** thread for using the SpecFlow LivingDoc CLI with Reqnroll until a replacement is shipped. ([docs.reqnroll.net][1])

---

## 8) CI/CD Updates

* Ensure build agents restore Reqnroll packages and run the same test runners (dotnet test / vstest).
* For MSTest + Scenario Outlines, validate **filtering** and **distributed execution**; toggle `allowRowTests` if needed. ([docs.reqnroll.net][1])
* If LivingDoc reports are required, implement the current workaround process referenced above. ([docs.reqnroll.net][1])

**Acceptance:** Green pipeline across branches; reports generated as before (or an agreed replacement).

---

## 9) Rollout Plan

1. **Pilot** one solution using **Compatibility Package path** (fast validation).
2. If clean, choose **Full Namespace path** to remove shims and future-proof.
3. Roll to remaining solutions; keep a flag to **fall back** to compatibility package if any blockers surface.

---

## 10) Automated Transform Steps (suggested implementation)

> Pseudocode for the AI tool; apply per `*.csproj` in the solution.

1. **Swap NuGets**

   * Remove packages: `SpecFlow*`, `CucumberExpressions.SpecFlow.*`.
   * Add `Reqnroll.{MsTest|NUnit|xUnit}` (match current framework).
   * If path = Compatibility: add `Reqnroll.SpecFlowCompatibility`; also add `Reqnroll.SpecFlowCompatibility.Actions.*` for each `SpecFlow.Actions.*` found. ([docs.reqnroll.net][1])

2. **Namespace & symbol refactors** (Full Namespace path):

   * Replace `TechTalk.SpecFlow` ? `Reqnroll`.
   * Replace `using BoDi;` ? `using Reqnroll.BoDi;`.
   * Replace `ISpecFlowOutputHelper` ? `IReqnrollOutputHelper`. ([docs.reqnroll.net][1])

3. **Config**

   * If `specflow.json` exists:

     * Option A (quick): keep as-is (Reqnroll-compatible).
     * Option B (full): rename to `reqnroll.json`, add `$schema`, rename keys (`stepAssemblies` ? `bindingAssemblies`, `bindingCulture/name` ? `language/binding`). ([docs.reqnroll.net][1])
   * If `App.config` contains SpecFlow section: map to `reqnroll.json`.

4. **Cucumber Expressions hardening (if regex-first project)**

   * Bulk add `^…$` to `[Given|When|Then]("…")` attributes using the documented find/replace. ([docs.reqnroll.net][1])
   * Or set project policy to generate regex skeletons via:

     ````json
     {
       "trace": { "stepDefinitionSkeletonStyle": "RegexAttribute" }
     }
     ``` :contentReference[oaicite:33]{index=33}

     ````

5. **MSTest Scenario Outline choice** (if MSTest):

   * If CI tooling incompatible with row tests, set:

     ````json
     { "generator": { "allowRowTests": false } }
     ``` :contentReference[oaicite:34]{index=34}

     ````

6. **Build & Test**

   * `dotnet restore && dotnet build && dotnet test -v n`.

7. **Report & Gate**

   * Emit a summary (changed packages, files touched, config diffs, failing specs, suggested next actions).

---

## 11) Acceptance Checklist (per project)

* [ ] Swapped to `Reqnroll.*` test package; **no** `SpecFlow.*` packages remain. ([docs.reqnroll.net][1])
* [ ] Builds succeed; `.feature` files generate code-behind via Reqnroll MSBuild generator. ([docs.reqnroll.net][2])
* [ ] Tests run & pass locally; **Scenario Outlines** run as desired (row tests on/off per policy). ([docs.reqnroll.net][1])
* [ ] Config validated (`reqnroll.json` or compatible `specflow.json`); schema added if renamed. ([docs.reqnroll.net][1])
* [ ] Namespace & symbol refactors complete (if Full path): `TechTalk.SpecFlow` ? `Reqnroll`, `BoDi` ? `Reqnroll.BoDi`, `ISpecFlowOutputHelper` ? `IReqnrollOutputHelper`. ([docs.reqnroll.net][1])
* [ ] Any Cucumber Expression mis-detections resolved (markers or expression fixes). ([docs.reqnroll.net][1])
* [ ] CI green; filtering/distribution verified; LivingDoc strategy agreed/worked around. ([docs.reqnroll.net][1])

---

## 12) Notes & References

* **Official migration guide** (packages, namespaces, config, breaking changes, MSTest Outline behavior, Actions shims, VSIX, LivingDoc note). ([docs.reqnroll.net][1])
* **Guides index** (context & related topics). ([docs.reqnroll.net][3])
* **Reqnroll docs PDF** (MSBuild generator, packages). ([docs.reqnroll.net][2])
* **Background/why + reassurance of effort**. ([Reqnroll][4])

---

### Appendix A — Example `.csproj` fragment (MSTest + Compatibility path)

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
  <PackageReference Include="MSTest.TestAdapter" Version="3.2.0" />
  <PackageReference Include="MSTest.TestFramework" Version="3.2.0" />

  <PackageReference Include="Reqnroll.MsTest" Version="2.*" />
  <PackageReference Include="Reqnroll.SpecFlowCompatibility" Version="2.*" />
  <!-- If previously used SpecFlow.Actions.Selenium -->
  <PackageReference Include="Reqnroll.SpecFlowCompatibility.Actions.Selenium" Version="2.*" />
</ItemGroup>
```

(Structure per guide example.) ([docs.reqnroll.net][1])

---

### Appendix B — Bulk “force regex” replace (if needed)

* **Find (regex):** `\[(Given|When|Then)\((@?)"(.*?)"\)\]`
* **Replace:** `[$1($2"^$3$$")]`
  Adds `^…$` to all step attributes. ([docs.reqnroll.net][1])

---

### Appendix C — `reqnroll.json` schema & options (snippets)

```json
{
  "$schema": "https://schemas.reqnroll.net/reqnroll-config-latest.json",
  "language": { "feature": "en-US" },
  "bindingAssemblies": [{ "assembly": "ExternalStepDefs" }],
  "generator": { "allowRowTests": true },
  "trace": { "stepDefinitionSkeletonStyle": "RegexAttribute" }
}
```

(Keys & schema per guide.) ([docs.reqnroll.net][1])

---

**Done.** This file is ready for an automation to execute stepwise and report findings.

[1]: https://docs.reqnroll.net/latest/guides/migrating-from-specflow.html "Migrating from SpecFlow - Reqnroll Documentation"
[2]: https://docs.reqnroll.net/_/downloads/en/latest/pdf/?utm_source=chatgpt.com "Reqnroll Documentation"
[3]: https://docs.reqnroll.net/latest/guides/index.html?utm_source=chatgpt.com "Guides - Reqnroll Documentation"
[4]: https://reqnroll.net/news/2024/02/from-specflow-to-reqnroll-why-and-how/?utm_source=chatgpt.com "From SpecFlow to Reqnroll: Why and How | ..."
