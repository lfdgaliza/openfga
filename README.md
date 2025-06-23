I'm using this repo to learn OpenFGA.

## Structure

### 1. basics/
Contains mostly code from [this](https://www.youtube.com/playlist?list=PLUR5l-oTFZqWaDdhEOVt_IfPOIbKo1Ypt) playlist with some additions from me.

* Every file x.0 is code from that playlist with tiny changes like comments or variable changes.
* Every file x.n where n > 0 is a use case made by me for some reason.

To test these files, run `fga model test --tests FILE.fga.yaml`

### 2. organized/
Takes the fine-grained API access example and splits it into separate files for better organization and maintainability:

- `1. schema.fga` - Schema definitions
- `2. tuples.yaml` - Tuple data  
- `3. tests.yaml` - Test cases

To test this organized structure, run `fga model test --tests '.\3. tests.fga.yaml'`

