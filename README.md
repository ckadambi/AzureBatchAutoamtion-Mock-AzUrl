# AzureBatchAutoamtion-Mock-AzUrl

[![.NET Build & Test](https://github.com/ckadambi/AzureBatchAutoamtion-Mock-AzUrl/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/ckadambi/AzureBatchAutoamtion-Mock-AzUrl/actions)
[![Codecov](https://codecov.io/gh/ckadambi/AzureBatchAutoamtion-Mock-AzUrl/branch/main/graph/badge.svg)](https://codecov.io/gh/ckadambi/AzureBatchAutoamtion-Mock-AzUrl)

This repository contains the Azure Batch Job Automation sample and tests. The GitHub Actions workflow builds the solution, runs tests (auto-discovered from the solution), collects code coverage, and publishes a concise test summary in the workflow run summary.

Notes:
- To enable Codecov uploads, add the `CODECOV_TOKEN` repository secret.
- Tests are discovered by running `dotnet test` on the solution; adding new test projects or test files will be picked up automatically.
