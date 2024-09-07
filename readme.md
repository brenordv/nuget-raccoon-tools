# Racoon Ninja Tools
[![Publish](https://github.com/brenordv/nuget-raccoon-tools/actions/workflows/publish.yml/badge.svg)](https://github.com/brenordv/nuget-raccoon-tools/actions/workflows/publish.yml)
![GitHub Release](https://img.shields.io/github/v/release/brenordv/nuget-raccoon-tools)
![NuGet Version](https://img.shields.io/nuget/v/Raccoon.Ninja.Tools)
![NuGet Downloads](https://img.shields.io/nuget/dt/Raccoon.Ninja.Tools)
![GitHub last commit](https://img.shields.io/github/last-commit/brenordv/nuget-raccoon-tools)

---

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=bugs)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)

---

## Description
This is a collection of helpers and tools I find useful enough to reuse in multiple projects.
I hope this can help other people too. :) 

The idea of the package is to be lightweight and without external dependencies as much as possible. 
Right now, the project is fully standalone.

## Installation
You can install the package via NuGet using the following command:
```bash
dotnet add package Raccoon.Ninja.Tools
```

## Changelog
Check the [changelog](changelog.md) for the latest updates.

# Features
- [Deterministic GUID](./deterministic-guid.md) generation;
- [List extensions](./list-extensions.md) with useful methods like `ForEachWithIndex`, `RemoveDuplicates`, `PopFirst`, `IndexOfMax`, etc.;
- [String extensions](./string-extensions.md) with methods like `Minify`, `StripAccents`, `OnlyDigits`, etc.;
- [DateTime extensions](./datetime-extensions.md) with methods like `DaysSince`, etc.;
- [Operation Result](./operation-result.md) class to handle operations with success and error states with a functional approach;

# TODO
- [ ] Move common tests to the `ErrorPresets` class;
