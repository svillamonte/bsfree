# BSFree

Frontend for the BSFree API.

[![Build status](https://muddlegrip.visualstudio.com/Muddlegrip/_apis/build/status/bsfree-CI)](https://muddlegrip.visualstudio.com/Muddlegrip/_build/latest?definitionId=4)

Single Page Application to display results from the [BSFree API](https://github.com/svillamonte/bsfree-api) _get-posts_ function.

## Architecture rundown

Built using the Blazor hosted approach:

- _BSFree_ is the SPA rendered client-side.
- _BSFree.Server_ handles communications with API and basic authentication.

Hosted on Azure App Services.
