# Deployment Instructions

1. Run Tests (execute from FloodGateSDK.Tests directory)

```
dotnet test
```

2. Increment the FloodgateSDK package and assembly version

3. Build library in Release mode

```
dotnet build -c Release
```

4. Deploy NuGet Package to [nuget.org](https://www.nuget.org/packages/FloodGateSDK/)