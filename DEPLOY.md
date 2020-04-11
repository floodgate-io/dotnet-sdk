# Deployment Instructions

1. Run Tests (execute from FloodGateSDK.Tests directory)

```
dotnet test
```

2. Update `.csproj` file as follows

- Increment FloodgateSDK package and assembly version
- Update release notes as required

3. Build library in Release mode

```
dotnet build -c Release
```

> The above command created the necessary NuGet package automatically

4. Deploy NuGet Package to [nuget.org](https://www.nuget.org/packages/FloodGateSDK/)
